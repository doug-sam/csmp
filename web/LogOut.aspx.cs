using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using Tool;

public partial class LogOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string gotoURL = string.Empty;
        
        if (Session["lvlogin"]!=null)
        {
            gotoURL = "<script>top.location.href='/lvdefault.html';</script>";
        }
        else {
            gotoURL = "<script>top.location.href='/login.aspx';</script>";
        
        }
        UserBLL.Logout();
         
        Response.Write(gotoURL);
        Response.End();
        
        
        
    }
}
