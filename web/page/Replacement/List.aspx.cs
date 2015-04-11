using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Replacement_List : _Call_StepReplacement
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }

            NotMyCallCheck(info);

            List<ReplacementInfo> list = ReplacementBLL.GetList(info.ID);
            GridView1.DataSource = list;
            GridView1.DataBind();
            PanelNoData.Visible = list.Count == 0;
        }
    }

    protected CallInfo GetInfo()
    {
        if (ViewState["info"] != null)
        {
            return (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("id");
        if (ID <= 0)
        {
            return null;
        }
        CallInfo info = CallBLL.Get(ID);
        if (null == info)
        {
            return null;
        }
        ViewState["info"] = info;
        return info;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        ReplacementInfo info = (ReplacementInfo)e.Row.DataItem;
        if (info.StateID==(int)SysEnum.ReplacementStatus.处理完成)
        {
            e.Row.BackColor = System.Drawing.Color.Green;
        }

    }

}
