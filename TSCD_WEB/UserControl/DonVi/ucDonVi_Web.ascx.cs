﻿using SHARED.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TSCD_WEB.UserControl.DonVi
{
    public partial class ucDonVi_Web : System.Web.UI.UserControl
    {
        List<TSCD.Entities.DonVi> listDonVi = new List<TSCD.Entities.DonVi>();
        TSCD.Entities.DonVi objDonVi = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ucTreeViTri.Label_TenViTri.Text = "Đơn vị";
            }
        }
        public void LoadData()
        {
            listDonVi = TSCD.Entities.DonVi.getAll();

            if (listDonVi.Count > 0)
            {
                infotr.Visible = true;
                ucTreeViTri.CreateTreeList();
                ucTreeViTri.ASPxTreeList_ViTri.DataSource = listDonVi;
                ucTreeViTri.ASPxTreeList_ViTri.DataBind();
                SearchFunction();
                ClearData();
                if (Request.QueryString["key"] != null)
                {
                    infotd.Visible = true;
                    string key = "";
                    try
                    {
                        key = Request.QueryString["key"].ToString();
                    }
                    catch
                    {
                        Response.Redirect(Request.Url.AbsolutePath);
                    }
                    DevExpress.Web.ASPxTreeList.TreeListNode node = ucTreeViTri.ASPxTreeList_ViTri.FindNodeByKeyValue(key);
                    if (node != null)
                    {
                        ucTreeViTri.FocusAndExpandToNode(node);
                        LoadFocusedNodeData();
                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsolutePath);
                    }
                }
                else
                {
                    ChuaChonDonVi.Visible = true;
                    DevExpress.Web.ASPxTreeList.TreeListNode node = ucTreeViTri.ASPxTreeList_ViTri.FindNodeByKeyValue("");
                    node.Focus();
                    ucWarning_ChuaChonDonVi.LabelInfo.Text = "Chưa chọn đơn vị";
                }
            }
            else
            {
                KhongCoDuLieu.Visible = true;
                ucDanger_KhongCoDuLieu.LabelInfo.Text = "Chưa có đơn vị";
            }
        }

        private void ClearData()
        {
            Label_ThongTin.Text = "Thông tin";
            Label_Ten.Text = "";
            Label_Loai.Text = "";
            Label_Ma.Text = "";
            Label_Thuoc.Text = "";
            Label_MoTa.Text = "";
        }
        private void LoadDataObj(Guid id)
        {
            objDonVi = TSCD.Entities.DonVi.getById(id);
            if (objDonVi != null)
            {
                Label_ThongTin.Text = string.Format("Thông tin {0}", objDonVi.ten);
                Label_Ma.Text = objDonVi.subId;
                ucDonVi_BreadCrumb.Label_TenDonVi.Text = Label_Ten.Text = objDonVi.ten;
                Label_Loai.Text = objDonVi.loaidonvi.ten;
                Label_Thuoc.Text = objDonVi.parent == null ? "[Đại học Sài Gòn]" : objDonVi.parent.ten;
                Label_MoTa.Text = StringHelper.ConvertRNToBR(objDonVi.mota);
            }
            else
            {
                Response.Redirect(Request.Url.AbsolutePath);
            }
        }

        private void LoadFocusedNodeData()
        {
            if (listDonVi.Count > 0)
            {
                if (ucTreeViTri.ASPxTreeList_ViTri.FocusedNode != null && GUID.From(ucTreeViTri.ASPxTreeList_ViTri.FocusedNode.GetValue("id")) != Guid.Empty)
                {
                    LoadDataObj(GUID.From(ucTreeViTri.ASPxTreeList_ViTri.FocusedNode.GetValue("id")));
                }
            }
        }

        private void SearchFunction()
        {
            if (Request.QueryString["Search"] != null)
            {
                Guid SearchID = Guid.Empty;
                try
                {
                    SearchID = GUID.From(Request.QueryString["Search"]);
                }
                catch
                {
                    Response.Redirect(Request.Url.AbsolutePath);
                }
                DevExpress.Web.ASPxTreeList.TreeListNode node = ucTreeViTri.ASPxTreeList_ViTri.GetAllNodes().Where(item => Object.Equals(item.GetValue("id").ToString(), SearchID.ToString())).FirstOrDefault();
                if (node != null)
                {
                    Response.Redirect(string.Format("{0}?key={1}", Request.Url.AbsolutePath, node.Key.ToString()));
                }
                else
                {
                    Response.Redirect(Request.Url.AbsolutePath);
                }
            }
            else
            {
                return;
            }
        }
    }
}