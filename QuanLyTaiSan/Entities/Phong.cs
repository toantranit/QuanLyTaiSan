﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTaiSan.Entities
{
    [Table("PHONGS")]
    public class Phong:_EntityAbstract<Phong>
    {
        public Phong()
        {
            this.thietbis = new List<ThietBi>();
        }
        /*
         * FK
         */
        [Required]
        public virtual ViTri vitri { get; set; }
        public virtual ICollection<ThietBi> thietbis { get; set; }
        public virtual NhanVienPT nhanvienpt { get; set; }
    }
}
