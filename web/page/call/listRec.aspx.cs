using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_call_listRec : _Call_list
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int StoreID = Function.GetRequestInt("ID");
        if (StoreID <= 0)
        {
            Response.End(); return;
        }
        ListRec1.StoreID = StoreID;
    }
}
