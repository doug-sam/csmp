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

using CSMP.Model;
using Tool;
public partial class page_Group_ItemDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Function.GetRequestInt("ID");
        if (ID>0)
        {
            HlItem1.NavigateUrl = "itemcall.aspx?ID=" + ID;
            HlItem2.NavigateUrl = "itemcall2.aspx?ID="+ID;
        }
    }
}
