﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TSCD.Entities;
using SHARED.Libraries;

namespace TSCD_GUI.QLTaiSan
{
    public partial class frmChuyenTinhTrang : DevExpress.XtraEditors.XtraForm
    {
        CTTaiSan objCTTaiSan = null;
        //ChungTu objChungTu = null;
        public delegate void ReloadAndFocused(Guid id);
        public ReloadAndFocused reloadAndFocused = null;

        public frmChuyenTinhTrang()
        {
            InitializeComponent();
        }

        public frmChuyenTinhTrang(CTTaiSan obj)
        {
            InitializeComponent();
            objCTTaiSan = obj;
            loadData();
            setData();
        }

        private void loadData()
        {
            lookUpTinhTrang.Properties.DataSource = TinhTrang.getQuery().OrderBy(c => c.order).ToList();
        }

        private void setData()
        {
            txtMaTS.Text = objCTTaiSan.subId;
            txtTenTS.Text = objCTTaiSan.taisan.ten;
            //objChungTu = objCTTaiSan.chungtu;
            //dateNgayGhi.EditValue = objCTTaiSan.ngay;
            //dateNgay_CT.EditValue = objCTTaiSan.chungtu.ngay;
            lookUpTinhTrang.EditValue = objCTTaiSan.tinhtrang_id;
            //txtSoHieu_CT.Text = objCTTaiSan.chungtu.sohieu;
            txtSoLuong.Properties.MinValue = 0;
            txtSoLuong.Properties.MaxValue = objCTTaiSan.soluong;
            txtSoLuong.EditValue = objCTTaiSan.soluong;
            lbltxtDonViTinh.Text = objCTTaiSan.taisan.loaitaisan.donvitinh != null ? objCTTaiSan.taisan.loaitaisan.donvitinh.ten : "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //DateTime ngayGhi = dateNgayGhi.EditValue != null ? dateNgayGhi.DateTime : DateTime.Now;
                //objChungTu.sohieu = txtSoHieu_CT.Text;
                //objChungTu.ngay = dateNgay_CT.EditValue != null ? dateNgay_CT.DateTime : DateTime.Now;
                int soLuong = Convert.ToInt32(txtSoLuong.EditValue);
                TinhTrang tinhTrang = TinhTrang.getById(lookUpTinhTrang.EditValue);
                String ghiChu = txtGhiChu.Text;
                CTTaiSan re = objCTTaiSan.chuyenTinhTrang(objCTTaiSan.chungtu, tinhTrang, soLuong, ghiChu);
                if (re != null && DBInstance.commit() > 0)
                {
                    XtraMessageBox.Show("Chuyển tình trạng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Guid id = CTTaiSan.getQuery().Where(c => c.taisan_id == objCTTaiSan.taisan_id && c.tinhtrang_id == tinhTrang.id && c.soluong == soLuong).FirstOrDefault().id;
                    if (reloadAndFocused != null)
                        reloadAndFocused(re.id);
                    this.Close();
                }
                else
                    XtraMessageBox.Show("Chuyển tình trạng không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->btnOK_Click: " + ex.Message);
            }
        }

        private void btnTinhTrang_Click(object sender, EventArgs e)
        {
            frmQuanLyTinhTrang frm = new frmQuanLyTinhTrang();
            frm.ShowDialog();
            loadData();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btnAttachment_Click(object sender, EventArgs e)
        //{
        //    if (objChungTu != null)
        //    {
        //        frmFileChungTu frm = new frmFileChungTu(objChungTu);
        //        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            objChungTu = frm.ct;
        //    }
        //}
    }
}