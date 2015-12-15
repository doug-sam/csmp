using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class APPService_AppMessageList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo CurrentUser = UserBLL.GetCurrent();

        if (null == CurrentUser)
        {
            return;
        }
        GridView1.DataSource = MarqueeMessageBLL.GetByMaintaimUserID(CurrentUser.ID); 
        GridView1.DataBind();

    }
}
