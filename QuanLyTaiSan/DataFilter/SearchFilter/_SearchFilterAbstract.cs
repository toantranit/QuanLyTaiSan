﻿using SHARED.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyTaiSan.DataFilter.SearchFilter
{
    public abstract class _SearchFilterAbstract<T>:FilterAbstract<T>
    {
        public T obj { get; set; }

        private String _master_key = "";
        protected String master_key
        {
            get
            {
                return _master_key;
            }
            set
            {
                _master_key = input_filter(value);
            }
        }

        protected static String input_filter(String key_word="")
        {
            return StringHelper.CoDauThanhKhongDau(key_word == null?"":key_word.ToLower());
        }
    }
}
