﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SHARED.Libraries;

namespace TSCD.Entities
{
    [Table("CHUNGTUS")]
    public class ChungTu:_EntityAbstract1<ChungTu>
    {
        public ChungTu()
        {

        }
        public String sohieu { get; set; }
        public DateTime? ngay { get; set; }
        /*
         * FK
         */
        public virtual ICollection<Attachment> attachments { get; set; }
        public virtual ICollection<CTTaiSan> cttaisans { get; set; }
        public virtual ICollection<LogSuaTaiSan> logsuataisans { get; set; }
        public virtual ICollection<LogTangGiamTaiSan> logtanggiamtaisans { get; set; }
        #region Nghiep vu
        public event SHARED.Libraries.FTPHelper.UploadProgress onUploadProgress;
        /// <summary>
        /// Upload tất cả this.attachments lên FTP server
        /// </summary>
        /// <returns></returns>
        public async Task<int> upload(CancellationToken cancel=new CancellationToken())
        {
            try
            {
                Boolean re = true;
                total_size = ATTACHMENTS_SIZE;
                foreach (var item in attachments)
                {
                    if (item.id != Guid.Empty)
                        continue;
                    item.onUploadProgress += new SHARED.Libraries.FTPHelper.UploadProgress(this.onOneFileUploading);
                    re = re && (await item.upload(cancel)) > 0;
                }
                current_size = 0;
                total_size = 0;
                return re ? 1 : -1;
            }catch(Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }
        private long total_size = 0;
        private long current_size = 0;
        private void onOneFileUploading(long current, long total)
        {
            current_size += FTPHelper.UPLOAD_BUFFER/1024;
            if(onUploadProgress!=null)
            {
                onUploadProgress(current_size, total_size);
            }
        }
        public long ATTACHMENTS_SIZE
        {
            get
            {
                long tmp = 0;
                foreach(var item in attachments)
                {
                    tmp += item.FILE_SIZE_KILOBYTE;
                }
                return tmp;
            }
        }
        #endregion
        #region override
        protected override void init()
        {
            base.init();
            attachments = new List<Attachment>();
        }
        public override int delete()
        {
            while(attachments.Count>0)
            {
                attachments.FirstOrDefault().delete();
            }
            return base.delete();
        }
        #endregion
    }
}
