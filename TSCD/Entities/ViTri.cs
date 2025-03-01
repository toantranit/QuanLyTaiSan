﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCD.Entities
{
    [Table("VITRIS")]
    public class ViTri : _EntityAbstract1<ViTri>
    {
        public ViTri():base()
        {
            
        }
        #region Dinh nghia        
        
        public Guid coso_id { get; set; }
        [Index("nothing", 1, IsUnique = true)]
        [ForeignKey("coso_id")]
        [Required]
        public virtual CoSo coso { get; set; }

        public Guid? day_id { get; set; }
        [Index("nothing", 2, IsUnique = true)]
        [ForeignKey("day_id")]
        public virtual Dayy day { get; set; }

        public Guid? tang_id { get; set; }
        [Index("nothing", 3, IsUnique = true)]
        [ForeignKey("tang_id")]
        public virtual Tang tang { get; set; }
        /*
         * FK 
         */
        public virtual ICollection<Phong> phongs { get; set; }
        #endregion
        #region Nghiệp vụ
        /// <summary>
        /// Hàm dùng tạm để test du liệu
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="id3"></param>
        /// <returns></returns>

        public ViTri getBy3Id(Guid id1, Guid id2, Guid id3)
        {
            try
            {
                //initDb();
                ViTri obj = null;
                if (id3 != Guid.Empty)
                {
                    obj = db.Set<ViTri>().Where(c => c.coso.id == id1 && c.day.id == id2 && c.tang.id == id3).FirstOrDefault();
                }
                else if (id2 != Guid.Empty)
                {
                    obj = db.Set<ViTri>().Where(c => c.coso.id == id1 && c.day.id == id2 && c.tang.id == null).FirstOrDefault();
                }
                else
                {
                    obj = db.Set<ViTri>().Where(c => c.coso.id == id1 && c.day.id == null && c.tang.id == null).FirstOrDefault();
                }
                //if (obj != null)
                //{
                //    obj.DB = db;
                //}
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }
        }
        public List<ViTri> search(CoSo coso, Dayy day, Tang tang)
        {
            //initDb();
            List<ViTri> tmp = new List<ViTri>(); ;
            if(coso==null && day==null && tang==null)
            {
                tmp = db.VITRIS.Where(c => c.coso==null && c.day==null && c.tang==null).ToList();
            }
            else if(coso!=null && day==null && tang==null)
            {
                tmp = db.VITRIS.Where(c => c.coso.id==coso.id && c.day==null && c.tang==null).ToList();
            }
            else if (coso != null && day != null && tang == null)
            {
                tmp = db.VITRIS.Where(c => c.coso.id == coso.id && c.day.id == day.id && c.tang == null).ToList();
            }
            else if (coso != null && day != null && tang != null)
            {
                tmp = db.VITRIS.Where(c => c.coso.id == coso.id && c.day.id == day.id && c.tang.id == tang.id).ToList();
            }
            //if (tmp != null)
            //{
            //    foreach (ViTri item in tmp)
            //    {
            //        item.DB = db;
            //    }
            //}
            return tmp;
        }
        public List<ViTri> search(Guid coso_id, Guid day_id, Guid tang_id)
        {
            //initDb();
            List<ViTri> tmp = new List<ViTri>(); ;
            //CoSo coso = new CoSo();
            coso = CoSo.getById(coso_id);
            //Dayy day = new Dayy();
            day = Dayy.getById(day_id);
            //Tang tang = new Tang();
            tang = Tang.getById(tang_id);
            return search(coso, day, tang);
        }
        /// <summary>
        /// Trả về 
        /// </summary>
        /// <param name="coso"></param>
        /// <param name="day"></param>
        /// <param name="tang"></param>
        /// <returns></returns>
        public static ViTri request(CoSo coso, Dayy day, Tang tang)
        {
            if (coso == null && day == null && tang == null)
            {
                return null;
            }
            ViTri final = new ViTri();
            if (tang != null)
            {
                ViTri tmp = db.VITRIS.Where(c => c.tang.id == tang.id).FirstOrDefault();
                if (tmp == null)
                {
                    final.tang = tang;
                    final.day = tang.day;
                    final.coso = tang.day.coso;
                    return final;
                }
                else
                    return tmp;
            }
            else if(day!=null)
            {
                ViTri tmp = db.VITRIS.Where(c => c.day.id == day.id && c.tang.id==null).FirstOrDefault();
                if (tmp == null)
                {
                    final.tang = null;
                    final.day = day;
                    final.coso = day.coso;
                    return final;
                }
                else
                    return tmp;
            }
            else if (coso != null)
            {
                ViTri tmp = db.VITRIS.Where(c => c.coso.id == coso.id && c.day.id==null && c.tang.id == null).FirstOrDefault();
                if (tmp == null)
                {
                    final.tang = null;
                    final.day = null;
                    final.coso = coso;
                    return final;
                }
                else
                    return tmp;
            }
            return null;
        }

        #endregion
		#region Override method
        public override string niceName()
        {
            String tmp = "";
            if (tang != null)
            {
                tmp += tang.niceName();
            }
            else if(day!=null)
            {
                tmp += day.niceName();
            }
            else if (coso != null)
            {
                tmp += coso.niceName();
            }
            return tmp;
        }
        /// <summary>
        /// Không cho sửa vị trí
        /// </summary>
        /// <returns></returns>
        public override int update()
        {
            return -1;
        }
        /// <summary>
        /// -2: Có phòng sử dụng vị trí
        /// </summary>
        /// <returns></returns>
        public override int delete()
        {
            if (phongs.Count > 0)
            {
                return -2;
            }
            return base.delete();
        }
        public override void doTrigger()
        {
            if(coso!=null)
            {
                coso.trigger();
            }
            if (day != null)
            {
                day.trigger();
            }
            if (tang != null)
            {
                tang.trigger();
            }
            base.doTrigger();
        }
        #endregion
    }
}
