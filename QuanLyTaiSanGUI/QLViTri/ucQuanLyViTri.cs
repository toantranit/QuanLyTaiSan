﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using QuanLyTaiSanGUI.MyUC;
using QuanLyTaiSan.Entities;
using QuanLyTaiSan.DataFilter;
using QuanLyTaiSan.Libraries;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;

namespace QuanLyTaiSanGUI.QLViTri.MyUserControl
{
    public partial class ucQuanLyViTri : UserControl
    {
        List<ViTriHienThi> listViTriHienThi = new List<ViTriHienThi>();
        ucComboBoxViTri _ucComboBoxViTri = new ucComboBoxViTri(false, false);
        ucComboBoxViTri _ucComboBoxViTriChonDay = new ucComboBoxViTri(true, false);
        List<HinhAnh> listHinh = new List<HinhAnh>();
        CoSo objCoSo = new CoSo();
        Dayy objDay = new Dayy();
        Tang objTang = new Tang();
        String type = "";
        String function = "";
        String node = "";
        public Boolean working = false;

        public ucQuanLyViTri()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //Ẩn ribbon
            ribbonViTri.Parent = null;
            loadData();
        }

        private void loadData()
        {
            try
            {
                listViTriHienThi = new ViTriHienThi().getAll();
                treeListViTri.DataSource = listViTriHienThi;
                listViTriHienThi = new ViTriHienThi().getAllCoSo();
                _ucComboBoxViTri.loadData(listViTriHienThi);
                listViTriHienThi = new ViTriHienThi().getAllHaveDay();
                _ucComboBoxViTriChonDay.loadData(listViTriHienThi);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + ": loadData :" + ex.Message);
            }
            finally
            { }
        }

        private void treeListViTri_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    SetTextGroupControl("Chi tiết", false);
                    if (function.Equals("edit") || function.Equals("add"))
                    {
                        enableEdit(false, "", "");
                        loadData();
                    }
                    if (e.Node.GetValue(2).ToString().Equals(typeof(CoSo).Name))
                    {
                        objCoSo = new CoSo().getById(Convert.ToInt32(e.Node.GetValue(0)));
                        setData(typeof(CoSo).Name);
                    }
                    else if (e.Node.GetValue(2).ToString().Equals(typeof(Dayy).Name))
                    {
                        objDay = new Dayy().getById(Convert.ToInt32(e.Node.GetValue(0)));
                        setData(typeof(Dayy).Name);
                    }
                    else if (e.Node.GetValue(2).ToString().Equals(typeof(Tang).Name))
                    {
                        objTang = new Tang().getById(Convert.ToInt32(e.Node.GetValue(0)));
                        setData(typeof(Tang).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : treeListViTri_FocusedNodeChanged : " + ex.Message);
            }
            finally
            { }
        }

        public void enableEdit(bool _enable, String _type, String _function)
        {
            function = _function;
            btnImage.Visible = _enable;
            btnOK.Visible = _enable;
            btnHuy.Visible = _enable;
            txtTen.Properties.ReadOnly = !_enable;
            txtMoTa.Properties.ReadOnly = !_enable;
            _ucComboBoxViTri.setReadOnly(!_enable);
            _ucComboBoxViTriChonDay.setReadOnly(!_enable);
            type = _type;
            //làm việc
            working = _enable;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                switch (function)
                {
                    case "edit":
                        editObj(type);
                        break;
                    case "add":
                        addObj(type);
                        break;
                }
                enableEdit(false, "", "");
            }
        }

        private void editObj(String _type)
        {
            try
            {
                switch (_type)
                {
                    case "CoSo":
                        objCoSo.ten = txtTen.Text;
                        objCoSo.mota = txtMoTa.Text;
                        objCoSo.hinhanhs = listHinh;
                        if (objCoSo.update() != -1)
                        {
                            XtraMessageBox.Show("Sửa cơ sở thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objCoSo.id, typeof(CoSo).Name);
                        }
                        break;
                    case "Dayy":
                        objDay.ten = txtTen.Text;
                        objDay.mota = txtMoTa.Text;
                        ViTri _vitri = _ucComboBoxViTri.getViTri();
                        objDay.coso = _vitri.coso;
                        objDay.hinhanhs = listHinh;
                        if (objDay.update() != -1)
                        {
                            XtraMessageBox.Show("Sửa dãy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objDay.id, typeof(Dayy).Name);
                        }
                        break;
                    case "Tang":
                        objTang.ten = txtTen.Text;
                        objTang.mota = txtMoTa.Text;
                        ViTri _vitri2 = _ucComboBoxViTriChonDay.getViTri();
                        objTang.day = _vitri2.day;
                        objTang.hinhanhs = listHinh;
                        if (objTang.update() != -1)
                        {
                            XtraMessageBox.Show("Sửa tầng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objTang.id, typeof(Tang).Name);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : editObj : " + ex.Message);
            }
            finally
            { }
        }

        public void beforeAdd(String _type)
        {
            try
            {
                txtTen.Text = "";
                txtMoTa.Text = "";
                imageSlider1.Images.Clear();
                ViTri _vitri;
                type = _type;
                switch (_type)
                {
                    case "CoSo":

                        ThemMoiNode(-1, "");

                        listHinh = null;
                        panelControl1.Controls.Clear();
                        TextEdit txt = new TextEdit();
                        txt.Properties.ReadOnly = true;
                        txt.Text = "Đại học Sài Gòn";
                        txt.Dock = DockStyle.Fill;
                        panelControl1.Controls.Add(txt);
                        break;
                    case "Dayy":
                        listHinh = null;
                        panelControl1.Controls.Clear();
                        _ucComboBoxViTri.Dock = DockStyle.Fill;
                        _vitri = new ViTri();
                        if (node.Equals(typeof(CoSo).Name))
                            _vitri.coso = objCoSo;
                        else if (node.Equals(typeof(Dayy).Name))
                            _vitri.coso = objDay.coso;
                        else
                            _vitri.coso = objTang.day.coso;

                        ThemMoiNode(_vitri.coso.id, typeof(CoSo).Name);

                        _ucComboBoxViTri.setViTri(_vitri);
                        panelControl1.Controls.Add(_ucComboBoxViTri);
                        break;
                    case "Tang":
                        listHinh = null;
                        panelControl1.Controls.Clear();
                        _ucComboBoxViTriChonDay.Dock = DockStyle.Fill;
                        _vitri = new ViTri();
                        if (node.Equals(typeof(Dayy).Name))
                        {
                            _vitri.coso = objDay.coso;
                            _vitri.day = objDay;
                        }
                        else
                        {
                            _vitri.coso = objTang.day.coso;
                            _vitri.day = objTang.day;
                        }

                        ThemMoiNode(_vitri.day.id, typeof(Dayy).Name);

                        _ucComboBoxViTriChonDay.setViTri(_vitri);
                        panelControl1.Controls.Add(_ucComboBoxViTriChonDay);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : beforeAdd : " + ex.Message);
            }
            finally
            { }
        }

        private void addObj(String _type)
        {
            try
            {
                switch (_type)
                {
                    case "CoSo":
                        CoSo objCoSoNew = new CoSo();
                        objCoSoNew.ten = txtTen.Text;
                        objCoSoNew.mota = txtMoTa.Text;
                        objCoSoNew.hinhanhs = listHinh;
                        if (objCoSoNew.add() != -1)
                        {
                            XtraMessageBox.Show("Thêm cơ sở thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objCoSoNew.id, typeof(CoSo).Name);
                        }
                        break;
                    case "Dayy":
                        Dayy objDayNew = new Dayy();
                        objDayNew.ten = txtTen.Text;
                        objDayNew.mota = txtMoTa.Text;
                        objDayNew.hinhanhs = listHinh;
                        ViTri _vitri = _ucComboBoxViTri.getViTri();
                        objDayNew.coso = _vitri.coso;
                        if (objDayNew.add() != -1)
                        {
                            XtraMessageBox.Show("Thêm dãy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objDayNew.id, typeof(Dayy).Name);
                        }
                        break;
                    case "Tang":
                        Tang objTangNew = new Tang();
                        objTangNew.ten = txtTen.Text;
                        objTangNew.mota = txtMoTa.Text;
                        objTangNew.hinhanhs = listHinh;
                        ViTri _vitri2 = _ucComboBoxViTriChonDay.getViTri();
                        objTangNew.day = _vitri2.day;
                        if (objTangNew.add() != -1)
                        {
                            XtraMessageBox.Show("Thêm tầng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reLoadAndSelectNode(objTangNew.id, typeof(Tang).Name);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : addObj : " + ex.Message);
            }
            finally
            { }
        }

        public void deleteObj(String _type)
        {
            try
            {
                switch (_type)
                {
                    case "CoSo":
                        if (XtraMessageBox.Show("Bạn có chắc là muốn xóa cơ sở?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (objCoSo.delete() != -1)
                            {
                                XtraMessageBox.Show("Xóa cơ sở thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reLoad();
                            }
                            else
                            {
                                XtraMessageBox.Show("Có dãy trong cơ sở. Vui lòng xóa dãy trước!");
                            }
                        }
                        break;
                    case "Dayy":
                        if (XtraMessageBox.Show("Bạn có chắc là muốn xóa dãy?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            CoSo obj = objDay.coso;
                            if (objDay.delete() != -1)
                            {
                                XtraMessageBox.Show("Xóa dãy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reLoad();
                            }

                            else
                            {
                                XtraMessageBox.Show("Có tầng/phòng trong dãy. Vui lòng xóa tầng/phòng trước!");
                            }
                        }
                        break;
                    case "Tang":
                        if (XtraMessageBox.Show("Bạn có chắc là muốn xóa tầng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Dayy obj = objTang.day;
                            if (objTang.delete() != -1)
                            {
                                XtraMessageBox.Show("Xóa tầng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reLoad();
                            }
                            else
                            {
                                XtraMessageBox.Show("Có phòng trong tầng. Vui lòng xóa phòng trước!");
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : deleteObj : " + ex.Message);
            }
            finally
            { }
        }

        public void reLoad()
        {
            errorProvider1.Clear();
            treeListViTri.ClearNodes();
            loadData();
            if (!function.Equals(""))
            {
                enableEdit(false, "", "");
                SetTextGroupControl("Chi tiết", false);
                listHinh = null;
                setData(node);
            }
        }

        private void reLoadAndSelectNode(int _id, String _type)
        {
            try
            {
                reLoad();
                FindNode findNode = new FindNode(_id, _type);
                treeListViTri.NodesIterator.DoOperation(findNode);
                treeListViTri.FocusedNode = findNode.Node;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : reLoadAndSelectNode : " + ex.Message);
            }
            finally
            { }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            reLoad();
            //enableEdit(false, "", "");
            //SetTextGroupControl("Chi tiết", false);
            //errorProvider1.Clear();
            //listHinh = null;
            //setData(node);
        }

        public void SetTextGroupControl(String _text, bool _color)
        {
            groupControl1.Text = _text;
            if(_color)
                groupControl1.AppearanceCaption.ForeColor = Color.Red;
            else
                groupControl1.AppearanceCaption.ForeColor = Color.Black;   
        }

        private Boolean CheckInput()
        {
            errorProvider1.Clear();
            Boolean check = true;
            //if (imageSlider1.Images.Count == 0)
            //{
            //    check = false;
            //    errorProvider1.SetError(imageSlider1, "Cần ít nhất 1 hình ảnh");
            //}
            if (txtTen.Text.Length == 0)
            {
                check = false;
                errorProvider1.SetError(txtTen, "Chưa điền tên");
            }
            return check;
        }

        private void reloadImage()
        {
            imageSlider1.Images.Clear();
            foreach (HinhAnh h in listHinh)
            {
                imageSlider1.Images.Add(h.getImage());
            }
            
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            try
            {
                frmHinhAnh frm = null;
                switch (type)
                {
                    case "CoSo":
                        if (function.Equals("edit"))
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh " + objCoSo.ten;
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        else
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh cơ sở mới";
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        break;
                    case "Dayy":
                        if (function.Equals("edit"))
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh " + objDay.ten;
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        else
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh dãy mới";
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        break;
                    case "Tang":
                        if (function.Equals("edit"))
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh " + objTang.ten;
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        else
                        {
                            frm = new frmHinhAnh(listHinh);
                            frm.Text = "Quản lý hình ảnh tầng mới";
                            frm.ShowDialog();
                            listHinh = frm.getlistHinhs();
                        }
                        break;
                }
                reloadImage();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : btnImage_Click : " + ex.Message);
            }
            finally
            { }
        }

        public void setData(String _type)
        {
            try
            {
                ViTri objViTri = null;
                switch (_type)
                {
                    case "CoSo":
                        txtTen.Text = objCoSo.ten;
                        txtMoTa.Text = objCoSo.mota;
                        panelControl1.Controls.Clear();
                        TextEdit txt = new TextEdit();
                        txt.Properties.ReadOnly = true;
                        txt.Text = "Đại học Sài Gòn";
                        txt.Dock = DockStyle.Fill;
                        panelControl1.Controls.Add(txt);
                        node = typeof(CoSo).Name;
                        listHinh = objCoSo.hinhanhs.ToList();
                        reloadImage();
                        enableGroupViTri(typeof(CoSo).Name);
                        break;
                    case "Dayy":
                        panelControl1.Controls.Clear();
                        _ucComboBoxViTri.Dock = DockStyle.Fill;
                        panelControl1.Controls.Add(_ucComboBoxViTri);
                        txtTen.Text = objDay.ten;
                        txtMoTa.Text = objDay.mota;
                        objViTri = new ViTri();
                        objViTri.coso = objDay.coso;
                        _ucComboBoxViTri.setViTri(objViTri);
                        node = typeof(Dayy).Name;
                        listHinh = objDay.hinhanhs.ToList();
                        reloadImage();
                        enableGroupViTri(typeof(Dayy).Name);
                        break;
                    case "Tang":
                        panelControl1.Controls.Clear();
                        _ucComboBoxViTriChonDay.Dock = DockStyle.Fill;
                        panelControl1.Controls.Add(_ucComboBoxViTriChonDay);
                        txtTen.Text = objTang.ten;
                        txtMoTa.Text = objTang.mota;
                        objViTri = new ViTri();
                        objViTri.day = objTang.day;
                        objViTri.coso = objTang.day.coso;
                        _ucComboBoxViTriChonDay.setViTri(objViTri);
                        node = typeof(Tang).Name;
                        listHinh = objTang.hinhanhs.ToList();
                        reloadImage();
                        enableGroupViTri(typeof(Tang).Name);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : setData : " + ex.Message);
            }
            finally
            { }
        }

        public RibbonControl getRibbon()
        {
            return ribbonViTri;
        }

        private void barBtnThemCoSo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            beforeAdd(typeof(CoSo).Name);
            enableEdit(true, typeof(CoSo).Name, "add");
            SetTextGroupControl("Thêm cơ sở", true);
        }

        private void barBtnSuaCoSo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            enableEdit(true, typeof(CoSo).Name, "edit");
            setData(typeof(CoSo).Name);
            SetTextGroupControl("Sửa cơ sở", true);
        }

        private void barBtnXoaCoSo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj(typeof(CoSo).Name);
        }

        private void barBtnThemDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            beforeAdd(typeof(Dayy).Name);
            enableEdit(true, typeof(Dayy).Name, "add");
            SetTextGroupControl("Thêm dãy", true);
        }

        private void barBtnSuaDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            enableEdit(true, typeof(Dayy).Name, "edit");
            setData(typeof(Dayy).Name);
            SetTextGroupControl("Sửa dãy", true);
        }

        private void barBtnXoaDay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj(typeof(Dayy).Name);
        }

        private void barBtnThemTang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            beforeAdd(typeof(Tang).Name);
            enableEdit(true, typeof(Tang).Name, "add");
            
            SetTextGroupControl("Thêm tầng", true);
        }

        private void barBtnSuaTang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            enableEdit(true, typeof(Tang).Name, "edit");
            setData(typeof(Tang).Name);
            SetTextGroupControl("Sửa tầng", true);
        }

        private void barBtnXoaTang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            deleteObj(typeof(Tang).Name);
        }

        public void enableGroupViTri(String _type)
        {
            switch (_type)
            {
                case "CoSo":
                    rbnGroupViTri_Tang.Enabled = false;
                    barBtnSuaDay.Enabled = false;
                    barBtnXoaDay.Enabled = false;
                    barBtnSuaCoSo.Enabled = true;
                    barBtnXoaCoSo.Enabled = true;
                    break;
                case "Dayy":
                    rbnGroupViTri_Tang.Enabled = true;
                    barBtnSuaDay.Enabled = true;
                    barBtnXoaDay.Enabled = true;
                    barBtnSuaTang.Enabled = false;
                    barBtnXoaTang.Enabled = false;
                    barBtnSuaCoSo.Enabled = false;
                    barBtnXoaCoSo.Enabled = false;
                    break;
                case "Tang":
                    rbnGroupViTri_Tang.Enabled = true;
                    barBtnSuaDay.Enabled = false;
                    barBtnXoaDay.Enabled = false;
                    barBtnSuaTang.Enabled = true;
                    barBtnXoaTang.Enabled = true;
                    barBtnSuaCoSo.Enabled = false;
                    barBtnXoaCoSo.Enabled = false;
                    break;
            }
        }

        private void ThemMoiNode(int _id, String _type)
        {
            try
            {
                FindNode findNode = null;
                if (_type.Equals(""))
                {
                    treeListViTri.AppendNode(new object[] { -1, "new", "new" }, null);
                    findNode = new FindNode(-1, "new");
                    treeListViTri.NodesIterator.DoOperation(findNode);
                    treeListViTri.FocusedNode = findNode.Node;
                }
                else
                {
                    findNode = new FindNode(_id, _type);
                    treeListViTri.NodesIterator.DoOperation(findNode);
                    treeListViTri.AppendNode(new object[] { -1, "new", "new" }, findNode.Node);
                    findNode = new FindNode(-1, "new");
                    treeListViTri.NodesIterator.DoOperation(findNode);
                    treeListViTri.FocusedNode = findNode.Node;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : ThemMoiNode : " + ex.Message);
            }
            finally
            { }
        }
    }
}
