﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PTB.Entities;
using DevExpress.XtraEditors;
using PTB.Libraries;
using SHARED.Libraries;
using PTB_GUI.MyUC;

namespace PTB_GUI.QLTinhTrang
{
    public partial class ucQuanLyTinhTrang : DevExpress.XtraEditors.XtraUserControl, _ourUcInterface
    {
        List<TinhTrang> listTinhTrang = new List<TinhTrang>();
        ucQuanLyTinhTrang_Control _ucQuanLyTinhTrang_Control = new ucQuanLyTinhTrang_Control();
        TinhTrang objTinhTrang = new TinhTrang();
        String function = "";
        bool working = false;
        bool canAdd = false;
        bool canEdit = false;
        bool canDelete = false;
        bool isThietBi = true;
        MyLayout layout = new MyLayout();

        public ucQuanLyTinhTrang()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            ribbonTinhTrang.Parent = null;
            layout.save(gridViewTinhTrang);
            _ucQuanLyTinhTrang_Control.Parent = this;
            _ucQuanLyTinhTrang_Control.loadData = new ucQuanLyTinhTrang_Control.LoadData(loadData);

        }

        public PanelControl getControl()
        {
            return _ucQuanLyTinhTrang_Control.getControl();
        }

        public void loadData(bool _isThietBi)
        {
            isThietBi = _isThietBi;
            loadData();
        }

        public void loadData()
        {
            try
            {
                checkPermission();
                layout.load(gridViewTinhTrang);
                editGUI("view");
                if(isThietBi)
                    listTinhTrang = TinhTrang.getAllForTHIETBI();
                else
                    listTinhTrang = TinhTrang.getAllForSUCOPHONG();
                if (listTinhTrang.Count == 0)
                {
                    enableButton(false);
                }
                gridControlTinhTrang.DataSource = listTinhTrang;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->loadData: " + ex.Message);
            }
        }

        private void reloadAndFocused(Guid _id)
        {
            loadData();
            int rowHandle = gridViewTinhTrang.LocateByValue(colid.FieldName, _id);
            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                gridViewTinhTrang.FocusedRowHandle = rowHandle;
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
                SetTextGroupControl("Thêm tình trạng", Color.Red);
                enableEdit(true);
                clearText();
                txtTen.Focus();
            }
            else if (_type.Equals("edit"))
            {
                SetTextGroupControl("Sửa tình trạng", Color.Red);
                enableEdit(true);
                txtTen.Focus();
            }
        }

        private void SetTextGroupControl(String text, Color color)
        {
            groupControl1.Text = text;
            groupControl1.AppearanceCaption.ForeColor = color;
        }

        private void enableEdit(bool _enable)
        {
            btnOk.Visible = _enable;
            btnHuy.Visible = _enable;
            txtTen.Properties.ReadOnly = !_enable;
            txtMoTa.Properties.ReadOnly = !_enable;
            enableButton(!_enable);
            btnR_Them.Enabled = !_enable && canAdd;
            barButtonThemTinhTrang.Enabled = !_enable && canAdd;
            working = _enable;
        }

        private void checkPermission()
        {
            try
            {
                canAdd = Permission.canAdd<TinhTrang>();
                canEdit = Permission.canEdit<TinhTrang>(null);
                canDelete = Permission.canDelete<TinhTrang>(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkPermission: " + ex.Message);
            }
        }

        private void enableButton(bool _enable)
        {
            //btnR_Them.Enabled = _enable;
            barButtonSuaTinhTrang.Enabled = _enable && canEdit;
            barButtonXoaTinhTrang.Enabled = _enable && canDelete;
            barBtnUp.Enabled = _enable && canEdit;
            barBtnDown.Enabled = _enable && canEdit;
            btnR_Sua.Enabled = _enable && canEdit;
            btnR_Xoa.Enabled = _enable && canDelete;
        }

        private void clearText()
        {
            txtTen.Text = "";
            txtMoTa.Text = "";
        }

        private void setDataView()
        {
            try
            {
                dxErrorProvider1.ClearErrors();
                if(!function.Equals("view")) 
                    editGUI("view");
                if (gridViewTinhTrang.RowCount > 0)
                {
                    if (gridViewTinhTrang.FocusedRowHandle > -1 && gridViewTinhTrang.GetFocusedRow() != null)
                    {
                        objTinhTrang = gridViewTinhTrang.GetFocusedRow() as TinhTrang;
                        txtTen.Text = objTinhTrang.value;
                        txtMoTa.Text = objTinhTrang.mota;
                    }
                    else
                    {
                        clearText();
                        objTinhTrang = new TinhTrang();
                    }
                }
                else
                {
                    enableButton(false);
                    clearText();
                    objTinhTrang = new TinhTrang();
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
                objTinhTrang.value = txtTen.Text;
                objTinhTrang.mota = txtMoTa.Text;
                objTinhTrang.type = isThietBi ? TinhTrang.TYPE_THIETBI : TinhTrang.TYPE_SUCOPHONG;
                objTinhTrang.key = StringHelper.CoDauThanhKhongDau(txtTen.Text).Replace(" ", String.Empty).ToUpper();
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
                if (objTinhTrang.ctthietbis.Count > 0 || objTinhTrang.logthietbis.Count > 0)
                {
                    XtraMessageBox.Show("Không thể xóa tình trạng này!\r\nNguyên do: Có các thiết bị thuộc tình trạng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (objTinhTrang.sucophongs.Count > 0 || objTinhTrang.logsucophongs.Count > 0)
                {
                    XtraMessageBox.Show("Không thể xóa tình trạng này!\r\nNguyên do: Có các sự cố phòng thuộc tình trạng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (XtraMessageBox.Show("Bạn có chắc là muốn xóa tình trạng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (objTinhTrang.delete() > 0 && DBInstance.commit() > 0)
                        {
                            XtraMessageBox.Show("Xóa loại tình trạng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadData();
                        }
                        else
                        {
                            XtraMessageBox.Show("Xóa loại tình trạng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
                dxErrorProvider1.ClearErrors();
                Boolean check = true;
                if (function.Equals("add"))
                {
                    if (listTinhTrang.Where(i => i.value == txtTen.Text).FirstOrDefault() != null)
                    {
                        check = false;
                        dxErrorProvider1.SetError(txtTen, "Tên tình trạng này đã tồn tại");
                    }
                }
                else if (function.Equals("edit"))
                {
                    if (listTinhTrang.Where(i => i.value == txtTen.Text && i.id != objTinhTrang.id).FirstOrDefault() != null)
                    {
                        check = false;
                        dxErrorProvider1.SetError(txtTen, "Tên tình trạng này đã tồn tại");
                    }
                }
                if (txtTen.Text.Length == 0)
                {
                    check = false;
                    dxErrorProvider1.SetError(txtTen, "Chưa điền tên tình trạng");
                }
                return check;
            }
            catch(Exception ex)
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
                            objTinhTrang = new TinhTrang();
                            setDataObj();
                            if (objTinhTrang.add() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Thêm tình trạng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objTinhTrang.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Thêm tình trạng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                        case "edit":
                            setDataObj();
                            if (objTinhTrang.update() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Sửa tình trạng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objTinhTrang.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Sửa tình trạng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            dxErrorProvider1.ClearErrors();
            setDataView();
        }

        private void btnR_Them_Click(object sender, EventArgs e)
        {
            editGUI("add");
        }

        private void btnR_Sua_Click(object sender, EventArgs e)
        {
            editGUI("edit");
        }

        private void btnR_Xoa_Click(object sender, EventArgs e)
        {
            deleteObj();
        }

        private void gridViewTinhTrang_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            setDataView();
        }

        private void gridViewTinhTrang_DataSourceChanged(object sender, EventArgs e)
        {
            setDataView();
        }

        private void barButtonThemTinhTrang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("add");
        }

        private void barButtonSuaTinhTrang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("edit");
        }

        private void barButtonXoaTinhTrang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj();
        }

        public DevExpress.XtraBars.Ribbon.RibbonControl getRibbon()
        {
            return ribbonTinhTrang;
        }

        private void barBtnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (objTinhTrang != null && objTinhTrang.id != Guid.Empty)
                {
                    objTinhTrang.moveUp();
                    DBInstance.commit();
                    reloadAndFocused(objTinhTrang.id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->barBtnUp_ItemClick: " + ex.Message);
            }
        }

        private void barBtnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (objTinhTrang != null && objTinhTrang.id != Guid.Empty)
                {
                    objTinhTrang.moveDown();
                    DBInstance.commit();
                    reloadAndFocused(objTinhTrang.id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->barBtnUp_ItemClick: " + ex.Message);
            }
        }

        public bool checkworking()
        {
            try
            {
                if (function.Equals("edit"))
                {
                    return
                        objTinhTrang.value != txtTen.Text ||
                        objTinhTrang.mota != txtMoTa.Text;
                }
                else if (function.Equals("add"))
                {
                    return
                        !txtTen.Text.Equals("") ||
                        !txtMoTa.Text.Equals("");
                }
                else
                {
                    return working;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkworking: " + ex.Message);
                return true;
            }
        }


        public void reLoad()
        {
            throw new NotImplementedException();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            open.Title = "Chọn tập tin để Import";
            if (open.ShowDialog() == DialogResult.OK)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this.ParentForm, typeof(WaitForm1), true, true, false);
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Đang Import...");
                if (PTB_GUI.Libraries.ExcelDataBaseHelper.ImportTinhTrang(open.FileName, "TinhTrang"))
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import thành công!");
                    loadData();
                }
                else
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Import không thành công!");
                    loadData();
                }

            }
        }
    }
}
