﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.DataFilter
{
    public class ChiTietTBHienThi : _FilterAbstract<ChiTietTBHienThi>
    {
        public Guid id { get; set; }
        public Guid idTB { get; set; }
        public String subId { get; set; }
        public String ten { get; set; }
        public String tinhtrang { get; set; }
        public int soluong { get; set; }
        public String tenloai { get; set; }
        public String kieuQL { get; set; }
        public PTB.Entities.CTThietBi ctthietbi { get; set; }

        #region Nghiệp vụ
        public static List<ChiTietTBHienThi> getAllByPhongId(Guid _phongid)
        {
            //OurDBContext db = new OurDBContext();
            List<ChiTietTBHienThi> re =
                (from c in db.CTTHIETBIS
                 where (c.phong.id == _phongid && c.soluong > 0)
                 select new ChiTietTBHienThi
                 {
                     id = c.id,
                     idTB = c.thietbi.id,
                     subId = c.thietbi.subId,
                     ten = c.thietbi.ten,
                     tinhtrang = c.tinhtrang.value,
                     soluong = c.soluong,
                     tenloai = c.thietbi.loaithietbi.ten,
                     kieuQL = c.thietbi.loaithietbi.loaichung == true ? "Theo số lượng" : "Theo cá thể",
                     ctthietbi = c
                 }).OrderBy(p => p.ten).ToList();
            return re;
        }
        #endregion
    }
}
