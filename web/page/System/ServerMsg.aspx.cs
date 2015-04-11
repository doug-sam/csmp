using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;


public partial class system_ServerMsg : _Sys_ServerMsg
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txbTitle.Text = OtherBLL.Get("WebTitle").Content;
            //Txbkeywords.Text = OtherBLL.Get("Keywords").Content;
            //TxbDescription.Text = OtherBLL.Get("Description").Content;
        }
    }


}
