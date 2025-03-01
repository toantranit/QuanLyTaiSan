﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using PTB_GUI.MyUserControl;
using DevExpress.XtraGrid.Columns;
using PTB_GUI.QLPhong;
using PTB_GUI.QLViTri.MyUserControl;
using PTB_GUI.QLNhanVien;
using PTB_GUI.MyUC;
using PTB_GUI.QLLoaiThietBi;
using PTB_GUI.ThongKe;
using PTB.Entities;
using PTB.DataFilter;
using PTB_GUI.HeThong;
using DevExpress.XtraBars.Ribbon;
using PTB_GUI.ThongKe.ChiTiet;
using PTB_GUI.QLThietBi;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraTreeList.Localization;
using DevExpress.XtraEditors.Controls;
using PTB.Libraries;
using PTB_GUI.Settings;
using PTB_GUI.QLTinhTrang;
using PTB_GUI.QLSuCo;
using DevExpress.LookAndFeel;
using PTB_GUI.MyForm;
using PTB_GUI.PhanCongQTV;
using PTB;

namespace PTB_GUI
{
    public partial class frmMain : frmCustom  //RibbonForm
    {
        ucPhanQuyen _ucPhanQuyen = null;
        ucQuanLyPhong _ucQuanLyPhong = null;
        ucQuanLyPhongThietBi _ucQuanLyPhongThietBi = null;
        ucQuanLyViTri _ucQuanLyViTri = null;
        ucQuanLyNhanVien _ucQuanLyNhanVien = null;
        ucQuanLyThietBi _ucQuanLyThietBi = null;
        ucQuanLyLoaiTB _ucQuanLyLoaiTB = null;
        ucTK_SLTB_TheoTinhTrang _ucTK_SLTB_TheoTinhTrang = null;
        ucQuanLyTinhTrang _ucQuanLyTinhTrang = null;
        ucQuanLySuCo _ucQuanLySuCo = null;
        ucPhanCongQTV _ucPhanCongQTV = null;
        ucLogHeThong _ucLogHeThong = null;

        ucCauHinh _ucCauHinh = null;
        ucGiaoDienvaNgonNgu _ucGiaoDienvaNgonNgu = null;
        ucCapNhatPhanMem _ucCapNhatPhanMem = null;
        ucThongTinPhanMem _ucThongTinPhanMem = null;

        Phong objPhong = new Phong();

        bool drawEnd = false;
        bool open = false;
        public frmMain()
        {
            InitializeComponent();
            init();
            DBInstance.onDBConnectionDown += new DBInstance.DBConnectionChanged(this.rotmang);
            DBInstance.onDBConnectionUp += new DBInstance.DBConnectionChanged(this.comang);
            //tự động đồng bộ sau thời gian
            PTB.Libraries.DatabaseHelper.autoSyncInNewThread();
        }

        private void rotmang(EventArgs e)
        {
            showConnection("Mất kết nối CSDL", false);
        }

        private void comang(EventArgs e)
        {
            showConnection("Có kết nối CSDL", true);
        }

        private void init()
        {
            //Hiện tên người dùng
            if (Global.current_quantrivien_login != null)
            {
                if (Global.current_quantrivien_login.hoten != null)
                    barBtnUser.Caption = Global.current_quantrivien_login.hoten;
                else
                    barBtnUser.Caption = "[Unknown]";
            }
                

            //Việt hóa
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new MyGridLocalizer();
            DevExpress.XtraTreeList.Localization.TreeListLocalizer.Active = new MyTreeListLocalizer();
            DevExpress.XtraEditors.Controls.Localizer.Active = new MyLocalizer();
            

            _ucPhanQuyen = new ucPhanQuyen();
            _ucQuanLyPhong = new ucQuanLyPhong();

            _ucQuanLyPhong.loadDataByPhong = new ucQuanLyPhong.LoadDataByPhong(loadDataByPhong);

            _ucQuanLyPhongThietBi = new ucQuanLyPhongThietBi();
            _ucQuanLyViTri = new ucQuanLyViTri();
            _ucQuanLyNhanVien = new ucQuanLyNhanVien();
            _ucQuanLyThietBi = new ucQuanLyThietBi();
            _ucQuanLyLoaiTB = new ucQuanLyLoaiTB();
            _ucTK_SLTB_TheoTinhTrang = new ThongKe.ucTK_SLTB_TheoTinhTrang();
            _ucQuanLyTinhTrang = new ucQuanLyTinhTrang();
            _ucQuanLySuCo = new ucQuanLySuCo();
            _ucPhanCongQTV = new ucPhanCongQTV();
            _ucLogHeThong = new ucLogHeThong();

            //UserLookAndFeel.Default.SetSkinStyle(Properties.Settings.Default.skin);
            //DockStyle
            _ucPhanQuyen.Dock = DockStyle.Fill;
            _ucQuanLyPhong.Dock = DockStyle.Fill;
            _ucQuanLyPhongThietBi.Dock = DockStyle.Fill;
            _ucQuanLyViTri.Dock = DockStyle.Fill;
            _ucQuanLyNhanVien.Dock = DockStyle.Fill;
            _ucQuanLyThietBi.Dock = DockStyle.Fill;
            _ucQuanLyLoaiTB.Dock = DockStyle.Fill;
            _ucTK_SLTB_TheoTinhTrang.Dock = DockStyle.Fill;
            _ucQuanLySuCo.Dock = DockStyle.Fill;
            _ucPhanCongQTV.Dock = DockStyle.Fill;
            _ucQuanLyTinhTrang.Dock = DockStyle.Fill;
            _ucLogHeThong.Dock = DockStyle.Fill;
            //Add RibbonPage
            //addRibbonPage(_ucThongKeChiTiet.getRibbon());
            //addRibbonPage(_ucThongKeTongQuat.getRibbon());
            addRibbonPage(_ucQuanLyViTri.getRibbon());
            addRibbonPage(_ucQuanLyPhong.getRibbon());
            addRibbonPage(_ucQuanLyPhongThietBi.getRibbon());
            addRibbonPage(_ucQuanLyThietBi.getRibbon());
            addRibbonPage(_ucQuanLyLoaiTB.getRibbon());
            addRibbonPage(_ucQuanLyNhanVien.getRibbon());
            addRibbonPage(_ucPhanCongQTV.getRibbon());
            addRibbonPage(_ucQuanLyTinhTrang.getRibbon());
            addRibbonPage(_ucQuanLySuCo.getRibbon());
            addRibbonPage(_ucTK_SLTB_TheoTinhTrang.getRibbon());
            addRibbonPage(_ucPhanQuyen.getRibbon());
            
            addRibbonPage(_ucLogHeThong.getRibbon());
            drawEnd = true;
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName("rbnPageThongKe_Home");
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName("rbnPageViTri_Home");
            backstageViewControl1.SelectedTabIndex = 7;
            ThongTinPhanMem();
        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
        }

        private void addRibbonPage(RibbonControl ribbon)
        {
            for (int i = 0; i < ribbon.Pages.Count; i++)
            {
                ribbonMain.Pages.Add(ribbon.Pages[i]);
            }
        }

        private void navBarControl1_GroupCollapsing(object sender, DevExpress.XtraNavBar.NavBarGroupCancelEventArgs e)
        {
        }

        private void ribbonMain_SelectedPageChanged(object sender, EventArgs e)
        {
            if (drawEnd)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                try
                {
                    DBInstance.reNew();
                    if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageViTri_Home")))
                    {
                        _ucQuanLyViTri.loadData();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyViTri);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageNhanVien_Home")))
                    {
                        _ucQuanLyNhanVien.loadData();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyNhanVien);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageLoaiTB_Home")))
                    {
                        _ucQuanLyLoaiTB.reLoad();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyLoaiTB);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageThietBi_Home")))
                    {
                        //_ucQuanLyThietBi.loadData(true);
                        _ucQuanLyThietBi.loadData(0, true);
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        _ucQuanLyThietBi.getControl().Parent = navBarGroupQLPhong.ControlContainer;
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyThietBi);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhongThietbi_Home")))
                    {
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        _ucQuanLyPhongThietBi.getTreeList().Parent = navBarGroupQLPhong.ControlContainer;
                        if (!open)
                            _ucQuanLyPhongThietBi.loadData();
                        else
                            _ucQuanLyPhongThietBi.loadData(objPhong.reload());
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyPhongThietBi);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhanQuyen_Home")))
                    {
                        //_ucPhanQuyen.loadData();
                        _ucPhanQuyen.reLoad();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        _ucPhanQuyen.getControl().Parent = navBarGroupQLPhong.ControlContainer;
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucPhanQuyen);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhong_Home")))
                    {
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        _ucQuanLyPhong.getTreeList().Parent = navBarGroupQLPhong.ControlContainer;
                        _ucQuanLyPhong.loadData();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyPhong);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageThongKe_Home")))
                    {
                        _ucTK_SLTB_TheoTinhTrang.loadData();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucTK_SLTB_TheoTinhTrang);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageSuCoPhong")))
                    {
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        _ucQuanLySuCo.getTreeList().Parent = navBarGroupQLPhong.ControlContainer;
                        if (!open)
                            _ucQuanLySuCo.loadData();
                        else
                            _ucQuanLySuCo.loadData(objPhong.reload());
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLySuCo);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhanCongQTV_Home")))
                    {
                        _ucPhanCongQTV.loadData();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucPhanCongQTV);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageTinhTrang_Home")))
                    {
                        _ucQuanLyTinhTrang.loadData(true);
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        navBarGroupQLPhong.ControlContainer.Controls.Add(_ucQuanLyTinhTrang.getControl());
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucQuanLyTinhTrang);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageLogHeThong")))
                    {
                        _ucLogHeThong.loadData();
                        navBarGroupQLPhong.ControlContainer.Controls.Clear();
                        panelControl1.Controls.Clear();
                        panelControl1.Controls.Add(_ucLogHeThong);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(this.Name + ": ribbonMain_SelectedPageChanged :" + ex.Message);
                }
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
            }
        }

        private void loadDataByPhong(Phong obj, String type)
        {
            open = true;
            objPhong = obj;
            if (type.Equals("thietbi"))
            {
                ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName("rbnPagePhongThietbi_Home");
            }
            else if (type.Equals("suco"))
            {
                ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName("rbnPageSuCoPhong");
            }
            open = false;
        }

        private void ribbonMain_SelectedPageChanging(object sender, RibbonPageChangingEventArgs e)
        {
            try
            {
                if (drawEnd)
                {
                    bool working = false;
                    if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageViTri_Home")))
                    {
                        //working = _ucQuanLyViTri.working;
                        working = _ucQuanLyViTri.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageNhanVien_Home")))
                    {
                        working = _ucQuanLyNhanVien.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageLoaiTB_Home")))
                    {
                        working = _ucQuanLyLoaiTB.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageThietBi_Home")))
                    {
                        working = _ucQuanLyThietBi.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhongThietbi_Home")))
                    {
                        working = _ucQuanLyPhongThietBi.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhanQuyen_Home")))
                    {
                        working = _ucPhanQuyen.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhong_Home")))
                    {
                        working = _ucQuanLyPhong.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageThongKe_Home")))
                    {

                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageSuCoPhong")))
                    {
                        working = _ucQuanLySuCo.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPagePhanCongQTV_Home")))
                    {
                        working = _ucPhanCongQTV.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName("rbnPageTinhTrang_Home")))
                    {
                        working = _ucQuanLyTinhTrang.checkworking();
                    }
                    if (working)
                    {
                        if (XtraMessageBox.Show("Dữ liệu chưa được lưu, bạn có chắc chắn muốn chuyển sang chức năng khác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": ribbonMain_SelectedPageChanging :" + ex.Message);
            }
            finally
            { }
        }

        private void backstageViewTabItemCauHinh_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if(backstageViewClientControlCauHinh.Visible)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                if (_ucCauHinh == null) _ucCauHinh = new ucCauHinh();
                _ucCauHinh.reLoad();
                _ucCauHinh.Dock = DockStyle.Fill;
                backstageViewClientControlCauHinh.Controls.Add(_ucCauHinh);
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
        }

        private void backstageViewTabItemGiaoDienvaNgonNgu_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlGiaoDienvaNgonNgu.Visible)
            {
                if (_ucGiaoDienvaNgonNgu == null) _ucGiaoDienvaNgonNgu = new ucGiaoDienvaNgonNgu();
                _ucGiaoDienvaNgonNgu.Dock = DockStyle.Fill;
                backstageViewClientControlGiaoDienvaNgonNgu.Controls.Add(_ucGiaoDienvaNgonNgu);
            }
        }

        private void backstageViewTabItemCapNhatPhanMem_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlCapNhatPhanMem.Visible)
            {
                if (_ucCapNhatPhanMem == null) _ucCapNhatPhanMem = new ucCapNhatPhanMem();
                _ucCapNhatPhanMem.Dock = DockStyle.Fill;
                backstageViewClientControlCapNhatPhanMem.Controls.Add(_ucCapNhatPhanMem);
            }
        }

        private void backstageViewTabItemThongTinPhanMem_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlThongTinPhanMem.Visible)
            {
                ThongTinPhanMem();
            }
        }

        private void backstageViewControl1_Hidden(object sender, EventArgs e)
        {
            backstageViewTabItemThongTinPhanMem.Selected = true;
        }

        private void ThongTinPhanMem()
        {
            if (_ucThongTinPhanMem == null) _ucThongTinPhanMem = new ucThongTinPhanMem();
            _ucThongTinPhanMem.Dock = DockStyle.Fill;
            backstageViewClientControlThongTinPhanMem.Controls.Add(_ucThongTinPhanMem);
        }



        private void backstageViewButtonItemLogout_ItemClick(object sender, BackstageViewItemEventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có muốn thoát chương trình ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void backstageViewButtonItemRestart_ItemClick(object sender, BackstageViewItemEventArgs e)
        {
            
        }

        private void backstageViewTabItemRestart_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            Application.Restart();
            Application.ExitThread();
        }

        private void backstageViewTabItemLogout_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có muốn thoát chương trình ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
            else
                backstageViewControl1.Ribbon.HideApplicationButtonContentControl();
        }

        public void showConnection(String text, bool haveConn)
        {
            barStaticConnectionT.Caption = text;
            barStaticConnection.Enabled = haveConn;
        }

        private void backstageViewTabImportAll_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            open.Title = "Chọn tập tin để Import";
            if (open.ShowDialog() == DialogResult.OK)
            {
                //vitri
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportViTri(open.FileName, "ViTri"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //nhanvien
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportNhanVien(open.FileName, "NhanVienPT"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //phong
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportPhong(open.FileName, "Phong"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //loaitb
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportLoaiThietBi(open.FileName, "LoaiThietBi"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //tinhtrang
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportTinhTrang(open.FileName, "TinhTrang"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //tb chung
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportThietBiChung(open.FileName, "ThietBiChung"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                //tb rieng
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportThietBiRieng(open.FileName, "ThietBiRieng"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }

            }
        }

        private void barBtnUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            SuaThongTinCaNhan frm = new SuaThongTinCaNhan();
            frm.ShowDialog();
        }

        private void barBtnSync_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang đồng bộ CSDL...");
            Global.client_database.start_sync();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
        }
    }
}