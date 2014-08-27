﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyTaiSan.Entities;
using QuanLyTaiSan.DataFilter;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraBars.Ribbon;
using QuanLyTaiSanGUI.MyUC;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Localization;
using QuanLyTaiSan.Libraries;

namespace QuanLyTaiSanGUI.MyUserControl
{
    public partial class ucQuanLyPhong : UserControl, _ourUcInterface
    {
        ViTri _ViTriHienTai = new ViTri();
        Phong objPhong = new Phong();
        NhanVienPT objNhanVienPT = new NhanVienPT();

        List<ViTriHienThi> listVitris = new List<ViTriHienThi>();
        List<Phong> listPhong = new List<Phong>();
        List<NhanVienPT> listNhanVienPT = new List<NhanVienPT>();
        List<HinhAnh> listHinhAnhPhong = new List<HinhAnh>();
        List<HinhAnh> listHinhAnhNhanVien = new List<HinhAnh>();

        ucTreeViTri _ucTreeViTri = new ucTreeViTri();
        ucComboBoxViTri _ucComboBoxViTri = new ucComboBoxViTri(false, false);
        String function = "";
        public bool working = false;
        MyLayout layout = new MyLayout();

        public delegate void LoadDataByPhong(Phong obj, String str);
        public LoadDataByPhong loadDataByPhong = null;

        public ucQuanLyPhong()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //_ucTreeViTri.Parent = this;
            _ucTreeViTri.focusedRow_phong = new ucTreeViTri.FocusedRow_phong(FocusedRowChangedTreePhong);
            ribbonPhong.Parent = null;
            _ucComboBoxViTri.Dock = DockStyle.Fill;
            panelControl1.Controls.Add(_ucComboBoxViTri);
            gridViewPhong.Columns[colten.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewPhong.Columns[colmota.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            gridViewPhong.Columns[colnhanvienpt.FieldName].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

            //save layout
            layout.save(gridViewPhong);
        }

        // Load dữ liệu
        public void loadData()
        {
            //load layout
            layout.load(gridViewPhong);

            listVitris = ViTriHienThi.getAll().ToList();
            _ucTreeViTri.loadData(listVitris);
            _ucComboBoxViTri.loadData(listVitris);
            _ViTriHienTai = _ucTreeViTri.getVitri();
            listPhong = Phong.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
            gridControlPhong.DataSource = listPhong;
            if (listPhong.Count() == 0)
            {
                deleteData();
                enableEdit(false);
                enableBar(false);
            }
            else
            {
                getThongTinPhong(true);
                enableEdit(false);
                enableBar(true);
            }

            listNhanVienPT = NhanVienPT.getAll();
            NhanVienPT NhanVienPTNULL = new NhanVienPT();
            NhanVienPTNULL.hoten = "[Không có]";
            NhanVienPTNULL.id = Guid.Empty;
            listNhanVienPT.Insert(0, NhanVienPTNULL);
            searchLookUpEditNhanVienPT.Properties.DataSource = listNhanVienPT;
        }

        //Mở tắt bar
        public void enableBar(bool _enable)
        {
            barButtonSuaPhong.Enabled = _enable;
            barButtonXoaPhong.Enabled = _enable;
            btnR_Sua.Enabled = _enable;
            btnR_Xoa.Enabled = _enable;
            rbnGroupThietBi.Enabled = _enable;
            rbnGroupSuCo.Enabled = _enable;
        }

        //Xóa hết dữ liệu form thông tin phòng + nhân viên
        private void deleteData()
        {
            setTextGroupControl("Chi tiết phòng", Color.Empty);
            function = "";
            errorProvider1.Clear();
            imgPhong.Images.Clear();
            listHinhAnhPhong = new List<HinhAnh>();
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtMoTaPhong.Text = "";
            if (listVitris.Count > 0)
                _ucComboBoxViTri.setViTri(_ViTriHienTai);
            searchLookUpEditNhanVienPT.EditValue = -1;

            objNhanVienPT = null;
            imgNhanVien.Images.Clear();
            listHinhAnhNhanVien = null;
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            txtSoDienThoai.Text = "";
        }

        //Mở/tắt chỉnh sửa form
        public void enableEdit(bool _enable)
        {
            btnImage.Visible = _enable;
            btnOK.Visible = _enable;
            btnHuy.Visible = _enable;
            txtMaPhong.Properties.ReadOnly = !_enable;
            txtTenPhong.Properties.ReadOnly = !_enable;
            txtMoTaPhong.Properties.ReadOnly = !_enable;
            _ucComboBoxViTri.setReadOnly(!_enable);
            lblNhanVienPT.Visible = _enable;
            searchLookUpEditNhanVienPT.Visible = _enable;
            working = _enable;
            //
            //rbnGroupPhong.Enabled = !_enable;
            //rbnGroupThietBi.Enabled = !_enable;
            //btnR_Them.Enabled = !_enable;
            //btnR_Sua.Enabled = !_enable;
            //btnR_Xoa.Enabled = !_enable;
            if (_enable)
                txtTenPhong.Focus();
        }

        // Reload dữ liệu
        public void reLoad()
        {
            try
            {
                _ucComboBoxViTri = new ucComboBoxViTri(false, false);
                _ucComboBoxViTri.loadData(listVitris);
                _ucComboBoxViTri.Dock = DockStyle.Fill;
                panelControl1.Controls.Clear();
                panelControl1.Controls.Add(_ucComboBoxViTri);
                _ucTreeViTri.setVitri(_ViTriHienTai);
                listPhong = Phong.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
                //listPhong = Phong.getPhongByViTri(cosoid, dayid, tangid);
                gridControlPhong.DataSource = listPhong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        //Khi thêm mới cơ sở -> phòng thì load treelist bên trái + reload dữ liệu ucQuanLyPhong
        public void reLoadAll()
        {
            try
            {
                listVitris = ViTriHienThi.getAll().ToList();
                _ucTreeViTri.loadData(listVitris);
                reLoad();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        //FocusedRowChanged in TreePhong
        public void FocusedRowChangedTreePhong()
        {
            try
            {
                _ViTriHienTai = _ucTreeViTri.getVitri();
                //cosoid = _cosoid;
                //dayid = _dayid;
                //tangid = _tangid;
                //listPhong = Phong.getPhongByViTri(_cosoid, _dayid, _tangid);
                listPhong = Phong.getPhongByViTri(_ViTriHienTai.coso != null ? _ViTriHienTai.coso.id : Guid.Empty, _ViTriHienTai.day != null ? _ViTriHienTai.day.id : Guid.Empty, _ViTriHienTai.tang != null ? _ViTriHienTai.tang.id : Guid.Empty);
                gridControlPhong.DataSource = listPhong;
                switch (listPhong.Count)
                {
                    case 0:
                        deleteData();
                        enableEdit(false);
                        enableBar(false);
                        break;
                    default:
                        getThongTinPhong(true);
                        enableEdit(false);
                        enableBar(true);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : FocusedRowChangedTreePhong : " + ex.Message);
            }
        }

        private void setData()
        {
            try
            {
                setTextGroupControl("Chi thiết phòng", Color.Empty);
                listHinhAnhPhong = new List<HinhAnh>();
                if (objPhong.hinhanhs != null)
                {
                    if (objPhong.hinhanhs.Count > 0)
                    {
                        listHinhAnhPhong = objPhong.hinhanhs.ToList();
                    }
                }
                reloadImagePhong();

                txtMaPhong.Text = objPhong.subId;
                txtTenPhong.Text = objPhong.ten;
                if (objPhong.vitri != null)
                    _ucComboBoxViTri.setViTri(objPhong.vitri);
                txtMoTaPhong.Text = objPhong.mota;

                listHinhAnhNhanVien = new List<HinhAnh>();
                if (objPhong.nhanvienpt != null)
                {
                    searchLookUpEditNhanVienPT.EditValue = objPhong.nhanvienpt.id;
                    if (objPhong.nhanvienpt.hinhanhs != null)
                    {
                        if (objPhong.nhanvienpt.hinhanhs.Count > 0)
                        {
                            listHinhAnhNhanVien = objPhong.nhanvienpt.hinhanhs.ToList();
                        }
                    }

                    txtMaNhanVien.Text = objPhong.nhanvienpt.subId;
                    txtTenNhanVien.Text = objPhong.nhanvienpt.hoten;
                    txtSoDienThoai.Text = objPhong.nhanvienpt.sodienthoai;
                }
                else
                {
                    searchLookUpEditNhanVienPT.EditValue = -1;
                    imgNhanVien.Images.Clear();
                    txtMaNhanVien.Text = "";
                    txtTenNhanVien.Text = "";
                    txtSoDienThoai.Text = "";
                }
                reloadImageNhanVienPT();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : setData : " + ex.Message);
            }
        }

        private void setDataObj()
        {
            try
            {
                objPhong.hinhanhs = listHinhAnhPhong;
                objPhong.subId = txtMaPhong.Text;
                objPhong.ten = txtTenPhong.Text;
                objPhong.vitri = _ucComboBoxViTri.getViTri();
                objPhong.mota = txtMoTaPhong.Text;
                if (objNhanVienPT != null)
                {
                    if (objNhanVienPT.id != Guid.Empty)
                    {
                        objPhong.nhanvienpt = objNhanVienPT;
                    }
                }
                else
                    objPhong.nhanvienpt = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : setDataObj : " + ex.Message);
            }
        }

        private void getThongTinPhong(Boolean first)
        {
            if (listPhong.Count() > 0)
            {
                int row = 0;
                if (!first)
                    row = gridViewPhong.FocusedRowHandle;
                if (row >= 0 && row < listPhong.Count())
                {
                    //int id = (gridViewPhong.GetRow(row) as Phong).id;
                    //objPhong = Phong.getById(id);
                    objPhong = gridViewPhong.GetRow(row) as Phong;
                    if (objPhong != null)
                    {
                        if (objPhong.nhanvienpt != null)
                        {
                            objNhanVienPT = objPhong.nhanvienpt;
                            //objNhanVienPT = NhanVienPT.getById(objPhong.nhanvienpt.id);
                        }
                        setData();
                        enableBar(true);
                    }
                }
                else
                {
                    deleteData();
                    enableBar(false);
                }
            }
            else
            {
                deleteData();
                enableBar(false);
            }
            rbnGroupPhong.Enabled = true;
            btnR_Them.Enabled = true;
            enableEdit(false);
            function = "";
        }

        private void CRUD()
        {
            try
            {
                switch (function)
                {
                    case "add":
                        objPhong = new Phong();
                        setDataObj();
                        if (objPhong.add() > 0 && DBInstance.commit() > 0)
                        {
                            XtraMessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndFocused(objPhong.id);
                            function = "";
                        }
                        else XtraMessageBox.Show("Có lỗi trong khi thêm");
                        break;
                    case "edit":
                        if (objPhong != null)
                        {
                            setDataObj();
                            if (objPhong.update() > 0 && DBInstance.commit() > 0)
                            {
                                XtraMessageBox.Show("Sửa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reLoadAndFocused(objPhong.id);
                                enableEdit(false);
                                function = "";
                            }
                            else XtraMessageBox.Show("Có lỗi trong khi sửa");
                        }
                        break;
                    case "delete":
                        if (objPhong != null)
                        {
                            if(XtraMessageBox.Show("Bạn có chắc là muốn xóa phòng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                int reee = objPhong.delete();
                                if (reee > 0 && DBInstance.commit() > 0)
                                {
                                    XtraMessageBox.Show("Xóa phòng thành công!");
                                    reLoad();
                                }
                                else if(reee == -2)
                                {
                                    XtraMessageBox.Show("Có thiết bị trong phòng. Vui lòng xóa thiết bị trước!");
                                }
                                else if (reee == -3)
                                {
                                    XtraMessageBox.Show("Xóa sự cố trong phòng trước!");
                                }
                                else
                                {
                                    XtraMessageBox.Show("Lỗi trong khi xóa phòng!");
                                }
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->CRUD: " + ex.Message);
            }
        }

        private void reLoadAndFocused(Guid _id)
        {
            try
            {
                reLoad();
                int rowHandle = gridViewPhong.LocateByValue(colid.FieldName, _id);
                if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                    gridViewPhong.FocusedRowHandle = rowHandle;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + "->reLoadAndFocused: " + ex.Message);
            }
        }

        //set màu tiêu đề group
        public void setTextGroupControl(String _text, Color _color)
        {
            groupControl1.Text = _text;
            groupControl1.AppearanceCaption.ForeColor = _color;
        }

        // kiểm tra dữ liệu trước khi lưu
        private Boolean CheckInput()
        {
            errorProvider1.Clear();
            Boolean check = true;
            if (txtTenPhong.Text.Length == 0)
            {
                check = false;
                errorProvider1.SetError(txtTenPhong, "Chưa điền tên");
            }
            //Trường hợp chưa có vị trí
            if (listVitris.Count() == 0)
            {
                check = false;
                errorProvider1.SetError(panelControl1, "Chưa có vị trí");
            }
            return check;
        }

        //load lại ảnh phòng
        private void reloadImagePhong()
        {
            try
            {
                imgPhong.Images.Clear();
                foreach (HinhAnh h in listHinhAnhPhong)
                {
                    imgPhong.Images.Add(h.getImage());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : reloadImagePhong : " + ex.Message);
            }
        }

        //load lại ảnh nhân viên
        private void reloadImageNhanVienPT()
        {
            try
            {
                imgNhanVien.Images.Clear();
                foreach (HinhAnh h in listHinhAnhNhanVien)
                {
                    imgNhanVien.Images.Add(h.getImage());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : reloadImageNhanVienPT : " + ex.Message);
            }
        }

        private void barButtonThemPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                deleteData();
                _ucComboBoxViTri.setViTri(_ViTriHienTai);
                txtMaPhong.Focus();
                enableEdit(true);
                enableAllBarButton(false);
                function = "add";
                setTextGroupControl("Thêm phòng mới", Color.Red);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        private void barButtonSuaPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                setData();
                txtMaPhong.Focus();
                enableEdit(true);
                enableAllBarButton(false);
                function = "edit";
                setTextGroupControl("Chỉnh sửa phòng", Color.Red);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        private void barButtonXoaPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                function = "delete";
                CRUD();
                gridViewPhong.Focus();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
                enableBar(false);
            }
        }

        public RibbonControl getRibbon()
        {
            return ribbonPhong;
        }

        public TreeList getTreeList()
        {
            return _ucTreeViTri.getTreeList();
        }

        private void gridViewPhong_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            getThongTinPhong(false);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            deleteData();
            gridViewPhong.Focus();
            enableEdit(false);
            enableAllBarButton(true);
            if (listPhong.Count() > 0)
            {
                setData();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                CRUD();
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            try
            {
                frmHinhAnh frm = null;

                if (function.Equals("edit"))
                {
                    frm = new frmHinhAnh(listHinhAnhPhong);
                    frm.Text = "Quản lý hình ảnh " + objPhong.ten;
                    frm.ShowDialog();
                    listHinhAnhPhong = frm.getlistHinhs();
                }
                else
                {
                    frm = new frmHinhAnh(listHinhAnhPhong);
                    frm.Text = "Quản lý hình ảnh phòng mới";
                    frm.ShowDialog();
                    listHinhAnhPhong = frm.getlistHinhs();
                }
                reloadImagePhong();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        private void searchLookUpEditNhanVienPT_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (searchLookUpEditNhanVienPT.EditValue != null)
                {
                    Guid id = QuanLyTaiSan.Libraries.GUID.From(searchLookUpEditNhanVienPT.EditValue.ToString());
                    if (id != Guid.Empty)
                        objNhanVienPT = NhanVienPT.getById(id);
                    else
                        objNhanVienPT = null;
                    if (objNhanVienPT != null)
                    {
                        txtMaNhanVien.Text = objNhanVienPT.subId;
                        txtTenNhanVien.Text = objNhanVienPT.hoten;
                        txtSoDienThoai.Text = objNhanVienPT.sodienthoai;
                        listHinhAnhNhanVien = objNhanVienPT.hinhanhs.ToList();
                        reloadImageNhanVienPT();
                    }
                    else
                    {
                        imgNhanVien.Images.Clear();
                        txtMaNhanVien.Text = "";
                        txtTenNhanVien.Text = "";
                        txtSoDienThoai.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        //Thêm xóa sửa bằng phím tắt
        private void gridViewPhong_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.T:
                    this.barButtonThemPhong.PerformClick();
                    break;
                case Keys.S:
                    this.barButtonSuaPhong.PerformClick();
                    break;
                case Keys.X:
                case Keys.Delete:
                    this.barButtonXoaPhong.PerformClick();
                    break;
            }
        }

        private void barButtonXemListTB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (loadDataByPhong != null && objPhong != null && objPhong.id != Guid.Empty)
            {
                loadDataByPhong(objPhong, "thietbi");
            }
        }

        private void btnR_Them_Click(object sender, EventArgs e)
        {
            try
            {
                deleteData();
                _ucComboBoxViTri.setViTri(_ViTriHienTai);
                txtMaPhong.Focus();
                enableEdit(true);
                enableAllBarButton(false);
                function = "add";
                setTextGroupControl("Thêm phòng mới", Color.Red);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        private void btnR_Sua_Click(object sender, EventArgs e)
        {
            try
            {
                setData();
                txtMaPhong.Focus();
                enableEdit(true);
                enableAllBarButton(false);
                function = "edit";
                setTextGroupControl("Chỉnh sửa phòng", Color.Red);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
            }
        }

        private void btnR_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                function = "delete";
                CRUD();
                gridViewPhong.Focus();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": " + ex.Message);
                enableBar(false);
            }
        }

        private void enableAllBarButton(Boolean _enable)
        {
            if (_enable)
            {
                if (listPhong.Count > 0)
                {
                    rbnGroupPhong.Enabled = _enable;
                    rbnGroupThietBi.Enabled = _enable;
                    btnR_Them.Enabled = _enable;
                    btnR_Sua.Enabled = _enable;
                    btnR_Xoa.Enabled = _enable;
                }
                else
                {
                    rbnGroupPhong.Enabled = _enable;
                    btnR_Them.Enabled = _enable;
                }
            }
            else
            {
                rbnGroupPhong.Enabled = _enable;
                rbnGroupThietBi.Enabled = _enable;
                btnR_Them.Enabled = _enable;
                btnR_Sua.Enabled = _enable;
                btnR_Xoa.Enabled = _enable;
            }
        }

        public bool checkworking()
        {
            try
            {
                if (!function.Equals("edit"))
                    return working;
                else
                {
                    //chưa kiểm tra vị trí
                    if (!searchLookUpEditNhanVienPT.EditValue.Equals(-1) && searchLookUpEditNhanVienPT.EditValue != null )
                        return
                            objPhong.hinhanhs.Except(listHinhAnhPhong).Count() > 0 ||
                            listHinhAnhPhong.Except(objPhong.hinhanhs).Count() > 0 || 
                            objPhong.subId != txtMaPhong.Text || 
                            objPhong.ten != txtTenPhong.Text || 
                            objPhong.mota != txtMoTaPhong.Text ||
                            objPhong.nhanvienpt_id != GUID.From(searchLookUpEditNhanVienPT.EditValue);
                    else
                        return 
                            objPhong.hinhanhs.Except(listHinhAnhPhong).Count() > 0 ||
                            listHinhAnhPhong.Except(objPhong.hinhanhs).Count() > 0 || 
                            objPhong.subId != txtMaPhong.Text || 
                            objPhong.ten != txtTenPhong.Text || 
                            objPhong.mota != txtMoTaPhong.Text || 
                            objPhong.nhanvienpt_id != null;
                }
            }
            catch
            {
                return true;
            }
        }

        private void imgPhong_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (imgPhong.Images.Count > 0)
            {
                frmShowImage frm = new frmShowImage(listHinhAnhPhong);
                frm.ShowDialog();
            }
        }

        private void imgNhanVien_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (imgNhanVien.Images.Count > 0)
            {
                frmShowImage frm = new frmShowImage(listHinhAnhNhanVien);
                frm.ShowDialog();
            }
        }

        private void gridViewPhong_DoubleClick(object sender, EventArgs e)
        {
            if (loadDataByPhong != null && objPhong != null && objPhong.id != Guid.Empty)
            {
                loadDataByPhong(objPhong, "thietbi");
            }
        }

        private void barBtnSuCo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (loadDataByPhong != null && objPhong != null && objPhong.id != Guid.Empty)
            {
                loadDataByPhong(objPhong, "suco");
            }
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
                if (QuanLyTaiSanGUI.Libraries.ExcelDataBaseHelper.ImportPhong(open.FileName, "Phong"))
                //if (QuanLyTaiSanGUI.Libraries.ExcelDataBaseHelper.ImportThietBiChung(open.FileName, "ThietBiChung"))
                //if (QuanLyTaiSanGUI.Libraries.ExcelDataBaseHelper.ImportThietBiRieng(open.FileName, "ThietBiRieng"))
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