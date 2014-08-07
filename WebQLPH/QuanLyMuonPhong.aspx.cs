﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuanLyTaiSan.Entities;

namespace WebQLPH
{
    public partial class QuanLyMuonPhong : System.Web.UI.Page
    {
        List<PhieuMuonPhong> ListPhieuMuonPhong = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;

            if (!IsPostBack)
            {
                try
                {
                    if (Convert.ToString(Session["Username"]).Equals(String.Empty))
                        Response.Redirect("DangNhap.aspx");
                    QuanLyPhongMuon();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }        

        protected void QuanLyPhongMuon()
        {
            if (LaQuanTriVien())
            {
                LabelPanel.Text = "DANH SÁCH GIẢNG VIÊN MƯỢN PHÒNG";
                ListPhieuMuonPhong = PhieuMuonPhong.getQuery().OrderByDescending(c => c.id).ToList();
            }
            else
            {
                LabelPanel.Text = "DANH SÁCH PHÒNG BẠN ĐÃ MƯỢN";
                GiangVien _GiangVien = GiangVien.getByUserName(Convert.ToString(Session["UserName"]));
                ListPhieuMuonPhong = _GiangVien.phieumuonphongs.OrderByDescending(c => c.id).ToList();
            }

            CollectionPagerQuanLyMuonPhong.DataSource = ListPhieuMuonPhong;
            CollectionPagerQuanLyMuonPhong.BindToControl = RepeaterQuanLyMuonPhong;
            RepeaterQuanLyMuonPhong.DataSource = CollectionPagerQuanLyMuonPhong.DataSourcePaged;
            RepeaterQuanLyMuonPhong.DataBind();
        }

        protected bool LaQuanTriVien()
        {
            return Convert.ToString(Session["KieuDangNhap"]).Equals("QuanTriVien");
        }

        protected string NgayTao()
        {
            DateTime dt = Convert.ToDateTime(Eval("date_create").ToString());
            return dt.ToString("HH\\hmm d/M/yyyy");
        }
        protected string NgayMuon()
        {
            DateTime dt = Convert.ToDateTime(Eval("ngaymuon").ToString());
            return dt.ToString("d/M/yyyy");
        }
        protected string Tu()
        {
            DateTime dt = Convert.ToDateTime(Eval("ngaymuon").ToString());
            return dt.ToString("HH\\hmm");
        }
        protected string Den()
        {
            DateTime dt = Convert.ToDateTime(Eval("ngaytra").ToString());
            return dt.ToString("HH\\hmm");
        }
        protected string Duyet()
        {
            int trangthai = Convert.ToInt32(Eval("trangthai").ToString());
            string str = string.Empty;
            if (LaQuanTriVien())
            {
                str = string.Format("data-toggle='modal' data-target='#PopupDuyet' onclick=\"return Duyet('{0}','{1}');\">", Eval("id"), Eval("trangthai"));
                switch (trangthai)
                {
                    case 0:
                        str = "<button class='btn btn-primary btn-sm' " + str + "Chờ duyệt</span>";
                        break;
                    case 1:
                        str = "<button class='btn btn-success btn-sm' " + str + "Chấp nhận</span>";
                        break;
                    case -1:
                        str = "<button class='btn btn-danger btn-sm' " + str + "Hủy bỏ</span>";
                        break;
                }
            }
            else
            {
                switch (trangthai)
                {
                    case 0:
                        str = "<label class='label label-primary btn-sm'>Chờ duyệt</span>";
                        break;
                    case 1:
                        str = "<label class='label label-success btn-sm'>Chấp nhận</span>";
                        break;
                    case -1:
                        str = "<label class='label label-danger btn-sm'>Hủy bỏ</span>";
                        break;
                }
            }
            return str;
        }

        protected void ButtonLuu_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuMuonPhong _PhieuMuonPhong = new PhieuMuonPhong();
                _PhieuMuonPhong = PhieuMuonPhong.getById(Convert.ToInt32(HiddenFieldID.Value));
                _PhieuMuonPhong.trangthai = Convert.ToInt32(DropDownListTrangThai.SelectedValue);
                _PhieuMuonPhong.ghichu = TextBoxGhiChu.Text;
                QuanTriVien _QuanTriVien = new QuanTriVien();
                _QuanTriVien = QuanTriVien.getByUserName(Session["username"].ToString());
                _PhieuMuonPhong.quantrivien = _QuanTriVien;
                if (_PhieuMuonPhong.update() > 0)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}