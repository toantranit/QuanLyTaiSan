﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using TSCD_GUI.QLViTri;
using TSCD_GUI.QLPhong;
using TSCD_GUI.QLDonVi;
using TSCD_GUI.QLLoaiTaiSan;
using TSCD_GUI.QLTaiSan;
using TSCD.Entities;
using TSCD_GUI.Settings;
using DevExpress.XtraEditors;
using TSCD_GUI.HeThong;
using TSCD_GUI.Libraries;
using TSCD_GUI.ThongKe;
using TSCD;
using SHARED.Libraries;

namespace TSCD_GUI
{
    public partial class RibbonFormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        ucQuanLyViTri _ucQuanLyViTri = null;
        ucQuanLyPhong _ucQuanLyPhong = null;
        ucQuanLyDonVi _ucQuanLyDonVi = null;
        ucQuanLyLoaiTS _ucQuanLyLoaiTS = null;
        //ucQuanLyTaiSan _ucQuanLyTaiSan = null;
        ucQuanLyDonVi_TaiSan _ucQuanLyDonVi_TaiSan = null;
        ucPhanQuyen _ucPhanQuyen = null;
        ucThongKe _ucThongKe = null;
        ucLogHeThong _ucLogHeThong = null;

        const String rbnPageViTri = "rbnPageViTri";
        const String rbnPagePhong = "rbnPagePhong";
        const String rbnPageDonVi = "rbnPageDonVi";
        const String rbnPageLoaiTS = "rbnPageLoaiTS";
        const String rbnPageTaiSan = "rbnPageTaiSan";
        const String rbnPageDonVi_TaiSan = "rbnPageDonVi_TaiSan";
        const String rbnPagePhanQuyen = "rbnPagePhanQuyen";
        const String rbnPageThongKe = "rbnPageThongKe";
        const String rbnPageLogHeThong = "rbnPageLogHeThong";

        ucCauHinh _ucCauHinh = null;
        ucGiaoDienvaNgonNgu _ucGiaoDienvaNgonNgu = null;
        ucCapNhatPhanMem _ucCapNhatPhanMem = null;
        ucThongTinPhanMem _ucThongTinPhanMem = null;
        ucImport _ucImport = null;

        bool drawEnd = false;

        Guid? donvi_id = null;
        Guid? phong_id = null;

        public RibbonFormMain()
        {
            InitializeComponent();
            init();
            //Tự động đồng bộ sau thời gian
            TSCD.Libraries.DatabaseHelper.autoSyncInNewThread();
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

            _ucQuanLyViTri = new ucQuanLyViTri();
            _ucQuanLyPhong = new ucQuanLyPhong();
            _ucQuanLyPhong.selectPageDonViTaiSan = new ucQuanLyPhong.SelectPageDonViTaiSan(SelectPageDonViTaiSan);
            _ucQuanLyDonVi = new ucQuanLyDonVi();
            _ucQuanLyLoaiTS = new ucQuanLyLoaiTS();
            //_ucQuanLyTaiSan = new ucQuanLyTaiSan();
            _ucQuanLyDonVi_TaiSan = new ucQuanLyDonVi_TaiSan();
            //_ucPhanQuyen = new ucPhanQuyen();
            _ucThongKe = new ucThongKe();
            //_ucLogHeThong = new ucLogHeThong();

            _ucQuanLyViTri.Dock = DockStyle.Fill;
            _ucQuanLyPhong.Dock = DockStyle.Fill;
            _ucQuanLyDonVi.Dock = DockStyle.Fill;
            _ucQuanLyLoaiTS.Dock = DockStyle.Fill;
            //_ucQuanLyTaiSan.Dock = DockStyle.Fill;
            _ucQuanLyDonVi_TaiSan.Dock = DockStyle.Fill;
            //_ucPhanQuyen.Dock = DockStyle.Fill;
            _ucThongKe.Dock = DockStyle.Fill;
            //_ucLogHeThong.Dock = DockStyle.Fill;

            addRibbonPage(_ucQuanLyViTri.getRibbonControl());
            addRibbonPage(_ucQuanLyPhong.getRibbonControl());
            addRibbonPage(_ucQuanLyDonVi.getRibbonControl());
            addRibbonPage(_ucQuanLyLoaiTS.getRibbonControl());
            //addRibbonPage(_ucQuanLyTaiSan.getRibbonControl());
            addRibbonPage(_ucQuanLyDonVi_TaiSan.getRibbonControl());
            //addRibbonPage(_ucPhanQuyen.getRibbonControl());
            addRibbonPage(_ucThongKe.getRibbonControl());
            //addRibbonPage(_ucLogHeThong.getRibbonControl());

            drawEnd = true;
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageLoaiTS);
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageViTri);
            BackstageHideAllOnlyShowThongTinPhanMem();
            ThongTinPhanMem();
        }

        private void addRibbonPage(RibbonControl ribbon)
        {
            for (int i = 0; i < ribbon.Pages.Count; i++)
            {
                ribbonMain.Pages.Add(ribbon.Pages[i]);
            }
        }

        private void ribbonMain_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                if (drawEnd)
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitFormLoad), true, true, false);
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                    DBInstance.reNew();
                    if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageViTri)))
                    {
                        _ucQuanLyViTri.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucQuanLyViTri);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPagePhong)))
                    {
                        _ucQuanLyPhong.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucQuanLyPhong);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageDonVi)))
                    {
                        _ucQuanLyDonVi.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucQuanLyDonVi);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageLoaiTS)))
                    {
                        _ucQuanLyLoaiTS.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucQuanLyLoaiTS);
                    }
                    //else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageTaiSan)))
                    //{
                    //    _ucQuanLyTaiSan.loadData();
                    //    panelControlMain.Controls.Clear();
                    //    panelControlMain.Controls.Add(_ucQuanLyTaiSan);
                    //}
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageDonVi_TaiSan)))
                    {
                        _ucQuanLyDonVi_TaiSan.loadData(DonVi.getById(donvi_id), Phong.getById(phong_id));
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucQuanLyDonVi_TaiSan);
                        donvi_id = null;
                        phong_id = null;
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPagePhanQuyen)))
                    {
                        _ucPhanQuyen.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucPhanQuyen);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageThongKe)))
                    {
                        _ucThongKe.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucThongKe);
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageLogHeThong)))
                    {
                        _ucLogHeThong.loadData();
                        panelControlMain.Controls.Clear();
                        panelControlMain.Controls.Add(_ucLogHeThong);
                    }
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                Debug.WriteLine(this.Name + "->ribbonMain_SelectedPageChanged: " + ex.Message);
            }
        }

        public void SelectPageDonViTaiSan(Guid? donvi_id, Guid? phong_id)
        {
            this.donvi_id = donvi_id;
            this.phong_id = phong_id;
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageDonVi_TaiSan);
        }

        private void backstageViewTabItemCaiDatCauHinh_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlCaiDatCauHinh.Visible)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitFormLoad), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
                if (_ucCauHinh == null) _ucCauHinh = new ucCauHinh();
                _ucCauHinh.reLoad();
                _ucCauHinh.Dock = DockStyle.Fill;
                backstageViewClientControlCaiDatCauHinh.Controls.Add(_ucCauHinh);
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
        }

        private void backstageViewTabItemGiaoDienVaNgonNgu_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlGiaoDienVaNgonNgu.Visible)
            {
                if (_ucGiaoDienvaNgonNgu == null) _ucGiaoDienvaNgonNgu = new ucGiaoDienvaNgonNgu();
                _ucGiaoDienvaNgonNgu.Dock = DockStyle.Fill;
                backstageViewClientControlGiaoDienVaNgonNgu.Controls.Add(_ucGiaoDienvaNgonNgu);
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

        private void ThongTinPhanMem()
        {
            if (_ucThongTinPhanMem == null) _ucThongTinPhanMem = new ucThongTinPhanMem();
            _ucThongTinPhanMem.Dock = DockStyle.Fill;
            backstageViewClientControlThongTinPhanMem.Controls.Add(_ucThongTinPhanMem);
        }

        private void backstageViewTabItemKhoiDongLai_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            Application.Restart();
            Application.ExitThread();
        }

        private void backstageViewTabItemThoat_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có muốn thoát chương trình ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
            else
                backstageViewControl.Ribbon.HideApplicationButtonContentControl();
        }

        private void backstageViewControl_Hidden(object sender, EventArgs e)
        {
            BackstageHideAllOnlyShowThongTinPhanMem();
        }

        public void BackstageHideAllOnlyShowThongTinPhanMem()
        {
            backstageViewTabItem7.Selected = backstageViewTabItemCaiDatCauHinh.Selected = backstageViewTabItemCapNhatPhanMem.Selected = backstageViewTabItemGiaoDienVaNgonNgu.Selected = backstageViewTabItemImport.Selected = backstageViewTabItemKhoiDongLai.Selected = backstageViewTabItemLogHeThong.Selected = backstageViewTabItemPhanQuyen.Selected = backstageViewTabItemThoat.Selected = false;
            backstageViewTabItemThongTinPhanMem.Selected = true;
        }

        private void ribbonMain_SelectedPageChanging(object sender, RibbonPageChangingEventArgs e)
        {
            try
            {
                if (drawEnd)
                {
                    bool working = false;
                    if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageViTri)))
                    {
                        working = _ucQuanLyViTri.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPagePhong)))
                    {
                        working = _ucQuanLyPhong.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageDonVi)))
                    {
                        working = _ucQuanLyDonVi.checkworking();
                    }
                    else if (ribbonMain.SelectedPage.Equals(ribbonMain.Pages.GetPageByName(rbnPageLoaiTS)))
                    {
                        working = _ucQuanLyLoaiTS.checkworking();
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
                System.Console.WriteLine(this.Name + "->ribbonMain_SelectedPageChanging: " + ex.Message);
            }
        }

        private void barBtnUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            SuaThongTinCaNhan frm = new SuaThongTinCaNhan();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void backstageViewTabItemPhanQuyen_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitFormLoad), true, true, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
            DBInstance.reNew();
            if (_ucPhanQuyen != null)
            {
                ribbonMain.Pages.Remove(ribbonMain.Pages.GetPageByName(rbnPagePhanQuyen));
            }

            _ucPhanQuyen = new ucPhanQuyen();
            _ucPhanQuyen.Dock = DockStyle.Fill;
            addRibbonPage(_ucPhanQuyen.getRibbonControl());

            _ucPhanQuyen.loadData();
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(_ucPhanQuyen);

            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPagePhanQuyen);

            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);

            _ucPhanQuyen.barButtonItemDongTab.ItemClick += barButtonPhanQuyenItemDongTab_ItemClick;
        }

        public void barButtonPhanQuyenItemDongTab_ItemClick(object sender, ItemClickEventArgs e)
        {
            removeRibbonPhanQuyen();
        }

        private void removeRibbonPhanQuyen()
        {
            ribbonMain.Pages.Remove(ribbonMain.Pages.GetPageByName(rbnPagePhanQuyen));
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageViTri);
        }

        private void backstageViewTabItemLogHeThong_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitFormLoad), true, true, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
            DBInstance.reNew();
            if (_ucLogHeThong != null)
            {
                ribbonMain.Pages.Remove(ribbonMain.Pages.GetPageByName(rbnPageLogHeThong));
            }

            _ucLogHeThong = new ucLogHeThong();
            _ucLogHeThong.Dock = DockStyle.Fill;
            addRibbonPage(_ucLogHeThong.getRibbonControl());

            _ucLogHeThong.loadData();
            panelControlMain.Controls.Clear();
            panelControlMain.Controls.Add(_ucLogHeThong);

            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageLogHeThong);

            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);

            _ucLogHeThong.barButtonItemDongTab.ItemClick += barButtonLogHeThongItemDongTab_ItemClick;
        }
        public void barButtonLogHeThongItemDongTab_ItemClick(object sender, ItemClickEventArgs e)
        {
            removeRibbonLogHeThong();
        }

        private void removeRibbonLogHeThong()
        {
            ribbonMain.Pages.Remove(ribbonMain.Pages.GetPageByName(rbnPageLogHeThong));
            ribbonMain.SelectedPage = ribbonMain.Pages.GetPageByName(rbnPageViTri);
        }

        private void backstageViewTabItemImport_SelectedChanged(object sender, BackstageViewItemEventArgs e)
        {
            if (backstageViewClientControlImport.Visible)
            {
                if (_ucImport == null) _ucImport = new ucImport();
                _ucImport.Dock = DockStyle.Fill;
                backstageViewClientControlImport.Controls.Add(_ucImport);
            }
        }
    }
}