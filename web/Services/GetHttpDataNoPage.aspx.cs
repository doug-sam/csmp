using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tool;


public partial class Services_GetHttpDataNoPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url;
        if (!string.IsNullOrEmpty(Request.QueryString["url"]))
            url = Request.QueryString["url"];
        else
        {
            Response.Write("url parameter lost");
            return;
        }

        Response.Write(DataHelper.GetHttpData(url));
    }
}