using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_CustomerRequest_Edit : _CustomerRequest
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BtnDisable.Visible = BtnSubmit.Visible = GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.编辑请求);

            CustomerRequestInfo info = GetInfo();
            if (null == info)
            {
                Response.End();
                return;
            }
            if (!info.Enable||info.CallID>0)
            {
                BtnSubmit.Visible = false;
                BtnDisable.Visible = false;
            }

            LabStoreMsg.Text = string.Format("店铺名：{0}；店铺编号：{1}；", info.StoreName, info.StoreNo);
            TxbDetail.Text = info.Details;
            LabErrorReportUser.Text = info.ErrorReportUserName;
        }
    }

    private CustomerRequestInfo GetInfo()
    {
        CustomerRequestInfo info;
        if (ViewState["info"] != null)
        {
            info = (CustomerRequestInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = CustomerRequestBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {

        CustomerRequestInfo crinfo = GetInfo();
        crinfo.Details = TxbDetail.Text.Trim();
        if (string.IsNullOrEmpty(crinfo.Details))
        {
            Function.AlertMsg("请填写点报修内容");
            return;
        }
        if (CustomerRequestBLL.Edit(crinfo))
        {
            Function.AlertRefresh("编辑成功", "main");
            return;
        }
        else
        {
            Function.AlertMsg("编辑失败，请重试");
            return;
        }


    }

    protected void BtnDisable_Click(object sender, EventArgs e)
    {
        CustomerRequestInfo crinfo = GetInfo();
        crinfo.Enable = false;
        if (CustomerRequestBLL.Edit(crinfo))
        {
            Function.AlertRefresh("取消成功", "main");
            return;
        }
        else
        {
            Function.AlertMsg("取消失败，请重试");
            return;
        }
    }
}
