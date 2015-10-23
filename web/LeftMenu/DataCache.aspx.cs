using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class LeftMenu_DataCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnGetFromData_Click(object sender, EventArgs e)
    {
        
        LeftMenuDataBLL.InsertLeftMenuDataCache();
        UserInfo currentUser = UserBLL.Get(845);

        LeftMenuData data = LeftMenuDataBLL.GetExpressInfoCacheByName(currentUser.ID); 
        tboxResult.Text = data.ToBeOnSite.ToString() ;

    }
}
