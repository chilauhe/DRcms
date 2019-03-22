using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DRcms.Common;

namespace DRcms.Web.admin
{
    public partial class index : Web.UI.ManagePage
    {
        protected Model.manager admin_info;//管理员信息

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                admin_info = GetAdminInfo();
            }
        }

        //安全退出
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            Session[Constants.SESSION_ADMIN_INFO] = null;
            Utils.WriteCookie("AdminName", "DRcms", -14400);
            Utils.WriteCookie("AdminPwd", "DRcms", -14400);
            Response.Redirect("login.aspx");
        }
    }
}