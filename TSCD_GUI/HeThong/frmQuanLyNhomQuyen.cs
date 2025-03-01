﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SHARED.Libraries;
using TSCD.Entities;
using TSCD;

namespace TSCD_GUI.HeThong
{
    public partial class frmQuanLyNhomQuyen : DevExpress.XtraEditors.XtraForm
    {
        public frmQuanLyNhomQuyen()
        {
            InitializeComponent();
        }

        List<Group> listGroup = null;
        List<Permission> listPermission = new List<Permission>();
        Group objGroup = new Group();
        String function = "";
        bool working = false;
        bool canAdd = false;
        bool canEdit = false;
        bool canDelete = false;

        public void loadData()
        {
            checkPermission();
            editGUI("view");
            listGroup = Group.getAll();
            gridControlGroup.DataSource = listGroup;
            if (listGroup.Count == 0)
            {
                btnSua_r.Enabled = false;
                btnXoa_r.Enabled = false;
            }
        }

        private void checkPermission()
        {
            try
            {
                canAdd = Permission.canAdd<Group>();
                canEdit = Permission.canEdit<Group>(null);
                canDelete = Permission.canDelete<Group>(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkPermission: " + ex.Message);
            }
        }

        public void editGUI(String _type)
        {
            function = _type;
            if (_type.Equals("view"))
            {
                SetTextGroupControl("Chi tiết", Color.Empty);
                enableEdit(false);
            }
            else if (_type.Equals("add"))
            {
                SetTextGroupControl("Thêm nhóm quyền", Color.Red);
                enableEdit(true);
                clearText();
                clearPermission();
                txtTen.Focus();
            }
            else if (_type.Equals("edit"))
            {
                SetTextGroupControl("Sửa nhóm quyền", Color.Red);
                enableEdit(true);
                txtTen.Focus();
            }
        }

        private void clearPermission()
        {
            listPermission.Clear();
            gridControlQuyen.DataSource = null;
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
            //btnPhanQuyen.Visible = _enable;
            //Không được phân quyền cho Group của mình
            btnPhanQuyen.Enabled = _enable && (Global.current_quantrivien_login != null && Global.current_quantrivien_login.group.id != objGroup.id);
            txtKey.Properties.ReadOnly = !_enable;
            txtTen.Properties.ReadOnly = !_enable;
            txtMoTa.Properties.ReadOnly = !_enable;
            //btnPhanQuyen.Visible = _enable;
            working = _enable;
            enableButton(!_enable);
        }

        private void enableButton(bool _enable)
        {
            btnThem_r.Enabled = _enable && canAdd;
            btnSua_r.Enabled = _enable && canEdit;
            btnXoa_r.Enabled = _enable && canDelete;
        }

        private void clearText()
        {
            txtKey.Text = "";
            txtTen.Text = "";
            txtMoTa.Text = "";
            gridControlQuyen.DataSource = null;
        }

        private void gridViewGroup_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            setDataView();
        }

        private void setDataView()
        {
            try
            {
                dxErrorProviderInfo.ClearErrors();
                if (!function.Equals("view"))
                    editGUI("view");
                if (gridViewGroup.RowCount > 0)
                {
                    if (gridViewGroup.FocusedRowHandle > -1 && gridViewGroup.GetFocusedRow() != null)
                    {
                        objGroup = gridViewGroup.GetFocusedRow() as Group;
                        txtKey.Text = objGroup.key;
                        txtTen.Text = objGroup.ten;
                        txtMoTa.Text = objGroup.mota;
                        gridControlQuyen.DataSource = objGroup.permissions.ToList();
                        listPermission = objGroup.permissions.ToList();
                    }
                    else
                    {
                        clearText();
                        objGroup = new Group();
                    }
                }
                else
                {
                    //enableButton(false);
                    clearText();
                    objGroup = new Group();
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
                objGroup.key = txtKey.Text;
                objGroup.ten = txtTen.Text;
                objGroup.mota = txtMoTa.Text;
                objGroup.permissions = listPermission;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->setDataObj: " + ex.Message);
            }
        }

        public void deleteObj()
        {
            try
            {
                if (objGroup.quantriviens.Count > 0)
                {
                    XtraMessageBox.Show("Không thể xóa nhóm quyền này!\r\nNguyên do: Có quản trị viên thuộc nhóm quyền này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (XtraMessageBox.Show("Bạn có chắc là muốn xóa nhóm quyền này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (objGroup.delete() > 0 && DBInstance.commit() > 0)
                        {
                            XtraMessageBox.Show("Xóa nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadData();
                        }
                        else
                        {
                            XtraMessageBox.Show("Xóa nhóm quyền không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->deleteObj: " + ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInput())
                {
                    switch (function)
                    {
                        case "add":
                            objGroup = new Group();
                            setDataObj();
                            if (objGroup.add() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Thêm nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objGroup.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Thêm nhóm quyền không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                        case "edit":
                            setDataObj();
                            if (objGroup.update() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Sửa nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objGroup.id;
                                reloadAndFocused(id);
                            }
                            else
                            {
                                XtraMessageBox.Show("Sửa nhóm quyền không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void reloadAndFocused(Guid _id)
        {
            loadData();
            int rowHandle = gridViewGroup.LocateByValue(colid.FieldName, _id);
            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                gridViewGroup.FocusedRowHandle = rowHandle;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            setDataView();
        }

        private Boolean checkInput()
        {
            return true;
        }

        private void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            frmSuaPermission frm = new frmSuaPermission(new List<Permission>(listPermission));
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                listPermission = frm.getResult();
                gridControlQuyen.DataSource = listPermission;
            }
        }

        public bool checkworking()
        {
            try
            {
                if (function.Equals("edit"))
                {
                    return
                        objGroup.key != txtKey.Text ||
                        objGroup.ten != txtTen.Text ||
                        objGroup.mota != txtMoTa.Text;
                }
                else if (function.Equals("add"))
                {
                    return
                        !txtKey.Text.Equals("") ||
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

        private void gridViewGroup_DataSourceChanged(object sender, EventArgs e)
        {
            setDataView();
        }

        private void frmQuanLyNhomQuyen_Load(object sender, EventArgs e)
        {
            loadData();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmQuanLyNhomQuyen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkworking())
            {
                if (XtraMessageBox.Show("Dữ liệu chưa được lưu, bạn có chắc chắn muốn đóng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}