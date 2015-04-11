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
using System.Collections.Generic;
using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Trace_listTrace : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<CallInfo> list = CallBLL.GetListTraceByCurrentUser();
        GridView1.DataSource = list;
        GridView1.DataBind();

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;


        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        CallStepInfo sinfo = CallStepBLL.GetLast(ID, SysEnum.StepType.店铺催促);
        if (null == sinfo)
        {
            return;
        }
        
        Label LabTraceDate = (Label)e.Row.FindControl("LabTraceDate");
        Label LabMajorUserName = (Label)e.Row.FindControl("LabMajorUserName");
        Label LabDetails = (Label)e.Row.FindControl("LabDetails");
        LabTraceDate.Text = sinfo.AddDate.ToString("yyyy-MM-dd HH:mm:ss");
        LabMajorUserName.Text = sinfo.MajorUserName;
        LabDetails.Text = sinfo.Details;
    }

}
