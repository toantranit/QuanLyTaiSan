﻿using QuanLyTaiSan.Entities;
using QuanLyTaiSanGUI.HeThong;
using QuanLyTaiSanGUI.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTaiSanGUI
{
    public partial class Setting : Form
    {
        /// <summary>
        /// Sẽ tự động mở Login lên sau khi Close()
        /// </summary>
        private Boolean cau_hinh_ban_dau = true;

        private ucCauHinh _ucCauHinh = null;
        private ucThongTinPhanMem _ucThongTinPhanMem = null;
        public Setting()
        {
            InitializeComponent();
        }
        public Setting(Boolean cauHinhBanDau=true)
        {
            InitializeComponent();
            this.cau_hinh_ban_dau = cauHinhBanDau;
        }

        private void btnCauHinh_Click(object sender, EventArgs e)
        {
            if (_ucCauHinh == null)
            {
                _ucCauHinh = new ucCauHinh();
                _ucCauHinh.load_data();
            }
            panelControlHienThiCauHinh.Controls.Clear();
            _ucCauHinh.Dock = DockStyle.Fill;
            panelControlHienThiCauHinh.Controls.Add(_ucCauHinh);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_ucCauHinh != null)
            {
                //call ucSave
                int re = _ucCauHinh.save();
                
                
                if (re > 0)
                {
                    this.custom_close();
                }
                else
                {
                    MessageBox.Show("Không thể tạo kết nối tới Database!");
                }
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.custom_close();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            //Check kết nối tới CSDL nếu OK thì gọi Close
            if (Global.working_database.isReady() && cau_hinh_ban_dau)
            {
                this.custom_close();
            }
            else //ngược lại set form defaul click Cấu hình. mấy nút kia disable
            {
                btnCauHinh.PerformClick();
                btnGiaoDienvaNgonNgu.Visible = false;
                btnCapNhatPhanMem.Visible = false;
            }
        }
        /// <summary>
        /// Quyết định hành động kế tiếp khi hoàn tất form setting
        /// </summary>
        private void custom_close()
        {
            if (cau_hinh_ban_dau)
            {
                this.show_frm_login();
            }
            else
            {
                this.Close();
            }
        }
        #region show from login in new thread
        private void ThreadProc()
        {
            Application.Run(new Login());
        }
        private void show_frm_login()
        {
            Thread thread = new Thread(new ThreadStart(ThreadProc));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            Application.Exit();
        }
        #endregion

        private void btnThongTinPhanMem_Click(object sender, EventArgs e)
        {
            _ucThongTinPhanMem = new ucThongTinPhanMem();
            panelControlHienThiCauHinh.Controls.Clear();
            _ucThongTinPhanMem.Dock = DockStyle.Fill;
            panelControlHienThiCauHinh.Controls.Add(_ucThongTinPhanMem);
        }
    }
}
