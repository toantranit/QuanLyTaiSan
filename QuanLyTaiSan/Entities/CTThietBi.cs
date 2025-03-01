﻿using PTB.Libraries;
using SHARED;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using PTB.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHARED.Libraries;

namespace PTB.Entities
{
    [Table("CTTHIETBIS")]
    public class CTThietBi:_EntityAbstract2<CTThietBi>
    {
        public CTThietBi():base()
        {
            
        }

        #region Dinh nghia
        public DateTime? ngay { get; set; }
        [Required]
        public int soluong { get; set; }
        /*
         * FK
         */
        public Guid phong_id { get; set; }
        [Required]
        [ForeignKey("phong_id")]
        public virtual Phong phong { get; set; }

        public Guid thietbi_id { get; set; }        
        [Required]
        [ForeignKey("thietbi_id")]
        public virtual ThietBi thietbi { get; set; }

        public Guid tinhtrang_id { get; set; }
        [Required]
        [ForeignKey("tinhtrang_id")]
        public virtual TinhTrang tinhtrang { get; set; }
        #endregion
        #region Nghiep vu
        /// <summary>
        /// Readonly
        /// </summary>
        [NotMapped]
        public ICollection<LogThietBi> logthietbis
        {
            get
            {
                try
                {
                    return this.thietbi.logthietbis.Where(c => c.phong_id == this.phong_id).ToList();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new List<LogThietBi>();
                }
            }
        }
        /// <summary>
        /// Di chuyển, kết hợp đổi tình trạng,
        /// có hỗ trợ ghi LOG tự động,vd:
        /// CTThietBi obj = CTThietBi.getById(24552);
        /// Phong dich = null;// Phong.getById(1228);
        /// TinhTrang ttr = TinhTrang.getById(3);
        /// int re = obj.dichuyen(dich, ttr, -1, "đổi tình trạng toàn bộ luôn");
        /// </summary>
        /// <param name="dich">Phòng cần di chuyển đến (null nếu chỉ muốn đổi tình trạng)</param>
        /// <param name="ttmoi">Tình trạng cần chuyển sang (null nếu chỉ muốn đổi phòng)</param>
        /// <param name="soluong">Số lượng cần chuyển (mac dinh la -1 (chuyển tất cả))</param>
        /// /// <param name="mota">Mô tả cho quá trình di chuyển</param>
        /// <param name="hinhs">Hình mô tả cho quá trình di chuyển (Hình sạch)</param>
        /// <returns></returns>
        public int dichuyen(Phong dich=null, TinhTrang ttmoi=null, int soluong=-1, String mota="", List<HinhAnh> hinhs=null, DateTime? ngay=null)
        {
            try
            {
                //pre set data
                dich = dich == null ? this.phong : dich;
                //=> dich co the van se la null (do this.phong có thể là null)
                ttmoi = ttmoi == null ? this.tinhtrang : ttmoi;//tinh trang không thể null
                ngay = ngay == null ? ServerTimeHelper.getNow() : ngay;
                //XÉT ĐIỀU KIỆN
                if
                (
                    //Nếu Không có bất kỳ sự thay đổi nào, phòng và tình trạng giống với this
                    ((dich == null && this.phong == null) || (dich != null && dich.id == this.phong.id))
                    && ttmoi.id == this.tinhtrang.id
                )
                {
                    return -2;
                }
                //kiem tra rang buoc không cho thực thi
                if
                (
                    soluong == 0 || soluong > this.soluong
                )
                {
                    return -2;
                }
                soluong = soluong < 0 ? this.soluong : soluong;

                //tao hoac cap nhat mot CTTB moi cho PHONG moi (dich)
                //kiem tra co record nao trung với record cần tạo mới (dich, tinhtrang, thietbi) ?
                CTThietBi tmp = search(dich, this.thietbi, ttmoi);

                //NO
                //TAO MOI CTTB => add
                if (tmp == null)
                {
                    tmp = new CTThietBi();
                    tmp.phong = dich;
                    tmp.soluong = soluong;
                    tmp.thietbi = this.thietbi;
                    tmp.tinhtrang = ttmoi;
                    tmp.mota = mota;
                    tmp.hinhanhs = hinhs;
                    tmp.ngay = ngay;
                    tmp.add();
                }
                else
                {
                    //Đã có CTTB sẵn giống với CTTB cần tạo mới
                    //SELECT CTTB do len => update
                    if (tmp.id != this.id)
                    {
                        tmp.soluong += soluong;
                        tmp.mota = mota;
                        tmp.hinhanhs = hinhs;
                        tmp.ngay = ngay;
                        tmp.update();
                    }
                }

                //cap nhat lai so luong cho cái hiện đã bị chuyển
                this.mota = mota;
                this.soluong -= soluong;
                this.soluong = this.soluong < 0 ? 0 : this.soluong;//for sure
                this.hinhanhs = hinhs;
                //ghi log thietbi ngay sau khi cap nhat ONLY soluong
                this.update();
                return 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }
        /// <summary>
        /// Kich hoat ham ghi log vao LogThietBi
        /// </summary>
        private int writelog()
        {
            try
            {
                //ghi log thiet bi
                LogThietBi logtb = new LogThietBi();
                logtb.mota = this.mota;
                logtb.phong = this.phong;
                logtb.soluong = this.soluong;
                logtb.thietbi = this.thietbi;
                logtb.tinhtrang = this.tinhtrang;
                logtb.hinhanhs = hinhanhs;
                logtb.quantrivien = Global.current_quantrivien_login;

                //call manual because of 2nd SaveChanges
                logtb.onBeforeAdded();
                return logtb.add();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }
        /// <summary>
        /// Tìm kiếm thiết bị với 3 yếu tố
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="tb"></param>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static CTThietBi search(Phong ph, ThietBi tb, TinhTrang tr)
        {
            try
            {
                IQueryable<CTThietBi> query = db.CTTHIETBIS.AsQueryable();
                if (ph == null)
                {
                    query = query.Where(c => c.phong_id == null);
                }
                else
                {
                    query = query.Where(c => c.phong_id == ph.id);
                }
                query = query.Where(c => c.thietbi_id == tb.id && c.tinhtrang_id == tr.id);

                return query.FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
        
        #endregion

        #region Override method
        public override void onAfterAdded()
        {
            writelog();
            base.onAfterAdded();
        }
        public override void onAfterUpdated()
        {
            writelog();
            base.onAfterUpdated();
        }
        public override void onBeforeUpdated()
        {
            if (phong != null)
            {
                phong.trigger();
            }
            if (thietbi != null)
            {
                thietbi.trigger();
            }
            if (tinhtrang != null)
            {
                tinhtrang.trigger();
            }

            base.onBeforeUpdated();
        }
        //quocdunginfo fail niceName validation ERROR

        public override string niceName()
        {
            try
            {
                return soluong + " " + thietbi.niceName() + ", " + phong.niceName() + ", " + tinhtrang.niceName();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "ERROR";
            }
        }
        /// <summary>
        /// Có hỗ trợ ghi log, phát sinh tự động thietbi hay không ? dựa trên loaichung hay riêng của loaithietbi,
        /// Tự động có transaction
        /// A. Trường hợp thêm Thiết bị vào phòng mà chỉ chọn: Loại thiết bị
        /// 
        /// CTThietBi obj = new CTThietBi();
        /// obj.thietbi = ThietBi.request(loaithietbi);
        /// obj.phong = phong;
        /// obj.tinhtrang = tinhtrang;
        /// obj.soluong=soluong;
        /// obj.mota=mota;
        /// obj.hinhanhs = hinhs;
        /// obj.add();
        /// 
        /// B. Trường hợp thêm Thiết bị vào phòng chọn được: Thiết bị
        /// 
        /// CTThietBi obj = new CTThietBi();
        /// obj.thietbi = thietbi;
        /// obj.phong = phong;
        /// obj.tinhtrang = tinhtrang;
        /// obj.soluong=soluong;
        /// obj.mota=mota;
        /// obj.hinhanh = hinhs;//Hình sạch
        /// obj.add();
        /// </summary>
        /// <returns></returns>
        public override int add()
        {
            try
            {
                this.ngay = this.ngay == null ? ServerTimeHelper.getNow() : this.ngay;
                //SCRIPT
                CTThietBi tmp = search(phong, thietbi, tinhtrang);
                //Nếu có CTTB sẵn trùng Phòng, Thiết bị, Tình trạng thì cộng dồn SL vào và update
                if (tmp != null)
                {
                    tmp.soluong += soluong;
                    tmp.ngay = this.ngay;
                    tmp.mota = this.mota;
                    tmp.hinhanhs = hinhanhs;
                    //call update on tmp
                    tmp.update();
                    //id = tmp.id;
                }
                else
                {
                    base.add();
                }
                return 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }
        public override int delete()
        {
            try
            {
                //trước khi delete phải ghi log
                Phong backup = Phong.getById(phong.id);
                this.phong = null;

                writelog();

                this.phong = backup;
                return base.delete();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }

        public override void doTrigger()
        {
            if (phong != null)
            {
                phong.trigger();
            }
            if (thietbi != null)
            {
                thietbi.trigger();
            }
            if (tinhtrang != null)
            {
                tinhtrang.trigger();
            }
            base.doTrigger();
        }
        #endregion
    }
}
