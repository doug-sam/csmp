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


public partial class page_Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            if (CurrentUser==null)
            {
                Response.Redirect("/login.aspx");
            }
            //if (CurrentUser.Rule==SysEnum.Rule.二线.ToString()||CurrentUser.Rule==SysEnum.Rule.一线.ToString()||CurrentUser.Rule==SysEnum.Rule.现场工程师.ToString())
            //{
            //    Response.Redirect("/page/call/list.aspx?state="+(int)SysEnum.CallStateMain.未处理);
            //}
            if (CurrentUser.Rule.Contains(SysEnum.Rule.客户.ToString()))
            {
                Response.Redirect("/page/call/sch.aspx");
            }
        }
    }
}
