﻿using PTB.Entities;
using SHARED.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTB.DataFilter.SearchFilter
{
    public class LoaiThietBiSF : _SearchFilterAbstract<LoaiThietBi>
    {
        public LoaiThietBiSF(Boolean enable_filter_input = true)
        {
            this.enable_filter_input = enable_filter_input;
        }
        private String _ten = "";
        public String ten { get { return _ten; } set { _ten = input_filter(value, enable_filter_input); } }
        public static List<LoaiThietBiSF> search(String key_work)
        {
            var re = new List<LoaiThietBiSF>();
            IEnumerable<LoaiThietBiSF> query;
            Boolean search_codau = StringHelper.isCoDau(key_work);
            //Đang search có dấu
            key_work = input_filter(key_work, !search_codau);
            if (key_work.Length == 0)
            {
                return new List<LoaiThietBiSF>();
            }
            query = LoaiThietBi.getAll().Select(c => new LoaiThietBiSF(!search_codau) { obj = c, ten = c.ten });
            

            Boolean once_match = false;
            foreach (var item in query)
            {
                if (item.ten.Contains(key_work))
                {
                    item.match_field.Add("ten");
                    once_match = true;
                }
                if (once_match)
                {
                    re.Add(item);
                    once_match = false;
                }
            }
            return re;
        }
    }
}
