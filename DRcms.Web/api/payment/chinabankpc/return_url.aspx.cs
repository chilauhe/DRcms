using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DRcms.Common;
using DRcms.API.Payment.chinabankpc;

namespace DRcms.Web.api.payment.chinabankpc
{
    public partial class return_url : System.Web.UI.Page
    {
        protected string v_oid;		// 订单号
        protected string v_pstatus;	// 支付状态码
        //20（支付成功，对使用实时银行卡进行扣款的订单）；
        //30（支付失败，对使用实时银行卡进行扣款的订单）；
        protected string v_pstring;	//支付状态描述
        protected string v_pmode;	//支付银行
        protected string v_amount;	//支付金额
        protected string v_moneytype;	//币种		
        protected string remark1;	// 备注1
        protected string remark2;	// 备注1
        protected string v_md5str;

        protected void Page_Load(object sender, EventArgs e)
        {
            //读取站点配置信息
            Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();
            int site_payment_id = 0; //站点支付方式ID

            v_oid = Common.Request.GetString("v_oid").ToUpper();
            v_pstatus = Common.Request.GetString("v_pstatus");
            v_pstring = Common.Request.GetString("v_pstring");
            v_pmode = Common.Request.GetString("v_pmode");
            v_md5str = Common.Request.GetString("v_md5str");
            v_amount = Common.Request.GetString("v_amount");
            v_moneytype = Common.Request.GetString("v_moneytype");
            remark1 = Common.Request.GetString("remark1");
            remark2 = Common.Request.GetString("remark2");
            if (v_oid.StartsWith("R")) //充值订单
            {
                site_payment_id = new BLL.user_recharge().GetPaymentId(v_oid);
            }
            else if (v_oid.StartsWith("B")) //商品订单
            {
                site_payment_id = new BLL.orders().GetPaymentId(v_oid);
            }
            //找到站点支付方式ID开始验证
            if (site_payment_id > 0)
            {
                // 拼凑加密串
                Config config = new Config(site_payment_id);
                string signtext = v_oid + v_pstatus + v_amount + v_moneytype + config.Key;
                signtext = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(signtext, "md5").ToUpper();
                if (signtext == v_md5str)
                {
                    if (v_pstatus.Equals("20"))
                    {
                        //成功状态
                        Response.Redirect(new Web.UI.BasePage().linkurl("payment", "?action=succeed&order_no=" + v_oid));
                        return;
                    }
                }
            }

            //失败状态
            Response.Redirect(new Web.UI.BasePage().linkurl("payment", "?action=error"));
            return;
        }
    }
}