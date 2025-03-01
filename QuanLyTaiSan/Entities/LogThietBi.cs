﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.Entities
{
    /*
     * Log thiet bi, phuc vu thong ke
     */
    [Table("LOGTHIETBIS")]
    public class LogThietBi : _EntityAbstract2<LogThietBi>
    {
        public LogThietBi():base()
        {

        }
        #region Dinh nghia

        [Required]
        public int soluong { get; set; }

        /*
         * FK
         */
        public Guid thietbi_id { get; set; }
        [Required]
        [ForeignKey("thietbi_id")]
        public virtual ThietBi thietbi { get; set; }

        public Guid tinhtrang_id { get; set; }
        [Required]
        [ForeignKey("tinhtrang_id")]
        public virtual TinhTrang tinhtrang { get; set; }

        public Guid? phong_id { get; set; }
        [ForeignKey("phong_id")]
        public virtual Phong phong { get; set; }

        public Guid? quantrivien_id { get; set; }
        [ForeignKey("quantrivien_id")]
        public virtual QuanTriVien quantrivien { get; set; }
		#endregion

        #region Override method
        /// <summary>
        /// LOG THIET BỊ KHÔNG CÓ UPDATE
        /// </summary>
        /// <returns></returns>
        public override int update()
        {
            return -1;
        }
        public override void doTrigger()
        {
            if (tinhtrang != null)
            {
                tinhtrang.trigger();
            }
            if (thietbi != null)
            {
                thietbi.trigger();
            }
            if (phong != null)
            {
                phong.trigger();
            }
            if (quantrivien != null)
            {
                quantrivien.trigger();
            }
            base.doTrigger();
        }
        #endregion
    }
}
