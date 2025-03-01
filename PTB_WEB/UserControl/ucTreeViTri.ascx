﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTreeViTri.ascx.cs" Inherits="PTB_WEB.UserControl.ucTreeViTri" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <h3 class="title_orange fix">
            <asp:Label ID="Label_TenViTri" runat="server"></asp:Label>
            <asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="UpdatePanel" DisplayAfter="0" DynamicLayout="false" ClientIDMode="Static">
                <ProgressTemplate>
                    <img alt="Đang xử lý..." src="/Images/loading.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:LinkButton ID="LinkButton_Expand" runat="server" ToolTip="Expand All" OnClick="LinkButton_Expand_Click"><span class="pull-right" style="cursor: pointer;"><img src="/Images/ExpandAllIcon.png" alt="Expand All" /></span></asp:LinkButton>
            <asp:LinkButton ID="LinkButton_Collapse" runat="server" ToolTip="Collapse All" OnClick="LinkButton_Collapse_Click"><span class="pull-right" style="cursor: pointer"><img src="/Images/CollapseAllIcon.png" alt="Collapse All" style="padding-right:10px;" /></span></asp:LinkButton>
            <h3></h3>
            <asp:Panel ID="PanelTreeList" runat="server"></asp:Panel>
            <h3></h3>
        </h3>
    </ContentTemplate>
</asp:UpdatePanel>
