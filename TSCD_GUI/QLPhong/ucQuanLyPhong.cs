﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TSCD.DataFilter;
using TSCD.Entities;
using SHARED.Libraries;
using TSCD_GUI.MyUserControl;
using DevExpress.XtraGrid.Views.Grid;

namespace TSCD_GUI.QLPhong
{
    public partial class ucQuanLyPhong : DevExpress.XtraEditors.XtraUserControl
    {
        ucComboBoxViTri _ucComboBoxViTri = new ucComboBoxViTri();
        ucTreeViTri _ucTreeViTri = new ucTreeViTri();
        ViTri _ViTriHienTai = new ViTri();
        Phong objPhong = new Phong();

        List<PhongHienThi> listPhong = new List<PhongHienThi>();

        String function = "";
        public bool working = false;

        public delegate void SelectPageDonViTaiSan(Guid? donvi_id, Guid? phong_id);
        public SelectPageDonViTaiSan selectPageDonViTaiSan = null;

        public ucQuanLyPhong()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            rbnControlPhong.Parent = null;
            _ucTreeViTri.focusedRow_phong = new MyUserControl.ucTreeViTri.FocusedRow_phong(FocusedRowChangedTreePhong);
            _ucComboBoxViTri.Dock = DockStyle.Fill;
            panelControlViTri.Controls.Add(_ucComboBoxViTri);
            _ucTreeViTri.Dock = DockStyle.Fill;
            navBarGroupControlContainerViTri.Controls.Add(_ucTreeViTri);
        }

        public DevExpress.XtraBars.Ribbon.RibbonControl getRibbonControl()
        {
            return rbnControlPhong;
        }

        public void loadData()
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitFormLoad), true, true, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
            try
            {
                editGUI("view");
                List<ViTriHienThi> listVitri = ViTriHienThi.getAll();
                List<ViTriHienThi> listVitri2 = new List<ViTriHienThi>(listVitri);
                _ucTreeViTri.DataSource = listVitri;
                _ucComboBoxViTri.DataSource = listVitri2;
                loadLoaiPhong();
                _ViTriHienTai = _ucTreeViTri.Vitri;
                if (_ViTriHienTai != null)
                    listPhong = PhongHienThi.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
                else
                    listPhong = null;
                gridControlPhong.DataSource = listPhong;
                if (listPhong == null || listPhong.Count() == 0)
                {
                    enableButton(false);
                    gridLookUpLoai.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->loadData: " + ex.Message);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
        }

        public void loadDataOnlyPhong()
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitFormLoad), true, true, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu...");
            try
            {
                editGUI("view");
                loadLoaiPhong();
                _ViTriHienTai = _ucTreeViTri.Vitri;
                if (_ViTriHienTai != null)
                    listPhong = PhongHienThi.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
                else
                    listPhong = null;
                gridControlPhong.DataSource = listPhong;
                if (listPhong == null || listPhong.Count() == 0)
                {
                    enableButton(false);
                    gridLookUpLoai.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->loadData: " + ex.Message);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
        }

        private void loadLoaiPhong()
        {
            gridLookUpLoai.Properties.DataSource = LoaiPhong.getQuery().OrderBy(c => c.order).ToList();
        }

        private void reloadAndFocused(Guid _id)
        {
            loadDataOnlyPhong();
            int rowHandle = gridViewPhong.LocateByValue(colid.FieldName, _id);
            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                gridViewPhong.FocusedRowHandle = rowHandle;
        }

        private void editGUI(String _type)
        {
            function = _type;
            if (_type.Equals("view"))
            {
                SetTextGroupControl("Chi tiết", Color.Empty);
                enableEdit(false);
            }
            else if (_type.Equals("add"))
            {
                SetTextGroupControl("Thêm phòng", Color.Red);
                enableEdit(true);
                clearText();
                txtTen.Focus();
            }
            else if (_type.Equals("edit"))
            {
                SetTextGroupControl("Sửa tình phòng", Color.Red);
                enableEdit(true);
                txtTen.Focus();
            }
        }

        public void FocusedRowChangedTreePhong()
        {
            try
            {
                _ViTriHienTai = _ucTreeViTri.Vitri;
                listPhong = PhongHienThi.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
                gridControlPhong.DataSource = listPhong;
                if (listPhong.Count() == 0)
                {
                    enableButton(false);
                    gridLookUpLoai.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->FocusedRowChangedTreePhong: " + ex.Message);
            }
        }

        private void SetTextGroupControl(String text, Color color)
        {
            groupControlInfo.Text = text;
            groupControlInfo.AppearanceCaption.ForeColor = color;
        }

        private void enableEdit(bool _enable)
        {
            btnOK.Visible = _enable;
            btnHuy.Visible = _enable;
            txtTen.Properties.ReadOnly = !_enable;
            txtSoChoNgoi.Properties.ReadOnly = !_enable;
            txtMoTa.Properties.ReadOnly = !_enable;
            _ucComboBoxViTri.ReadOnly = !_enable;
            gridLookUpLoai.Properties.ReadOnly = !_enable;
            enableButton(!_enable);
            btnThem_r.Enabled = !_enable;
            barBtnThemPhong.Enabled = !_enable;
            working = _enable;
        }

        private void enableButton(bool _enable)
        {
            barBtnSuaPhong.Enabled = _enable;
            barBtnXoaPhong.Enabled = _enable;
            btnSua_r.Enabled = _enable;
            btnXoa_r.Enabled = _enable;
        }

        private void clearText()
        {
            txtTen.Text = "";
            txtMoTa.Text = "";
            txtSoChoNgoi.EditValue = 0;
            gridLookUpLoai.EditValue = null;
        }

        private void setDataView()
        {
            try
            {
                dxErrorProviderInfo.ClearErrors();
                editGUI("view");
                if (gridViewPhong.RowCount > 0)
                {
                    if (gridViewPhong.GetFocusedRowCellValue(colphong) != null)
                    {
                        objPhong = gridViewPhong.GetFocusedRowCellValue(colphong) as Phong;
                        txtTen.Text = objPhong.ten;
                        txtMoTa.Text = objPhong.mota;
                        txtSoChoNgoi.EditValue = objPhong.sochongoi;
                        gridLookUpLoai.EditValue = objPhong.loaiphong_id;
                        _ucComboBoxViTri.ViTri = objPhong.vitri;
                    }
                    else
                    {
                        clearText();
                        objPhong = new Phong();
                    }
                }
                else
                {
                    enableButton(false);
                    clearText();
                    objPhong = new Phong();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->setDataView: " + ex.Message);
            }
        }

        private void setDataObj()
        {
            try
            {
                objPhong.ten = txtTen.Text;
                objPhong.mota = txtMoTa.Text;
                objPhong.vitri = _ucComboBoxViTri.ViTri;
                objPhong.sochongoi = Convert.ToInt32(txtSoChoNgoi.EditValue);
                LoaiPhong obj = gridLookUpLoai.EditValue != null ? LoaiPhong.getById(GUID.From(gridLookUpLoai.EditValue)) : null;
                objPhong.loaiphong = (obj != null && obj.id != Guid.Empty) ? obj : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->setDataObj: " + ex.Message);
            }
        }

        private void deleteObj()
        {
            try
            {
                if (XtraMessageBox.Show("Bạn có chắc là muốn xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (objPhong.delete() > 0 && DBInstance.commit() > 0)
                    {
                        XtraMessageBox.Show("Xóa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadData();
                    }
                    else
                    {
                        XtraMessageBox.Show("Xóa phòng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->deleteObj: " + ex.Message);
            }
        }

        private Boolean checkInput()
        {
            try
            {
                dxErrorProviderInfo.ClearErrors();
                Boolean check = true;
                if (txtTen.Text.Length == 0)
                {
                    dxErrorProviderInfo.SetError(txtTen, "Chưa điền tên phòng");
                    check = false;
                }
                if (gridLookUpLoai.EditValue == null || GUID.From(gridLookUpLoai.EditValue) == Guid.Empty)
                {
                    dxErrorProviderInfo.SetError(gridLookUpLoai, "Chưa chọn loại phòng");
                    check = false;
                }
                return check;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkInput: " + ex.Message);
                return false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    switch (function)
                    {
                        case "add":
                            objPhong = new Phong();
                            setDataObj();
                            if (objPhong.add() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objPhong.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Thêm phòng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                        case "edit":
                            setDataObj();
                            if (objPhong.update() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Sửa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objPhong.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Sửa phòng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->btnOk_Click: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            dxErrorProviderInfo.ClearErrors();
            setDataView();
        }

        private void btnThem_r_Click(object sender, EventArgs e)
        {
            editGUI("add");
        }

        private void btnSua_r_Click(object sender, EventArgs e)
        {
            editGUI("edit");
        }

        private void btnXoa_r_Click(object sender, EventArgs e)
        {
            deleteObj();
        }

        private void gridViewPhong_DataSourceChanged(object sender, EventArgs e)
        {
            setDataView();
        }

        private void gridViewPhong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            setDataView();
        }

        private void barBtnLoaiPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmQuanLyLoaiPhong frm = new frmQuanLyLoaiPhong();
            frm.ShowDialog();
            loadLoaiPhong();
        }

        private void btnLoaiPhong_Click(object sender, EventArgs e)
        {
            frmQuanLyLoaiPhong frm = new frmQuanLyLoaiPhong();
            frm.ShowDialog();
            loadLoaiPhong();
        }

        private void barBtnThemPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("add");
        }

        private void barBtnSuaPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("edit");
        }

        private void barBtnXoaPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj();
        }

        public bool checkworking()
        {
            try
            {
                if (function.Equals("edit"))
                {
                    return
                        objPhong.ten != txtTen.Text ||
                        (objPhong.mota == null && !txtMoTa.Text.Equals("")) ||
                        (objPhong.mota != null && objPhong.mota != txtMoTa.Text) ||
                        objPhong.loaiphong_id != GUID.From(gridLookUpLoai.EditValue);
                }
                else if (function.Equals("add"))
                {
                    return
                        !txtTen.Text.Equals("") ||
                        !txtMoTa.Text.Equals("") ||
                        gridLookUpLoai.EditValue != null;
                }
                else
                    return working;
            }
            catch
            {
                return true;
            }
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            open.Title = "Chọn tập tin để Import";
            if (open.ShowDialog() == DialogResult.OK)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitFormLoad), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (TSCD_GUI.Libraries.ExcelDataBaseHelper.ImportLoaiPhong(open.FileName, "LoaiPhong"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                }
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitFormLoad), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (TSCD_GUI.Libraries.ExcelDataBaseHelper.ImportPhong(open.FileName, "Phong"))
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

        private void barBtnXemTaiSan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (objPhong != null && objPhong.id != Guid.Empty)
                {
                    CTTaiSan obj = CTTaiSan.getQuery().Where(c => c.phong_id == objPhong.id && c.soluong > 0).FirstOrDefault();
                    if (obj == null)
                    {
                        XtraMessageBox.Show(objPhong.ten + " không chứa tài sản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (selectPageDonViTaiSan != null)
                            selectPageDonViTaiSan(obj.donviquanly_id, obj.phong_id);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->barBtnXemTaiSan_ItemClick: " + ex.Message);
            }
        }

        private void barBtnExpandAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewPhong.ExpandAllGroups();
        }

        private void barBtnCollapseAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewPhong.CollapseAllGroups();
        }

        private void gridViewPhong_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.ListSourceRow >= 0 && !String.IsNullOrEmpty(view.FindFilterText))
                {
                    String text = view.GetRowCellValue(e.ListSourceRow, colten).ToString();
                    String find = view.FindFilterText;
                    if (find.Equals(StringHelper.CoDauThanhKhongDau(find)))
                    {
                        if (StringHelper.CoDauThanhKhongDau(text.ToUpper()).Contains(StringHelper.CoDauThanhKhongDau(find.ToUpper())))
                        {
                            e.Visible = true;
                            e.Handled = true;
                        }
                        else
                        {
                            e.Visible = false;
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        if (text.ToUpper().Contains(find.ToUpper()))
                        {
                            e.Visible = true;
                            e.Handled = true;
                        }
                        else
                        {
                            e.Visible = false;
                            e.Handled = true;
                        }
                    }
                }
            }
            catch { };
        }

        private void btnHuy_VisibleChanged(object sender, EventArgs e)
        {
            btnXemTaiSan.Visible = !btnHuy.Visible;
        }

        private void btnXemTaiSan_Click(object sender, EventArgs e)
        {
            try
            {
                if (objPhong != null && objPhong.id != Guid.Empty)
                {
                    CTTaiSan obj = CTTaiSan.getQuery().Where(c => c.phong_id == objPhong.id && c.soluong > 0).FirstOrDefault();
                    if (obj == null)
                    {
                        XtraMessageBox.Show(objPhong.ten + " không chứa tài sản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (selectPageDonViTaiSan != null)
                            selectPageDonViTaiSan(obj.donviquanly_id, obj.phong_id);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->barBtnXemTaiSan_ItemClick: " + ex.Message);
            }
        }
    }
}
