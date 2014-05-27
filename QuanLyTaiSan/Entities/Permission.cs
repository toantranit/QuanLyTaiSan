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
    [Table("PERMISSIONS")]
    public class Permission
    {
        public Permission()
        {
            this.groups = new List<Group>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public String key { get; set; }
        public String mota { get; set; }
        /*
         * FK
         */
        public virtual ICollection<Group> groups { get; set; }
        /*
         * Manual method
         */
        public Boolean isInGroup(Group obj)
        {
            if(obj==null || obj.permissions==null)
            {
                return false;
            }
            foreach (Permission item in obj.permissions)
            {
                if (item.key.Equals(this.key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
