﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLoaiThietBi_Mobile.ascx.cs" Inherits="WebQLPH.UserControl.LoaiThietBis.ucLoaiThietBi_Mobile" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxImageSlider" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/LoaiThietBi/ucLoaiThietBi_BreadCrumb.ascx" TagPrefix="uc" TagName="ucLoaiThietBi_BreadCrumb" %>


<asp:Panel ID="Panel_ThongBaoLoi" runat="server" Visible="False">
    <div class="row">
        <div class="alert alert-danger" role="alert">
            <span class="glyphicon glyphicon-exclamation-sign"></span>
            <asp:Label ID="Label_ThongBaoLoi" runat="server" Text="Label"></asp:Label>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="Panel_Chinh" runat="server" Visible="False">
    <uc:ucLoaiThietBi_BreadCrumb runat="server" ID="ucLoaiThietBi_BreadCrumb" />
    <asp:Panel ID="Panel_TreeViTri" runat="server" Visible="False">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Loại thiết bị
            </div>
            <dx:ASPxTreeList ID="ASPxTreeList_LoaiThietBi" runat="server" AutoGenerateColumns="False" KeyFieldName="id" ParentFieldName="id_parent" Theme="Aqua" ClientInstanceName="treeList" Width="100%" EnableTheming="True" OnCustomDataCallback="ASPxTreeList_LoaiThietBi_CustomDataCallback" OnFocusedNodeChanged="ASPxTreeList_LoaiThietBi_FocusedNodeChanged">
                <Columns>
                    <dx:TreeListTextColumn Caption="Tên" FieldName="ten" Name="colten" VisibleIndex="0" ShowInCustomizationForm="True">
                    </dx:TreeListTextColumn>
                </Columns>
                <Settings ShowColumnHeaders="false" ShowTreeLines="true" />
                <SettingsBehavior AllowFocusedNode="True" FocusNodeOnExpandButtonClick="False" />
                <SettingsCookies Enabled="True" StoreExpandedNodes="True" StorePaging="True" />
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                <ClientSideEvents CustomDataCallback="function(s, e) { document.location = e.result; }"
                    Init="function(s, e) {
                    s.SetFocusedNodeKey('');
                }"
                    NodeClick="function(s, e) {
	                var key = e.nodeKey;
	                treeList.PerformCustomDataCallback(key);
                }" />
            </dx:ASPxTreeList>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel_ThongTinObj" runat="server" Visible="False">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Label_ThongTin" runat="server" Text="Thông tin"></asp:Label>
            </div>
            <div class="panel-body">
                <table class="table table-bordered">
                    <tr>
                        <th style="width: 120px" class="warning">Tên loại</th>
                        <td>
                            <asp:Label ID="Label_TenLoai" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <th class="warning">Kiểu quản lý</th>
                        <td>
                            <asp:Label ID="Label_KieuQuanLy" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <th class="warning">Mô tả</th>
                        <td>
                            <asp:Label ID="Label_MoTa" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <th class="warning">Thuộc</th>
                        <td>
                            <asp:Label ID="Label_Thuoc" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:Button ID="Button_Back" CssClass="btn btn-default" runat="server" Text="Quay lại" OnClick="Button_Back_Click" Width="100px" />
    </asp:Panel>
</asp:Panel>
