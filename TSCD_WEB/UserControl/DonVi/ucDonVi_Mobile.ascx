﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDonVi_Mobile.ascx.cs" Inherits="TSCD_WEB.UserControl.DonVi.ucDonVi_Mobile" %>
<%@ Register Src="~/UserControl/DonVi/ucDonVi_BreadCrumb.ascx" TagPrefix="uc" TagName="ucDonVi_BreadCrumb" %>
<%@ Register Src="~/UserControl/TreeViTri/ucTreeViTri.ascx" TagPrefix="uc" TagName="ucTreeViTri" %>
<%@ Register Src="~/UserControl/Alert/ucDanger.ascx" TagPrefix="uc" TagName="ucDanger" %>

<uc:ucDonVi_BreadCrumb runat="server" ID="ucDonVi_BreadCrumb" />

<table class="table largetable noboder">
    <tbody>
        <tr id="thongbaoloi" runat="server" visible="false">
            <td>
                <uc:ucDanger runat="server" ID="ucDanger" />
            </td>
        </tr>

        <tr id="treevitri" runat="server" visible="false">
            <td>
                <uc:ucTreeViTri runat="server" ID="ucTreeViTri" />
            </td>
        </tr>
        <tr id="info" runat="server" visible="false">
            <td>
                <h3 class="title_green fix">
                    <asp:Label ID="Label_ThongTin" runat="server" Text="Thông tin"></asp:Label>
                </h3>
                <table class="table table-striped">
                    <tbody>
                        <tr>
                            <td>Mã đơn vị:</td>
                            <td>
                                <asp:Label ID="Label_Ma" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên đơn vị:</td>
                            <td>
                                <asp:Label ID="Label_Ten" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Loại đơn vị:</td>
                            <td>
                                <asp:Label ID="Label_Loai" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Thuộc:</td>
                            <td>
                                <asp:Label ID="Label_Thuoc" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Mô tả:</td>
                            <td>
                                <asp:Label ID="Label_MoTa" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
