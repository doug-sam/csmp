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

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_SpanTime_list : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void BindData(int CallID)
    {
        GridView1.DataSource = SpanTimeBLL.GetList(CallID);
        GridView1.DataBind();

    }
    public void BtnDelete_Hide()
    {
        BtnDelete.Visible = false;
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        int Flag = 0;
        foreach (string item in delList.Split(','))
        {
            int ID = Function.ConverToInt(item);
            if (ID > 0)
            {
                if (SpanTimeBLL.Delete(ID))
                {
                    Flag++;
                }
            }
        }
        Function.AlertRefresh(Flag + "条数据删除成功");
    }
}
