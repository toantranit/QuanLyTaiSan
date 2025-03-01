﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PTB_WEB.Default1" %>

<%@ Register Src="~/UserControl/TimKiem.ascx" TagPrefix="uc" TagName="TimKiem" %>
<%@ Register Src="~/UserControl/ucFooter.ascx" TagPrefix="uc" TagName="ucFooter" %>
<%@ Register Src="~/UserControl/Search/ucTimKiem_Mobile.ascx" TagPrefix="uc" TagName="ucTimKiem_Mobile" %>
<%@ Register Src="~/UserControl/Search/ucTimKiem_Web.ascx" TagPrefix="uc" TagName="ucTimKiem_Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>.: Quản lý Thiết bị :.</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="Content/css/metro/metro.css" />
    <link rel="stylesheet" type="text/css" href="Content/css/metro/metro_mobile.css" media="screen and (max-height: 500px), screen and (orientation:portrait)" />
    <link rel="stylesheet" type="text/css" href="Content/css/metro/website.css" />
    <script type="text/javascript" src="Scripts/scriptgates.js"></script>

    <%if (isMobile)
      {%>
    <uc:ucTimKiem_Mobile runat="server" ID="ucTimKiem_Mobile" />
    <%}
      else
      {%>
    <uc:ucTimKiem_Web runat="server" ID="ucTimKiem_Web" />
    <%}%>
    <div class="panel-body fixpanel">
        <div class="row">
            <h3 class="title_orange fix">Quản lý Thiết bị</h3>
        </div>
        <div class="row">
            <div class="widget_container full">
                <div class="widget widget2x2 widget_orange animation unloaded">
                    <a href="ViTri.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/vitri.png');">
                                <span>VỊ TRÍ</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_red animation unloaded">
                    <a href="Phong.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/phong.png');">
                                <span>PHÒNG</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_darkblue animation unloaded">
                    <a href="PhongThietBi.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/phongthietbi.png');">
                                <span>PHÒNG THIẾT BỊ</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_blue widget_link animation unloaded">
                    <a href="ThietBis.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/thietbi.png');">
                                <span>THIẾT BỊ</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_darkgreen animation unloaded">
                    <a href="LoaiThietBis.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/loaithietbi.png');">
                                <span>LOẠI THIẾT BỊ</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_darkred animation unloaded">
                    <a href="NhanVien.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/nhanvien.png');">
                                <span>NHÂN VIÊN</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_green animation unloaded">
                    <a href="SuCo.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/suco.png');">
                                <span>SỰ CỐ</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <div class="row">
            <h3 class="title_green fix">Khác</h3>
        </div>
        <div class="row">
            <div class="widget_container full">
                <div class="widget widget4x2 widget_blue animation unloaded">
                    <a href="MuonPhong.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/muonphong.png');">
                                <span>MƯỢN PHÒNG</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget4x2 widget_orange animation unloaded">
                    <a href="QuanLyHinhAnh.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/quanlyhinhanh.png');">
                                <span>QUẢN LÝ HÌNH ẢNH</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_blue animation unloaded">
                    <a href="ThongTin.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/thongtin.png');">
                                <span>THÔNG TIN</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_green animation unloaded">
                    <a href="LienHe.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/lienhe.png');">
                                <span>LIÊN HỆ</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <div class="row">
            <h3 class="title_blue fix">Bảng điều khiển</h3>
        </div>
        <div class="row">
            <div class="widget_container full">
                <%if (Session["UserName"] == null)
                  { %>
                <div class="widget widget2x2 widget_darkblue animation unloaded">
                    <a href="DangNhap.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/dangnhap.png');">
                                <span>ĐĂNG NHẬP</span>
                            </div>
                        </div>
                    </a>
                </div>
                <%}
                  else
                  { %>
                <div class="widget widget4x2 widget_blue animation unloaded">
                    <a href="ThongTinCaNhan.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/thongtincanhan.png');">
                                <span>THÔNG TIN CÁ NHÂN</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget4x2 widget_green animation unloaded">
                    <a href="QuanLyMuonPhong.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/quanlymuonphong.png');">
                                <span>QUẢN LÝ MƯỢN PHÒNG</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget4x2 widget_blue animation unloaded">
                    <a href="QuanLyTaiKhoan.aspx">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/quanlytaikhoan.png');">
                                <span>QUẢN LÝ TÀI KHOẢN</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="widget widget2x2 widget_red animation unloaded">
                    <a href="?op=thoat" onclick="return confirm('Bạn muốn thoát?');">
                        <div class="widget_content">
                            <div class="main" style="background-image: url('Images/metro/thoat.png');">
                                <span>THOÁT RA</span>
                            </div>
                        </div>
                    </a>
                </div>
                <%} %>
            </div>
        </div>
    </div>
    <uc:ucFooter runat="server" ID="ucFooter" />
</asp:Content>
