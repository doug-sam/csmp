using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Data;
using System.IO;


public partial class page_DropInMemo_list : _Call_Step
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int StepID=Function.GetRequestInt("StepID");
            GridView1.DataSource = DropInMemoBLL.GetList(StepID);
            GridView1.DataBind();
        }
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        //int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        //HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");
        //if (CallStepBLL.GetReview(ID) != null)
        //    HyperLink1.Text = "已回访";
        //HyperLink1.NavigateUrl = "javascript:tb_show('回访', '/page/call/Review.aspx?ID=" + ID + "&TB_iframe=true&height=450&width=730', false);";
        
    }


    protected void LbDel_Click(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(((LinkButton)sender).CommandArgument);
        DropInMemoBLL.Delete(ID);
        Function.Refresh();

    }
}
