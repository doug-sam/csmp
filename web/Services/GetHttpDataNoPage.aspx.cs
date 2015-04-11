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
        Response.Write(DataHelper.GetHttpData("http://10.0.29.241:8012/?outbound=6002&ext=6001"));
    }
}