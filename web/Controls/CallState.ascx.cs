using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class Controls_CallState : System.Web.UI.UserControl
{
    public string FocusItemIndex;
    public int CallID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "FocusState("+FocusItemIndex+");", true);
        }
        if (CallID > 0)
        {
            CallInfo info = CallBLL.Get(CallID);
            if (null == info) return;
            LabListRec.Text = "<a href=\"javascript:tb_show('报修记录', '/page/call/listRec.aspx?ID=" + info.StoreID;
            LabListRec.Text += "&UnCallID=" + info.ID + "&TB_iframe=true&height=450&width=730', false);\">报修历史</a>";
            LabListRec.Visible = true;

            if (info.StateMain == (int)SysEnum.CallStateMain.处理中)
            {
                List<CallStepInfo> list = CallStepBLL.GetListJoin(info);
                for (int i = 0; i < list.Count; i++)
                {
                    LabStep.Text += string.Format("<div>{0}、({1}){2}</div>", (i + 1), list[i].DateBegin.ToString("yyyy-MM-dd HH:mm"), list[i].StepName);
                }
                Panel1.Visible = true;
            }
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                LabSln.Text += info.SuggestSlnName;
                tr_sln.Visible = !string.IsNullOrEmpty(info.SuggestSlnName);

            }
            View1.BindData(info);
            PanelIrameSrc.Visible = true;

        }
    }
}
