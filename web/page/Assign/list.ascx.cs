﻿using System;
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
using CSMP.BLL;

public partial class page_Assign_list : System.Web.UI.UserControl
{
    public int CallID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataSource = AssignBLL.GetList(" f_CallID=" + CallID + " AND f_AssignType=0 order by id asc ");
            //GridView1.DataSource = AssignBLL.GetList(CallID);
            GridView1.DataBind();
        }

    }
}
