﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PTB.Entities;
using SHARED.Libraries;

namespace PTB_GUI.QLSuCo
{
    public partial class ucQuanLySuCo : UserControl, _ourUcInterface
    {
        //QuanLyTaiSanGUI.MyUC.ucTreeViTri _ucTreeViTri = new QuanLyTaiSanGUI.MyUC.ucTreeViTri("QLSuCoPhong");
        PTB_GUI.MyUC.ucTreeViTri _ucTreeViTri = new PTB_GUI.MyUC.ucTreeViTri();
        List<HinhAnh> listHinhs = new List<HinhAnh>();
        List<SuCoPhong> listSuCo = new List<SuCoPhong>();
        SuCoPhong objSuCo = new SuCoPhong();
        Phong objPhong = new Phong();
        String function = "";
        bool working = false;
        bool canAdd = false;
        bool canEdit = false;
        bool canDelete = false;
        PTB_GUI.MyUC.MyLayout layout = new PTB_GUI.MyUC.MyLayout();

        public ucQuanLySuCo()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            ribbonSuCoPhong.Parent = null;
            //gridViewSuCo.Columns[colday.FieldName].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            gridViewSuCo.Columns[colten.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewSuCo.Columns[coltinhtrang.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewSuCo.Columns[colmota.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            
            //gridViewLogSuCo.Columns[collcreate.FieldName].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            gridViewLogSuCo.Columns[colltinhtrang.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewLogSuCo.Columns[collmota.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewLogSuCo.Columns[collqtvien.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

            //_ucTreeViTri.Parent = this;
            _ucTreeViTri.setData_phongid = new PTB_GUI.MyUC.ucTreeViTri.SetData_phongid(loadDataById);
            layout.save(gridViewSuCo);
        }

        public DevExpress.XtraBars.Ribbon.RibbonControl getRibbon()
        {
            return ribbonSuCoPhong;
        }

        public DevExpress.XtraTreeList.TreeList getTreeList()
        {
            return _ucTreeViTri.getTreeList();
        }

        public void loadData(Phong obj = null)
        {
            try
            {
                layout.load(gridViewSuCo);
                List<PTB.DataFilter.ViTriHienThi> listViTri = PTB.DataFilter.ViTriHienThi.getAllHavePhong();
                _ucTreeViTri.loadData(listViTri);
                List<TinhTrang> listTinhTrang = TinhTrang.getAllForSUCOPHONG();
                lookUpEditTinhTrang.Properties.DataSource = listTinhTrang;
                if (obj == null)
                {
                    objPhong = _ucTreeViTri.getPhong();
                }
                else
                {
                    objPhong = obj;
                    _ucTreeViTri.setPhong(objPhong);
                }
                loadDataByPhong();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->loadData: " + ex.Message);
            }
        }

        private void checkPermission()
        {
            try
            {
                canAdd = Permission.canAdd<SuCoPhong>() && Permission.canEdit<Phong>(objPhong);
                canEdit = Permission.canEdit<SuCoPhong>(null) && Permission.canEdit<Phong>(objPhong);
                canDelete = Permission.canDelete<SuCoPhong>(null) && Permission.canEdit<Phong>(objPhong);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkPermission: " + ex.Message);
            }
        }

        public void loadDataById(Guid id)
        {
            objPhong = Phong.getById(id);
            loadDataByPhong();
        }

        public void loadDataByPhong()
        {
            try
            {
                checkPermission();
                if (objPhong != null && objPhong.id != Guid.Empty)
                {
                    listSuCo = objPhong.sucophongs.OrderByDescending(c => c.ngay).ToList();
                    gridControlSuCo.DataSource = listSuCo;
                    barBtnThem.Enabled = true && canAdd;
                    btnR_Them.Enabled = true && canAdd;
                }
                else
                {
                    listSuCo = new List<SuCoPhong>();
                    gridControlSuCo.DataSource = listSuCo;
                    function = "";
                    barBtnThem.Enabled = false;
                    btnR_Them.Enabled = false;
                }
                if (listSuCo.Count == 0)
                {
                    enableButton(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->loadDataByPhong: " + ex.Message);
            }
        }


        private void editGUI(String _type)
        {
            if (_type.Equals("view"))
            {
                SetTextGroupControl("Chi tiết", Color.Empty);
                enableEdit(false);
            }
            else if (_type.Equals("onlyview"))
            {
                SetTextGroupControl("Chi tiết Log", Color.Empty);
                enableButton(false);
            }
            else if (_type.Equals("add"))
            {
                SetTextGroupControl("Thêm sự cố", Color.Red);
                enableEdit(true);
                clearText();
                txtTen.Focus();
                lblPhong.Text = objPhong != null ? objPhong.ten : "";
            }
            else if (_type.Equals("edit"))
            {
                SetTextGroupControl("Sửa sự cố", Color.Red);
                enableEdit(true);
                txtTen.Focus();
            }
            function = _type;
        }

        private void SetTextGroupControl(String text, Color color)
        {
            groupControl1.Text = text;
            groupControl1.AppearanceCaption.ForeColor = color;
        }

        private void enableEdit(bool _enable)
        {
            btnOK.Visible = _enable;
            btnHuy.Visible = _enable;
            btnImage.Visible = _enable;
            txtTen.Properties.ReadOnly = !_enable;
            txtMota.Properties.ReadOnly = !_enable;
            lookUpEditTinhTrang.Properties.ReadOnly = !_enable;
            dateEdit1.Properties.ReadOnly = !_enable;
            spinPhiSuaChua.Properties.ReadOnly = !_enable;
            enableButton(!_enable);
            barBtnThem.Enabled = !_enable && canAdd;
            btnR_Them.Enabled = !_enable && canAdd;
            working = _enable;
        }

        private void enableButton(bool _enable)
        {
            //btnR_Them.Enabled = _enable;
            btnR_Sua.Enabled = _enable && canEdit;
            btnR_Xoa.Enabled = _enable && canDelete;
            barBtnSua.Enabled = _enable && canEdit;
            barBtnXoa.Enabled = _enable && canDelete;
        }

        private void clearText()
        {
            txtTen.Text = "";
            lookUpEditTinhTrang.EditValue = null;
            dateEdit1.EditValue = null;
            lblPhong.Text = "";
            lblNhanVien.Text = "";
            txtMota.Text = "";
            listHinhs = new List<HinhAnh>();
            imageSlider1.Images.Clear();
            dateEdit1.EditValue = DateTime.Now;
        }

        private void setDataView(bool isLog = false, DevExpress.XtraGrid.Views.Grid.GridView view = null)
        {
            try
            {
                //errorProvider1.Clear();
                clearText();
                if (gridViewSuCo.RowCount > 0)
                {
                    if (gridViewSuCo.FocusedRowHandle > -1 && gridViewSuCo.GetFocusedRow() != null)
                    {                        
                        objSuCo = gridViewSuCo.GetFocusedRow() as SuCoPhong;
                        txtTen.Text = objSuCo.ten;
                        lblPhong.Text = objSuCo.phong.ten;
                        dateEdit1.EditValue = objSuCo.ngay;
                        //spinPhiSuaChua.EditValue = objSuCo.phisuachua;
                        if (!isLog)
                        {
                            lookUpEditTinhTrang.EditValue = objSuCo.tinhtrang_id;
                            txtMota.Text = objSuCo.mota;
                            if (objSuCo.hinhanhs.Count > 0)
                            {
                                listHinhs = objSuCo.hinhanhs.ToList();
                                reloadImage();
                            }
                        }
                        else if (isLog && view.FocusedRowHandle > -1 && view.GetFocusedRow() != null)
                        {
                            LogSuCoPhong obj = view.GetFocusedRow() as LogSuCoPhong;
                            //dateEdit1.EditValue = obj.ngay;
                            lblNhanVien.Text = obj.quantrivien != null ? obj.quantrivien.hoten : "";
                            lookUpEditTinhTrang.EditValue = obj.tinhtrang_id;
                            txtMota.Text = obj.mota;
                            dateEdit1.EditValue = obj.date_create;
                            //spinPhiSuaChua.EditValue = obj.phisuachua;
                            if (obj.hinhanhs.Count > 0)
                            {
                                listHinhs = obj.hinhanhs.ToList();
                                reloadImage();
                            }
                        }
                    }
                    else
                    {
                        objSuCo = new SuCoPhong();
                    }
                }
                else
                {
                    enableButton(false);
                    objSuCo = new SuCoPhong();
                }
                if (isLog)
                {
                    //if (!function.Equals("onlyview"))
                    editGUI("onlyview");
                }
                else
                {
                    //if (!function.Equals("view"))
                    editGUI("view");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->setDataView: " + ex.Message);
            }
        }

        private void gridViewSuCo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            setDataView();
        }

        private void barBtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("add");
        }

        private void gridViewSuCo_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            SuCoPhong c = (SuCoPhong)gridViewSuCo.GetRow(e.RowHandle);
            e.IsEmpty = c.logsucophongs.Count == 0;
        }

        private void gridViewSuCo_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gridViewSuCo_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Log";
        }

        private void gridViewSuCo_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            SuCoPhong c = (SuCoPhong)gridViewSuCo.GetRow(e.RowHandle);
            e.ChildList = c.logsucophongs.OrderByDescending(p=>p.date_create).ToList();
        }

        private void gridViewSuCo_DataSourceChanged(object sender, EventArgs e)
        {
            setDataView();
        }

        private void gridViewLogSuCo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            setDataView(true, sender as DevExpress.XtraGrid.Views.Grid.GridView);
        }

        private void gridControlSuCo_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            if (e.View.Equals(gridViewSuCo))
            {
                setDataView();
            }
            else
            {
                setDataView(true, e.View as DevExpress.XtraGrid.Views.Grid.GridView);
            }
        }

        private void reloadImage()
        {
            try
            {
                imageSlider1.Images.Clear();
                if (listHinhs != null)
                {
                    foreach (HinhAnh h in listHinhs)
                    {
                        imageSlider1.Images.Add(h.getImage());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->reloadImage: " + ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckInput())
                {
                    if (function.Equals("add"))
                    {
                        objSuCo = new SuCoPhong();
                        objSuCo.ten = txtTen.Text;
                        objSuCo.phong = objPhong;
                        objSuCo.tinhtrang = lookUpEditTinhTrang.GetSelectedDataRow() as TinhTrang;
                        objSuCo.mota = txtMota.Text;
                        //objSuCo.phisuachua = spinPhiSuaChua != null ? long.Parse(spinPhiSuaChua.EditValue.ToString()) : 0;
                        objSuCo.hinhanhs = listHinhs;
                        objSuCo.ngay = dateEdit1.EditValue != null ? dateEdit1.DateTime : DateTime.Now;
                        if (objSuCo.add() > 0 && DBInstance.commit() > 0)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Thêm sự cố thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Guid id = objSuCo.id;
                            reLoadAndFocused(id);
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Thêm sự cố không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (function.Equals("edit"))
                    {
                        if (checkworking())
                        {
                            objSuCo.ten = txtTen.Text;
                            objSuCo.tinhtrang = lookUpEditTinhTrang.GetSelectedDataRow() as TinhTrang;
                            objSuCo.mota = txtMota.Text;
                            //objSuCo.phisuachua = spinPhiSuaChua != null ? long.Parse(spinPhiSuaChua.EditValue.ToString()) : 0;
                            objSuCo.hinhanhs = listHinhs;
                            objSuCo.ngay = dateEdit1.EditValue != null ? dateEdit1.DateTime : DateTime.Now;
                            if (objSuCo.update() > 0 && DBInstance.commit() > 0)
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("Sửa sự cố thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Guid id = objSuCo.id;
                                reLoadAndFocused(id);
                            }
                            else
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("Sửa sự cố không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Không thể cập nhật nếu không có thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->btnOK_Click: " + ex.Message);
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            try
            {
                frmHinhAnh frm = new frmHinhAnh(listHinhs);
                if (function.Equals("edit"))
                {
                    frm.Text = "Quản lý hình ảnh " + objSuCo.ten;

                }
                else
                {
                    frm.Text = "Quản lý hình ảnh sự cố mới";
                }
                frm.ShowDialog();
                listHinhs = frm.getlistHinhs();
                reloadImage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->btnImage_Click: " + ex.Message);
            }
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

        private void barBtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editGUI("edit");
        }

        private void barBtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj();
        }

        private void deleteObj()
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Bạn có chắc là muốn xóa sự cố?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (objSuCo.delete() > 0 && DBInstance.commit() > 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Xóa sự cố thành công!");
                        loadDataByPhong();
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Xóa sự cố thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->deleteObj: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            dxErrorProvider1.ClearErrors();
            setDataView();
        }

        private Boolean CheckInput()
        {
            dxErrorProvider1.ClearErrors();
            Boolean check = true;
            if (txtTen.Text.Length == 0)
            {
                check = false;
                dxErrorProvider1.SetError(txtTen, "Chưa điền tên");
            }
            //else if(function.Equals("add") && listSuCo.Where(c=>c.ten.ToUpper().Equals(txtTen.Text.ToUpper())).Count()>0)
            //{
            //    check = false;
            //    dxErrorProvider1.SetError(txtTen, "Tên sự cố này đã tồn tại");
            //}
            if (lookUpEditTinhTrang.EditValue == null)
            {
                check = false;
                dxErrorProvider1.SetError(lookUpEditTinhTrang, "Chưa chọn tình trạng");
            }
            return check;
        }

        private void reLoadAndFocused(Guid _id)
        {
            try
            {
                loadDataByPhong();
                int rowHandle = gridViewSuCo.LocateByValue(colid.FieldName, _id);
                if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                {
                    gridViewSuCo.FocusedRowHandle = rowHandle;
                    gridViewSuCo.ExpandMasterRow(rowHandle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->reLoadAndFocused: " + ex.Message);
            }
        }

        private void imageSlider1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (imageSlider1.Images.Count > 0)
            {
                frmShowImage frm = new frmShowImage(listHinhs);
                frm.ShowDialog();
            }
        }

        public bool checkworking()
        {
            try
            {
                if (!function.Equals("edit"))
                {
                    if (function.Equals("add"))
                    {
                        return
                            !txtTen.Text.Equals("") ||
                            lookUpEditTinhTrang.GetSelectedDataRow() != null ||
                            !txtMota.Text.Equals("") ||
                            listHinhs.Count > 0;
                    }
                    else
                        return working;
                }
                else
                {
                    return
                        objSuCo.ten != txtTen.Text ||
                        objSuCo.tinhtrang != lookUpEditTinhTrang.GetSelectedDataRow() as TinhTrang ||
                        !objSuCo.ngay.Equals(dateEdit1.EditValue) ||
                        objSuCo.mota != txtMota.Text ||
                        objSuCo.hinhanhs.Except(listHinhs).Count() > 0 ||
                        listHinhs.Except(objSuCo.hinhanhs).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->checkworking: " + ex.Message);
                return true;
            }
        }

        private void gridViewSuCo_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            setDataView();
        }

        public void reLoad()
        {
            throw new NotImplementedException();
        }
    }
}
