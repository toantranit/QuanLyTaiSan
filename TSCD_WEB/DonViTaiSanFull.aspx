﻿<%@ Page Title="" Language="C#" MasterPageFile="~/PopupSite.Master" AutoEventWireup="true" CodeBehind="DonViTaiSanFull.aspx.cs" Inherits="TSCD_WEB.DonViTaiSanFull" %>

<%@ Register Src="~/UserControl/TreeViTri/ucTreeViTri.ascx" TagPrefix="uc" TagName="ucTreeViTri" %>
<%@ Register Src="~/UserControl/Alert/ucWarning.ascx" TagPrefix="uc" TagName="ucWarning" %>
<%@ Register Src="~/UserControl/Alert/ucDanger.ascx" TagPrefix="uc" TagName="ucDanger" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/DangNhap/ucDangNhap.ascx" TagPrefix="uc" TagName="ucDangNhap" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="table largetable">
        <tbody>
            <tr id="DangNhap" runat="server" visible="false">
                <td>
                    <uc:ucDangNhap runat="server" ID="ucDangNhap" />
                </td>
            </tr>
            <tr id="KhongCoDuLieu" runat="server" visible="false">
                <td>
                    <uc:ucDanger runat="server" ID="ucDanger_KhongCoDuLieu" />
                </td>
            </tr>
            <tr id="infotr" runat="server" visible="false">
                <td id="tdvitri" clientidmode="Static" runat="server" style="width: 300px;" class="border_right">
                    <uc:ucTreeViTri runat="server" ID="_ucTreeViTri" />
                </td>
                <td id="ChuaChonViTri" runat="server" visible="false">
                    <uc:ucWarning runat="server" ID="ucWarning_ChuaChon" />
                </td>
                <td id="infotd" clientidmode="Static" runat="server" visible="false">
                    <script type="text/javascript">
                        // <![CDATA[
                        var textSeparator = ";";
                        var valueSeparator = ";";
                        function OnListBoxSelectionChanged(listBox, args) {
                            if (args.index == 0)
                                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
                            UpdateSelectAllItemState();
                            UpdateText();
                        }
                        function UpdateSelectAllItemState() {
                            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
                        }
                        function IsAllSelected() {
                            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
                            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
                        }
                        function UpdateText() {
                            var selectedItems = checkListBox.GetSelectedItems();
                            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
                            $("#HiddenField").val(GetSelectedItemsValue(selectedItems));
                        }
                        function SynchronizeListBoxValues(dropDown, args) {
                            checkListBox.UnselectAll();
                            var texts = dropDown.GetText().split(textSeparator);
                            var values = GetValuesByTexts(texts);
                            checkListBox.SelectValues(values);
                            UpdateSelectAllItemState();
                            UpdateText(); // for remove non-existing texts
                        }
                        function GetSelectedItemsText(items) {
                            var texts = [];
                            for (var i = 0; i < items.length; i++)
                                if (items[i].index != 0)
                                    texts.push(items[i].text);
                            return texts.join(textSeparator);
                        }
                        function GetSelectedItemsValue(items) {
                            var values = [];
                            for (var i = 0; i < items.length; i++)
                                if (items[i].index != 0)
                                    values.push(items[i].value);
                            return values.join(valueSeparator);
                        }
                        function GetValuesByTexts(texts) {
                            var actualValues = [];
                            var item;
                            for (var i = 0; i < texts.length; i++) {
                                item = checkListBox.FindItemByText(texts[i]);
                                if (item != null)
                                    actualValues.push(item.value);
                            }
                            return actualValues;
                        }
                        // ]]>
                    </script>
                    <table>
                        <tr>
                            <h3 class="title_green fix">
                                <asp:LinkButton ID="LinkButton_ThuLai" runat="server" OnClick="LinkButton_ThuLai_Click" ToolTip="Thu Lai"><i class="glyphicon glyphicon-chevron-left"></i></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton_MoRa" runat="server" OnClick="LinkButton_MoRa_Click" ToolTip="Mo Ra"><i class="glyphicon glyphicon-chevron-right"></i></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton_Expand" runat="server" OnClick="LinkButton_Expand_Click" ToolTip="Expand All"><i class="glyphicon glyphicon-plus"></i></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton_Collapse" runat="server" OnClick="LinkButton_Collapse_Click" ToolTip="Collapse All"><i class="glyphicon glyphicon-minus"></i></asp:LinkButton>
                                Danh sách tài sản
                            </h3>
                            <td>Chọn cột cần hiển thị</td>
                            <td style="padding-left: 10px">
                                <dx:ASPxDropDownEdit ID="ASPxDropDownEdit" ClientInstanceName="checkComboBox" Width="210px" Height="25px" runat="server" Theme="Aqua">
                                    <DropDownWindowTemplate>
                                        <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" runat="server" Theme="Aqua">
                                            <Items>
                                                <dx:ListEditItem Text="(Chọn tất cả)" />
                                                <dx:ListEditItem Text="Loại tài sản" Value="loaits" />
                                                <dx:ListEditItem Text="Ngày sử dụng" Value="ngay" />
                                                <dx:ListEditItem Text="Số hiệu chứng từ" Value="sohieu_ct" />
                                                <dx:ListEditItem Text="Số hiệu chứng từ" Value="ngay_ct" />
                                                <dx:ListEditItem Text="Tên Tài sản cố định" Value="ten" />
                                                <dx:ListEditItem Text="Đơn vị tính" Value="donvitinh" />
                                                <dx:ListEditItem Text="Số lượng" Value="soluong" />
                                                <dx:ListEditItem Text="Đơn giá" Value="dongia" />
                                                <dx:ListEditItem Text="Thành tiền" Value="thanhtien" />
                                                <dx:ListEditItem Text="Tình trạng" Value="tinhtrang" />
                                                <dx:ListEditItem Text="Nước sản xuất" Value="nuocsx" />
                                                <dx:ListEditItem Text="Nguồn gốc" Value="nguongoc" />
                                                <dx:ListEditItem Text="Phòng" Value="phong" />
                                                <dx:ListEditItem Text="Vị trí" Value="vitri" />
                                                <dx:ListEditItem Text="Đơn vị quản lý" Value="dvquanly" />
                                                <dx:ListEditItem Text="Đơn vị sử dụng" Value="dvsudung" />
                                                <dx:ListEditItem Text="Ghi chú" Value="ghichu" />
                                            </Items>
                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                        </dx:ASPxListBox>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="padding: 4px">
                                                    <dx:ASPxButton ID="ASPxButton" runat="server" Text="OK" Style="float: right" Theme="Aqua" OnClick="ASPxButton_Click">
                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </DropDownWindowTemplate>
                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                </dx:ASPxDropDownEdit>
                                <asp:HiddenField ID="HiddenField" ClientIDMode="Static" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <dx:ASPxGridView ID="ASPxGridView" KeyFieldName="id" ClientIDMode="Static" ClientInstanceName="ASPxGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Metropolis" Width="100%">
                    <GroupSummary>
                        <dx:ASPxSummaryItem FieldName="soluong" ShowInColumn="loaits" SummaryType="Sum" DisplayFormat="Số lượng = {0}" />
                        <dx:ASPxSummaryItem FieldName="thanhtien" ShowInColumn="loaits" SummaryType="Sum" DisplayFormat="Tổng tiền = {0:### ### ### ###}" />
                    </GroupSummary>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Loại tài sản" FieldName="loaits" VisibleIndex="1" GroupIndex="1" SortIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Ngày sử dụng" FieldName="ngay" VisibleIndex="2">
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Số hiệu chứng từ" FieldName="sohieu_ct" VisibleIndex="3" Width="120">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Ngày tháng chứng từ" FieldName="ngay_ct" VisibleIndex="4" Width="140">
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Tên TSCĐ" FieldName="ten" VisibleIndex="5" Width="300">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn vị tính" FieldName="donvitinh" VisibleIndex="6" Width="70">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Số lượng" FieldName="soluong" VisibleIndex="7" Width="60">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn giá" FieldName="dongia" VisibleIndex="8" Width="150">
                            <PropertiesTextEdit DisplayFormatString="### ### ### ###"></PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Thành tiền" FieldName="thanhtien" VisibleIndex="9" Width="150">
                            <PropertiesTextEdit DisplayFormatString="### ### ### ###"></PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Tình trạng" FieldName="tinhtrang" VisibleIndex="10">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Nước sản xuất" FieldName="nuocsx" VisibleIndex="11" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Nguồn gốc" FieldName="nguongoc" VisibleIndex="12" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="ghichu" VisibleIndex="13" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Phòng" FieldName="phong" VisibleIndex="14" GroupIndex="0" SortIndex="0" SortOrder="Ascending">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Vị trí" FieldName="vitri" VisibleIndex="15" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn vị quản lý" FieldName="dvquanly" VisibleIndex="16" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn vị sử dụng" FieldName="dvsudung" VisibleIndex="17" Visible="False">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Styles>
                        <AlternatingRow BackColor="#FFFFCC"></AlternatingRow>
                        <SelectedRow BackColor="Transparent" ForeColor="#3333ff" Font-Bold="true"></SelectedRow>
                    </Styles>
                    <SettingsPager PageSize="20">
                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                    </SettingsPager>
                    <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="Control" />
                    <Settings ShowFooter="True" HorizontalScrollBarMode="Auto" ShowGroupPanel="True" UseFixedTableLayout="True" />
                    <SettingsCookies Enabled="False" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                </dx:ASPxGridView>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
