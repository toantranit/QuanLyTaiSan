﻿using PTB.Libraries;
using SHARED;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using PTB.Entities;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using SHARED.Libraries;
using SHARED.Interface;

namespace PTB.Entities
{
    /// <summary>
    /// Lúc gán kiểu obj.hinhanhs = list;
    /// là lúc Entity State bị đổi
    /// </summary>
    public class OurDBContext : DbContext
    {
        /// <summary>
        /// Có tự động tạo "cấu trúc CSDL" hay không ?
        /// </summary>
        private Boolean create_db_structure = false;
        /// <summary>
        /// Hàm khởi tạo mặc định sử dụng App.config
        /// </summary>
        public OurDBContext()
            : base("Default")
        {
            try
            {
                if (create_db_structure)
                {
                    //Auto create DB Structure 
                    Database.SetInitializer<OurDBContext>(new CreateDatabaseIfNotExists<OurDBContext>());
                    //FORCE CREATE TABLE if Database Existed
                    Database.Initialize(true);
                }
                else
                {
                    Database.SetInitializer<OurDBContext>(null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        
        /// <summary>
        /// Hàm khởi tạo có chỉ định cụ thể DB đích
        /// </summary>
        /// <param name="connection_string"></param>
        /// <param name="create_db_structure">Có tự động tạo "cấu trúc CSDL" và "dữ liệu mẫu" hay không ?</param>
        public OurDBContext(String connection_string = "Default", Boolean create_db_structure = false)
            : base(connection_string)
        {
            try
            {
                this.create_db_structure = create_db_structure;
                if (create_db_structure)
                {
                    //Auto create DB Structure
                    Database.SetInitializer<OurDBContext>(new CreateDatabaseIfNotExists<OurDBContext>());
                    //FORCE CREATE TABLE if Database Existed
                    //May through Exeption but FINE
                    Database.Initialize(true);
                }
                else
                {
                    Database.SetInitializer<OurDBContext>(null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #region COMMON (SHARED)
        public DbSet<QuanTriVien> QUANTRIVIENS { get; set; }
        public DbSet<Group> GROUPS { get; set; }
        public DbSet<Permission> PERMISSIONS { get; set; }
        public DbSet<HinhAnh> HINHANHS { get; set; }
        public DbSet<TinhTrang> TINHTRANGS { get; set; }
        public DbSet<Setting> SETTINGS { get; set; }
        public DbSet<LogHeThong> LOGHETHONGS { get; set; }
        #endregion

        #region QL: PHONG~THIETBI
        public DbSet<CoSo> COSOS { get; set; }
        public DbSet<Phong> PHONGS { get; set; }
        public DbSet<ThietBi> THIETBIS { get; set; }
        public DbSet<CTThietBi> CTTHIETBIS { get; set; }
        public DbSet<LogThietBi> LOGTHIETBIS { get; set; }
        public DbSet<SuCoPhong> SUCOPHONGS { get; set; }
        public DbSet<LogSuCoPhong> LOGSUCOPHONGS { get; set; }
        public DbSet<NhanVienPT> NHANVIENPTS { get; set; }
        public DbSet<Tang> TANGS { get; set; }
        public DbSet<Dayy> DAYYS { get; set; }
        public DbSet<ViTri> VITRIS { get; set; }
        public DbSet<LoaiThietBi> LOAITHIETBIS { get; set; }
        public DbSet<PhieuMuonPhong> PHIEUMUONPHONGS { get; set; }
        #endregion

        #region STATIC
        /// <summary>
        /// Dùng trong frm Sửa quyền, thuws tujw k quan trong
        /// </summary>
        public static String[] permission_entity_list =
        {
            CoSo.USNAME, Dayy.USNAME, Tang.USNAME, Phong.USNAME, ThietBi.USNAME, LoaiThietBi.USNAME, NhanVienPT.USNAME, QuanTriVien.USNAME, Group.USNAME, TinhTrang.USNAME, SuCoPhong.USNAME, HinhAnh.USNAME, LogHeThong.USNAME
        };
        /// <summary>
        /// for SYNC, mỗi lần đổi cấu trúc là phải set up SCOPE lại
        /// </summary>

        public static String[] tracking_tables =
        {
            //UNDEPENDENT (bản thân không có bất kỳ FK nào)
            //TABLES HAVE TO BE IN RIGHT ORDER FOR FK CONSTRAIN
            
            //---DO NOT TRACK __MigrationHistory because of init state on 2 side client-server
            //"__MigrationHistory",//UNDEPENDENT
            "SETTINGS",//UNDEPENDENT
            "GROUPS",//UNDEPENDENT
                "QUANTRIVIENS",
            "NHANVIENPTS",//UNDEPENDENT
            "TINHTRANGS",//UNDEPENDENT
            //BEGIN PTB
            "COSOS",//UNDEPENDENT
                "DAYS",
                    "TANGS",
                        "VITRIS",
                            "PHONGS",
            "LOAITHIETBIS",//UNDEPENDENT
                "THIETBIS",
                    "CTTHIETBIS",
                
            "PERMISSIONS",//UNDEPENDENT
                "GROUP_PERMISSION",
                "COSO_PERMISSION",
                "DAY_PERMISSION",
                "TANG_PERMISSION",
                "PHONG_PERMISSION",
            
            "PHIEUMUONPHONGS",
            "LOGTHIETBIS",

            "SUCOPHONGS",
                "LOGSUCOPHONGS",
            
            "HINHANHS",//UNDEPENDENT
                "COSO_HINHANH",
                "DAY_HINHANH",
                "TANG_HINHANH",
                "THIETBI_HINHANH",
                "CTTHIETBI_HINHANH",
                "LOGSUCOPHONG_HINHANH",
                "LOGTHIETBI_HINHANH",
                "NHANVIENPT_HINHANH",
                "PHONG_HINHANH",
                "SUCOPHONG_HINHANH",

            "LOGHETHONGS",//UNDEPENDENT
            //END PTB
        };
        #endregion
        #region Manual
        /// <summary>
        /// Lấy danh sách lỗi sau khi add/update được gọi,
        /// dùng để phục vụ mục đích hiển thị lỗi
        /// </summary>
        public ICollection<DbValidationError> ERRORs
        {
            get
            {
                List<DbValidationError> re = new List<DbValidationError>();
                foreach (DbEntityValidationResult tmp in DBInstance.DB.GetValidationErrors())
                {
                    re.AddRange(tmp.ValidationErrors);
                }
                return re;
            }
        }
        /// <summary>
        /// Kiem tra model backing changed,
        /// CSDL Sẽ tạo tự động nếu chưa có (Chưa hay lắm)
        /// </summary>
        /// <returns></returns>
        public bool isValidModel()
        {
            try
            {
                COSOS.Find(Guid.Empty);
                DAYYS.Find(Guid.Empty);
                TANGS.Find(Guid.Empty);
                VITRIS.Find(Guid.Empty);
                PHONGS.Find(Guid.Empty);
                THIETBIS.Find(Guid.Empty);
                CTTHIETBIS.Find(Guid.Empty);
                HINHANHS.Find(Guid.Empty);
                QUANTRIVIENS.Find(Guid.Empty);
                NHANVIENPTS.Find(Guid.Empty);
                LOGHETHONGS.Find(Guid.Empty);
                LOGSUCOPHONGS.Find(Guid.Empty);
                LOGTHIETBIS.Find(Guid.Empty);
                LOAITHIETBIS.Find(Guid.Empty);
                SETTINGS.Find(Guid.Empty);
                TINHTRANGS.Find(Guid.Empty);
                PERMISSIONS.Find(Guid.Empty);
                SUCOPHONGS.Find(Guid.Empty);
                GROUPS.Find(Guid.Empty);
                PHIEUMUONPHONGS.Find(Guid.Empty);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        #endregion
        #region Override

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //CONFIG
            //DISABLE AUTO DELETE ON CASCADE
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //FORCE USE LAZY LOADING
            this.Configuration.LazyLoadingEnabled = true;
            /*
             * TPC Mapping, Inheritance
             */
            #region COMMON (SHARED)
            modelBuilder.Entity<HinhAnh>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            modelBuilder.Entity<QuanTriVien>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<Group>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<Permission>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            modelBuilder.Entity<LogHeThong>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            modelBuilder.Entity<TinhTrang>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            modelBuilder.Entity<Setting>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            /*
             * n-n relationship GROUP~PERMISSION,
             * Định nghĩa chi tiết
             */
            modelBuilder.Entity<Group>().
            HasMany(c => c.permissions).
            WithMany(p => p.groups).
            Map(
                m =>
                {
                    m.MapLeftKey("group_id");
                    m.MapRightKey("permission_id");
                    m.ToTable("GROUP_PERMISSION");
                });
            #endregion

            #region QL: PHONG~THIETBI
            modelBuilder.Entity<CoSo>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<Phong>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<ThietBi>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<LogThietBi>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<CTThietBi>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<NhanVienPT>().Map(x =>
            {
                x.MapInheritedProperties();
            });
            modelBuilder.Entity<Tang>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<Dayy>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<ViTri>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<LoaiThietBi>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<SuCoPhong>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<LogSuCoPhong>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            modelBuilder.Entity<PhieuMuonPhong>().Map(x =>
            {
                x.MapInheritedProperties();
            });

            /*
             * Double 1-n relationship PHIEUMUONPHONG~QUANTRIVIEN
             */
            modelBuilder.Entity<PhieuMuonPhong>()
            .HasRequired(a => a.nguoimuon)
            .WithMany(b => b.phieudamuons);

            modelBuilder.Entity<PhieuMuonPhong>()
            .HasOptional(a => a.nguoiduyet)
            .WithMany(b => b.phieudaduyets);

            /*
             * n-n relationship COSO, DAY, TANG, PHONG - PERMISSION
             */
            modelBuilder.Entity<CoSo>().
            HasMany(c => c.permissions).
            WithMany(p => p.cosos).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("COSO_PERMISSION");
                });

            modelBuilder.Entity<Dayy>().
            HasMany(c => c.permissions).
            WithMany(p => p.days).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("DAY_PERMISSION");
                });
            modelBuilder.Entity<Tang>().
            HasMany(c => c.permissions).
            WithMany(p => p.tangs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("TANG_PERMISSION");
                });
            modelBuilder.Entity<Phong>().
            HasMany(c => c.permissions).
            WithMany(p => p.phongs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("PHONG_PERMISSION");
                });
            /*
             * n-n relationship OBJECT~HINHANH
             */
            modelBuilder.Entity<CoSo>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.cosos).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("COSO_HINHANH");
                });

            modelBuilder.Entity<Dayy>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.days).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("DAY_HINHANH");
                });

            modelBuilder.Entity<Tang>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.tangs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("TANG_HINHANH");
                });

            modelBuilder.Entity<ThietBi>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.thietbis).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("THIETBI_HINHANH");
                });

            modelBuilder.Entity<SuCoPhong>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.sucophongs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("SUCOPHONG_HINHANH");
                });

            modelBuilder.Entity<LogSuCoPhong>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.logsucophongs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("LOGSUCOPHONG_HINHANH");
                });

            modelBuilder.Entity<LogThietBi>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.logthietbis).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("LOGTHIETBI_HINHANH");
                });

            modelBuilder.Entity<Phong>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.phongs).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("PHONG_HINHANH");
                });

            modelBuilder.Entity<NhanVienPT>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.nhanvienpts).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("NHANVIENPT_HINHANH");
                });

            modelBuilder.Entity<CTThietBi>().
            HasMany(c => c.hinhanhs).
            WithMany(p => p.ctthietbis).
            Map(
                m =>
                {
                    m.MapLeftKey("id1");
                    m.MapRightKey("id2");
                    m.ToTable("CTTHIETBI_HINHANH");
                });
            #endregion
        }
        public override int SaveChanges()
        {
            //Ban đầu lúc tạo CSDL Server (Seed), sau khi Seed xong thì SaveChanges sẽ được gọi
            //nếu không kiểm tra để bỏ qua thì lớp Entity sẽ được callback,
            //LogHeThong sẽ được tạo ra, sẽ gọi new DB tren Global.working_database...
            //khi đó sẽ tự động tạo CSDL cho CLIENT luôn và log hệ thống sẽ bị ghi ở CSDL khác với Server
            if (create_db_structure == true)
            {
                return base.SaveChanges();
            }

            IEnumerable<DbEntityEntry> changedEntities = ChangeTracker.Entries().Where(c => c.State == EntityState.Added || c.State == EntityState.Modified || c.State == EntityState.Deleted);
            ICollection<_EFEventRegisterInterface> Added_Callbacks = new List<_EFEventRegisterInterface>();
            ICollection<_EFEventRegisterInterface> Modified_Callbacks = new List<_EFEventRegisterInterface>();
            ICollection<_EFEventRegisterInterface> Deleted_Callbacks = new List<_EFEventRegisterInterface>();

            foreach (DbEntityEntry item in changedEntities)
            {
                if (item.Entity is _EFEventRegisterInterface)
                {
                    _EFEventRegisterInterface entity = (_EFEventRegisterInterface)item.Entity;
                    switch (item.State)
                    {
                        case EntityState.Added:
                            //quocdunginfo fail here (Some time HinhAnh was "added state")
                            Added_Callbacks.Add(entity);
                            entity.onBeforeAdded();
                            break;

                        case EntityState.Modified:
                            Modified_Callbacks.Add(entity);
                            entity.onBeforeUpdated();
                            break;

                        case EntityState.Deleted:
                            Deleted_Callbacks.Add(entity);
                            entity.onBeforeDeleted();
                            break;
                    }
                }
            }
            //---SaveChange lần 1
            int result = base.SaveChanges();
            //Raise event
            foreach (_EFEventRegisterInterface item in Added_Callbacks)
            {
                item.onAfterAdded();
            }
            foreach (_EFEventRegisterInterface item in Modified_Callbacks)
            {
                item.onAfterUpdated();
            }

            //Có thể gây lỗi khi thêm 1 object thì lại bị ghi log 2 lần: add > edit
            //---SaveChange lần 2
            //changedEntities = ChangeTracker.Entries().Where(c => c.State == EntityState.Added || c.State == EntityState.Modified || c.State == EntityState.Deleted);
            ////Nếu có ít nhất 1 sự thay đổi trong ChangeTracker thì gọi lại SaveChanges
            //foreach (var item in changedEntities)
            //{
            //    result += this.SaveChanges();
            //    break;
            //}
            result += base.SaveChanges();

            //clear RAM
            Added_Callbacks = null;
            Modified_Callbacks = null;
            changedEntities = null;
            return result;
        }
        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            //if (entityEntry.Entity is CoSo && entityEntry.State == EntityState.Deleted)
            //{

            //    return true;
            //}
            return base.ShouldValidateEntity(entityEntry);
        }
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            ////Khai báo Validation
            //List<DbValidationError> list = new List<DbValidationError>();
            //////Kiểm tra CoSo
            ////if (entityEntry.Entity is CoSo && entityEntry.State == EntityState.Deleted)
            ////{
            ////    CoSo tmp = (CoSo)entityEntry.Entity;
            ////    CoSo tmp2 = this.COSOS.AsNoTracking().Where(c => c.id == tmp.id).FirstOrDefault();
            ////    //check Dãy
            ////    if(tmp2.days.Count>0)
            ////    {
            ////        list.Add(new DbValidationError("error", "Cơ sở có chứa dãy"));
            ////        return new DbEntityValidationResult(entityEntry, list);
            ////    }
            ////    //check Phòng
            ////    if (tmp2.vitris.Where(c=>c.phongs.Count>0).FirstOrDefault()!=null)
            ////    {
            ////        list.Add(new DbValidationError("error", "Cơ sở có chứa phòng"));
            ////        return new DbEntityValidationResult(entityEntry, list);
            ////    }
            ////}
            ////Kiểm tra QuanTriVien
            //if (entityEntry.Entity is QuanTriVien && entityEntry.State == EntityState.Added)
            //{
            //    QuanTriVien tmp = (QuanTriVien)entityEntry.Entity;
            //    if (this.QUANTRIVIENS.Where(c => c.username.ToUpper().Equals(tmp.username.ToUpper())).FirstOrDefault() != null)
            //    {
            //        list.Add(new DbValidationError("username", "Tên đăng nhập đã có"));

            //        return new DbEntityValidationResult(entityEntry, list);
            //    }
            //}
            return base.ValidateEntity(entityEntry, items);
        }
        #endregion

        /// <summary>
        /// reload tất cả entity bị fail do validation ERROR
        /// </summary>
        internal void reloadAllFail()
        {
            try{
                var error_list = this.GetValidationErrors().Select(c => c.Entry);
                foreach (var item in error_list)
                {
                    //reload
                    this.Entry(item.Entity).Reload();
                }
            }catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        /// <summary>
        /// Tạo Group và QuanTriVien mau
        /// </summary>
        internal void forceSeed()
        {
            try
            {
                //DATETIME
                String mota = "Hệ thống tự động tạo";
                //GROUP
                if (this.GROUPS.Where(c => c.ten.ToLower().Equals("root")).Count() <= 0)
                {
                    Group gp = new Group();
                    gp.mota = mota;
                    gp.date_create = gp.date_modified = ServerTimeHelper.getNow();
                    gp.ten = "root";
                    gp.subId = gp.ten;
                    gp.key = gp.ten;

                    if (this.QUANTRIVIENS.Where(c => c.username.ToLower().Equals("root")).Count() <= 0)
                    {
                        //QUANTRIVIEN
                        QuanTriVien qtv = new QuanTriVien();
                        qtv.date_create = qtv.date_modified = ServerTimeHelper.getNow();
                        qtv.username = "root";
                        qtv.setPassword(qtv.username); //hashPassword("root") => "2B1ED923B31D1B0990A28C932565156D11F9F7D9"
                        qtv.hoten = "root";
                        qtv.mota = mota;
                        qtv.subId = qtv.username;
                        qtv.group = gp;
                        this.QUANTRIVIENS.Add(qtv);
                    }
                }

                //final commit
                SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
