using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class Controls_View : System.Web.UI.UserControl
{
   public CallInfo info;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (null!=ViewState["controlInfo"])
        {
            info = (CallInfo)ViewState["controlInfo"];
        }
        if (null==info)
        {
            return;
            //Response.End();
        }
        tr_Edit.Visible = CallBLL.EnableEdit(info, UserBLL.GetCurrent());
    }




    public void BindData(CallInfo info)
    {
        if (null == info)
        {
            return ;
            //Response.End();
        }
        ViewState["controlInfo"] = info;
        StoreInfo sinfo = StoresBLL.Get(info.StoreID);
        if (null == sinfo)
        {
            return;
        }
        LtlTel.Text = sinfo.Tel;
        LtlAddress.Text = sinfo.Address;
        Page_Load(null, null);
    }
}
