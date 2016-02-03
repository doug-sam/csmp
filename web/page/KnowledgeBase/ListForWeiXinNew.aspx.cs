using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Data;

public partial class page_KnowledgeBase_ListForWeiXinNew : System.Web.UI.Page
{
    public string content = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            if (!IsPostBack)
            {
                string openid = Request["openid"];
                if (string.IsNullOrEmpty(openid))
                {
                    HttpContext.Current.Response.Redirect("/Logout.aspx");
                    return;
                }
            }
        }
    }

}
