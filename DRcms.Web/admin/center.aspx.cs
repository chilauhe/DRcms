﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DRcms.Common;

namespace DRcms.Web.admin
{
    public partial class center : Web.UI.ManagePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Model.manager admin_info = GetAdminInfo(); //管理员信息
                //登录信息
                if (admin_info != null)
                {
                    BLL.manager_log bll = new BLL.manager_log();
                    Model.manager_log model1 = bll.GetModel(admin_info.user_name, 1, Enums.ActionEnum.Login.ToString());
                    if (model1 != null)
                    {
                        //本次登录
                        litIP.Text = model1.user_ip;
                    }
                    Model.manager_log model2 = bll.GetModel(admin_info.user_name, 2, Enums.ActionEnum.Login.ToString());
                    if (model2 != null)
                    {
                        //上一次登录
                        litBackIP.Text = model2.user_ip;
                        litBackTime.Text = model2.add_time.ToString();
                    }
                }
                LitUpgrade.Text = FileHelper.GetDomainStr(Constants.CACHE_OFFICIAL_UPGRADE, DESEncrypt.Decrypt(Constants.FILE_URL_UPGRADE_CODE, "DT"));
                LitNotice.Text = FileHelper.GetDomainStr(Constants.CACHE_OFFICIAL_NOTICE, DESEncrypt.Decrypt(Constants.FILE_URL_NOTICE_CODE, "DT"));
            }
        }
    }
}