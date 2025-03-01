﻿<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="PhongThietBi.aspx.cs" Inherits="PTB_WEB.PhongThietBi" %>

<%@ Register Src="~/UserControl/PhongThietBi/ucPhongThietBi_Web.ascx" TagPrefix="uc" TagName="ucPhongThietBi_Web" %>
<%@ Register Src="~/UserControl/PhongThietBi/ucPhongThietBi_Mobile.ascx" TagPrefix="uc" TagName="ucPhongThietBi_Mobile" %>
<%@ Register Src="~/UserControl/ucFooter.ascx" TagPrefix="uc" TagName="ucFooter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>.: Thiết bị từng phòng :: Quản lý Thiết bị :.</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel_Web" runat="server" Visible="false">
        <uc:ucPhongThietBi_Web runat="server" ID="_ucPhongThietBi_Web" />
    </asp:Panel>
    <asp:Panel ID="Panel_Mobile" runat="server" Visible="false">
        <uc:ucPhongThietBi_Mobile runat="server" ID="_ucPhongThietBi_Mobile" />
    </asp:Panel>
    <uc:ucFooter runat="server" ID="ucFooter" />
</asp:Content>
