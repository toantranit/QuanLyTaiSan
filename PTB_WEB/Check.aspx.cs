﻿using PTB.Entities;
using System;
using SHARED.Libraries;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PTB_WEB
{
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["TextBoxTaiKhoan"]))
            {
                string TextBoxTaiKhoan = Request["TextBoxTaiKhoan"].Trim();
                if (!String.IsNullOrEmpty(Request["UserName"]))
                {
                    if (Request["UserName"] == TextBoxTaiKhoan)
                    {
                        Response.Write("-1");
                    }
                    else
                    {
                        if (QuanTriVien.isUsernameExist(TextBoxTaiKhoan))
                            Response.Write("1");
                        else Response.Write("-1");
                    }
                }
                else
                {
                    if (QuanTriVien.isUsernameExist(TextBoxTaiKhoan))
                        Response.Write("1");
                    else Response.Write("-1");
                }
            }
        }
    }
}