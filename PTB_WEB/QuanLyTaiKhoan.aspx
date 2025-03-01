﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="QuanLyTaiKhoan.aspx.cs" Inherits="PTB_WEB.QuanLyTaiKhoan" %>

<%@ Register Src="~/UserControl/ucDangNhap.ascx" TagPrefix="uc" TagName="ucDangNhap" %>
<%@ Register TagPrefix="cp" Namespace="SiteUtils" Assembly="CollectionPager" %>
<%@ Register Src="~/UserControl/ucFooter.ascx" TagPrefix="uc" TagName="ucFooter" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>.: Quản lý tài khoản :: Quản lý Thiết bị :.</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
        <li><a href="Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
        <li><a href="QuanLyTaiKhoan.aspx">Quản lý tài khoản</a></li>
    </ol>
    <table class="table largetable">
        <tbody>
            <tr>
                <td>
                    <asp:Panel ID="PanelDangNhap" runat="server" Visible="false">
                        <div class="center">
                            <uc:ucDangNhap runat="server" ID="ucDangNhap" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelKhongPhaiQuanTriVien" runat="server" Visible="false">
                        <div class="alert alert-warning alert-dismissible" role="alert">
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <span class="glyphicon glyphicon-info-sign"></span>
                            Bạn không có quyền xem thông tin tài khoản.
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelQuanLyTaiKhoan" runat="server" Visible="false">
                        <asp:Panel ID="PanelThanhCong" runat="server" Visible="false">
                            <div class="alert alert-success" role="alert">
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <asp:Label ID="LabelThongBaoThanhCong" runat="server" Text="Label"></asp:Label>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="PanelThatBai" runat="server" Visible="false">
                            <div class="alert alert-danger" role="alert">
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <asp:Label ID="LabelThongBaoThatBai" runat="server" Text="Label"></asp:Label>
                            </div>
                        </asp:Panel>
                        <div class="buttonthem">
                            <asp:Button ID="ButtonThemMoiTaiKhoan" data-target="#PopupQuanLyTaiKhoan" OnClientClick="ShowThemMoi(); return false;" data-toggle="modal" runat="server" Text="Thêm mới" CssClass="btn btn-primary" ClientIDMode="Static" />
                        </div>
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <h3 class="panel-title"><b>DANH SÁCH TÀI KHOẢN</b></h3>
                            </div>
                            <table class="table table-bordered table-striped valign_middle">
                                <thead>
                                    <tr>
                                        <th class="tdcenter">STT</th>
                                        <th>HỌ TÊN</th>
                                        <th>EMAIL</th>
                                        <th>TÀI KHOẢN</th>
                                        <th>KHOA</th>
                                        <th>NGÀY TẠO</th>
                                        <th>MÔ TẢ</th>
                                        <th class="tdcenter">CÔNG CỤ</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RepeaterQuanLyTaiKhoan" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="tdcenter"><%# Container.ItemIndex + 1 + ((CollectionPagerQuanLyTaiKhoan.CurrentPage - 1) * CollectionPagerQuanLyTaiKhoan.PageSize) %></td>
                                                <td id="hoten<%#Eval("id")%>"><%# Eval("hoten") %></td>
                                                <td id="email<%#Eval("id")%>"><%# Eval("email") %></td>
                                                <td id="username<%#Eval("id")%>"><%# Eval("username") %></td>
                                                <td id="khoa<%#Eval("id")%>"><%# Eval("donvi") %></td>
                                                <td><%# NgayTao() %></td>
                                                <td id="mota<%#Eval("id")%>"><%#MoTa()%></td>
                                                <td class="tdcenter">
                                                    <div class="btn-group">
                                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown">
                                                            Công cụ&nbsp;<span class="caret"></span>
                                                        </button>
                                                        <ul class="dropdown-menu" role="menu">
                                                            <%#_QuyenSuaQuanTriVien()%>
                                                            <%#_QuyenXoaQuanTriVien()%>
                                                        </ul>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <div class="centerCollectionPager">
                            <div class="CollectionPager">
                                <cp:CollectionPager ID="CollectionPagerQuanLyTaiKhoan" runat="server" LabelText="" MaxPages="20" ShowLabel="False" BackNextDisplay="HyperLinks" BackNextLinkSeparator="" BackNextLocation="None" BackText="" EnableViewState="False" FirstText="&laquo;" LabelStyle="FONT-WEIGHT: blue;" LastText="&raquo;" NextText="" PageNumbersSeparator="" PageSize="30" PagingMode="QueryString" QueryStringKey="Trang" ResultsFormat="" ResultsLocation="None" ResultsStyle="" ShowFirstLast="True" ClientIDMode="Static">
                                </cp:CollectionPager>
                            </div>
                        </div>
                        <!-- Popup Quản lý -->
                        <div class="modal fade" id="PopupQuanLyTaiKhoan" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                        <h4 class="modal-title" id="myModalLabel"></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <asp:HiddenField ID="HiddenFieldID" ClientIDMode="Static" runat="server" />
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4">Họ tên(*):</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxHoTen" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4">Email(*):</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4">Nhóm(*):</div>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="DropDownListNhom" runat="server" CssClass="form-control" ClientIDMode="Static" DataTextField="ten" DataValueField="id">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4">Tài khoản(*):</div>
                                            <div class="col-lg-8">
                                                <i id="ThongBao" style="font-size: small"></i>
                                                <asp:TextBox ID="TextBoxTaiKhoan" runat="server" AutoCompleteType="Disabled" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4" id="matkhau">Mật khẩu(*):</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxMatKhau" runat="server" TextMode="Password" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                <i id="thongbaomatkhau" style="font-size: small; color: blue">Để trống nếu không muốn thay đổi</i>
                                            </div>
                                        </div>
                                        <div class="row" id="rownhaplaimatkhau">
                                            <br />
                                            <div class="col-lg-4" id="nhaplaimatkhau">Nhập lại mật khẩu(*):</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxNhapLaiMatKhau" runat="server" TextMode="Password" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4">Khoa(*):</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxKhoa" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-4">Ghi chú:</div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="TextBoxGhiChu" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                                        <asp:Button ID="ButtonThemMoi" ClientIDMode="Static" CssClass="btn btn-primary" runat="server" Text="Thêm mới" OnClick="ButtonThemMoi_Click" OnClientClick="return ThemMoi();" />
                                        <asp:Button ID="ButtonLuu" ClientIDMode="Static" CssClass="btn btn-primary" runat="server" Text="Lưu" OnClientClick="return CapNhat();" OnClick="ButtonLuu_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </tbody>
    </table>
    <uc:ucFooter runat="server" ID="ucFooter" />
</asp:Content>
