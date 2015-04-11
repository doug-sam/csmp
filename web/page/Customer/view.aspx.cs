using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Customer_view : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            Response.End();
        }
        CustomersInfo info = CustomersBLL.Get(ID);
        if (null == info)
        {
            Response.End();
        }
        ViewState["info"] = info;
        if (!IsPostBack)
        {
            CityInfo cinfo = CityBLL.Get(info.CityID);
            if (null==cinfo)
            {
                Response.End(); return;
            }
            LabCity.Text = cinfo.Name;
        }
    }
}
