using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class LeftMenu_DataCacheStartService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LeftMenuDataBLL.InsertLeftMenuDataCache();

    }
}
