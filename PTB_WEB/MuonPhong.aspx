﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="MuonPhong.aspx.cs" Inherits="PTB_WEB.MuonPhong" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/ucDangNhap.ascx" TagPrefix="uc" TagName="ucDangNhap" %>
<%@ Register Src="~/UserControl/ucFooter.ascx" TagPrefix="uc" TagName="ucFooter" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>.: Biểu mẫu mượn phòng :: Quản lý Thiết bị :.</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
        <li><a href="Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
        <li><a href="MuonPhong.aspx">Mượn phòng</a></li>
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
                    <asp:Panel ID="PanelThongBaoMuonPhongThanhCong" runat="server" Visible="false">
                        <div class="alert alert-success alert-dismissible" role="alert">
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <strong>Hoàn thành!</strong> Chúng tôi sẽ xem xét và gửi thông báo cho bạn trong thời gian sớm nhất.
            <a href="MuonPhong.aspx" class="alert-link">Click vào đây để mượn thêm phòng</a>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelKhongPhaiGiangVien" runat="server" Visible="false">
                        <div class="alert alert-warning alert-dismissible" role="alert">
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <span class="glyphicon glyphicon-info-sign"></span>
                            Tài khoản này không được phép mượn phòng. Liên hệ với chúng tôi để biết thêm chi tiết.
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelMuonPhong" runat="server" Visible="False">

                        <h3 class="title_green fix"><b>ĐỀ NGHỊ VỀ VIỆC NHU CẦU SỬ DỤNG PHÒNG</b></h3>
                        <div class="col-lg-12">
                            <asp:Panel ID="PanelThongBaoMuonPhong" runat="server" Visible="false">
                                <div class="alert alert-warning alert-dismissible" role="alert">
                                    <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>

                                    <span class="glyphicon glyphicon-info-sign"></span>
                                    <asp:Label ID="LabelThongBaoMuonPhong" runat="server" Text="Label"></asp:Label>
                                </div>
                            </asp:Panel>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Khoa(Phòng) mượn</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TextBoxKhoa" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Ngày mượn</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='datetimepickerNgayMuon'>
                                        <asp:TextBox ID="TextBoxNgayMuon" CssClass="form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                    <script type="text/javascript">
                                        $(function () {
                                            $('#datetimepickerNgayMuon').datetimepicker({ pickTime: false });
                                        });
                                    </script>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Thời gian mượn</label>
                                <div class="col-sm-3">
                                    <div class='input-group time' id='datetimepickerThoiGianMuon' data-date-format="HH:mm">
                                        <asp:TextBox ID="TextBoxThoiGianMuon" CssClass="form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span>
                                        </span>
                                    </div>
                                    <script type="text/javascript">
                                        $(function () {
                                            $('#datetimepickerThoiGianMuon').datetimepicker({ pickDate: false });
                                        });
                                    </script>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Thời gian trả</label>
                                <div class="col-sm-3">
                                    <div class='input-group time' id='datetimepickerThoiGianTra' data-date-format="HH:mm">
                                        <asp:TextBox ID="TextBoxThoiGianTra" CssClass="form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span>
                                        </span>
                                    </div>
                                    <script type="text/javascript">
                                        $(function () {
                                            $('#datetimepickerThoiGianTra').datetimepicker({ pickDate: false });
                                        });
                                    </script>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Số lượng phòng mượn</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TextBoxPhong" runat="server" CssClass="form-control" placeholder="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Số lượng sinh viên/phòng</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TextBoxSoLuong" runat="server" CssClass="form-control" placeholder="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Lớp</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TextBoxLop" runat="server" CssClass="form-control" placeholder="DCT1104"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Lý do sử dụng</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TextBoxLyDoSuDung" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Dạy bù cho sinh viên" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-offset-3 col-sm-8">
                                    <asp:Button ID="ButtonMuonPhong" runat="server" Text="Mượn phòng" CssClass="btn btn-success" OnClick="ButtonMuonPhong_Click" />
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
