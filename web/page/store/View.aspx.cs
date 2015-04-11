using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Store_View : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            StoreInfo info= GetInfo();
            if (null==info)
            {
                Response.End();
            }
            HlHistory.NavigateUrl =string.Format("/page/call/sch.aspx?StoreNo={0}",info.No);
        }
    }

    private StoreInfo GetInfo()
    {
        StoreInfo info;
        if (ViewState["info"] != null)
        {
            info = (StoreInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = StoresBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void LbTop_Click(object sender, EventArgs e)
    {
        StoreInfo sinfo= GetInfo();
        PanelHistory.Visible = true;
        int count =0;
        GridView1.DataSource = CallBLL.GetList(3, 1, " f_StoreID=" + sinfo.ID+" order by ID desc ", out count);
        GridView1.DataBind();
        LabHistoryNoRecord.Visible = count == 0;
    }

}
