﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPhongThietBi_Mobile.ascx.cs" Inherits="PTB_WEB.UserControl.PhongThietBi.ucPhongThietBi_Mobile" %>

<%@ Register Src="~/UserControl/ucASPxImageSlider_Mobile.ascx" TagPrefix="uc" TagName="ucASPxImageSlider_Mobile" %>
<%@ Register Src="~/UserControl/ucCollectionPager.ascx" TagPrefix="uc" TagName="ucCollectionPager" %>
<%@ Register Src="~/UserControl/ucTreeViTri.ascx" TagPrefix="uc" TagName="ucTreeViTri" %>
<%@ Register Src="~/UserControl/PhongThietBi/ucPhongThietBi_BreadCrumb.ascx" TagPrefix="uc" TagName="ucPhongThietBi_BreadCrumb" %>
<%@ Register Src="~/UserControl/ucThongBaoLoi.ascx" TagPrefix="uc" TagName="ucThongBaoLoi" %>

<uc:ucPhongThietBi_BreadCrumb runat="server" ID="ucPhongThietBi_BreadCrumb" />
<uc:ucThongBaoLoi runat="server" ID="ucThongBaoLoi" />

<asp:Panel ID="Panel_TreeListViTri" runat="server" Visible="false">
    <table class="table largetable">
        <tbody>
            <tr>
                <td>
                    <uc:ucTreeViTri runat="server" ID="_ucTreeViTri" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Panel>

<asp:Panel ID="Panel_DanhSachThietBi" runat="server" Visible="false">
    <table class="table largetable">
        <tbody>
            <tr>
                <td>
                    <% if (RepeaterDanhSachThietBi.Items.Count == 0)
                       { %>
                    <div class="alert alert-warning alert-dismissible" role="alert">
                        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <asp:Label ID="Label_DanhSachThietBi" runat="server"></asp:Label>
                    </div>
                    <% }
                       else
                       { %>
                    <h3 class="title_blue fix">Danh sách thiết bị</h3>
                    <table class="table table-bordered table-striped table-hover">
                        <thead class="centered">
                            <tr>
                                <th>#</th>
                                <th>Tên</th>
                                <th>Tình trạng</th>
                                <th>Số lượng</th>
                                <th>Xem Log</th>
                            </tr>
                        </thead>
                        <tbody class="centered">
                            <asp:Repeater ID="RepeaterDanhSachThietBi" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td onclick="location.href='<%# Eval("url") %>'" style="cursor: pointer"><%# Container.ItemIndex + 1 + ((_ucCollectionPager_DanhSachThietBi.CollectionPager_Object.CurrentPage - 1)*_ucCollectionPager_DanhSachThietBi.CollectionPager_Object.PageSize) %></td>
                                        <td onclick="location.href='<%# Eval("url") %>'" style="cursor: pointer"><%# Eval("ten") %></td>
                                        <td onclick="location.href='<%# Eval("url") %>'" style="cursor: pointer"><%# Eval("tinhtrang") %></td>
                                        <td onclick="location.href='<%# Eval("url") %>'" style="cursor: pointer"><%# Eval("soluong") %></td>

                                        <td class="tdcenter">
                                            <button class="btn btn-default" onclick="location.href='<%# Eval("urlLog") %>'; return false;"><span class="glyphicon glyphicon-tasks"></span></button>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <uc:ucCollectionPager runat="server" ID="_ucCollectionPager_DanhSachThietBi" />
                    <% } %>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Panel>

<asp:Panel ID="Panel_ThietBi" runat="server" Visible="false">
    <table class="table largetable">
        <tbody>
            <tr>
                <td>
                    <h3 class="title_green fix">
                        <asp:Label ID="Label_ThongTinThietBi" runat="server" Text="Thông tin thiết bị"></asp:Label>
                    </h3>
                    <uc:ucASPxImageSlider_Mobile runat="server" ID="_ucASPxImageSlider_Mobile" />
                    <table class="table table-striped">
                        <tbody>
                            <tr>
                                <td style="width: 120px">Mã thiết bị:</td>
                                <td>
                                    <asp:Label ID="Label_MaThietBi" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Tên thiết bị:</td>
                                <td>
                                    <asp:Label ID="Label_TenThietBi" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Loại thiết bị:</td>
                                <td>
                                    <asp:Label ID="Label_LoaiThietBi" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Kiểu quản lý:</td>
                                <td>
                                    <asp:Label ID="Label_KieuQuanLy" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Thuộc phòng:</td>
                                <td>
                                    <asp:Label ID="Label_Phong" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <asp:Panel ID="Panel_NgayMua" runat="server" Visible="False">
                                <tr>
                                    <td>Ngày mua:</td>
                                    <td>
                                        <asp:Label ID="Label_NgayMua" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td>Ngày lắp:</td>
                                <td>
                                    <asp:Label ID="Label_NgayLap" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Mô tả:</td>
                                <td>
                                    <asp:Label ID="Label_MoTa" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdcenter">
                                    <asp:Button ID="Button_XemLog" runat="server" Text="Xem log" CssClass="btn btn-default" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
