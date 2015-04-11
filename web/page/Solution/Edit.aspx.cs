using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Solution_Edit : _BaseData_Solution
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SolutionInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("参数有误"); return;
            }
            Class1Info c1info = Class1BLL.Get(info.Class1);
            if (null == c1info)
            {
                Function.AlertBack("数据出错"); return;
            }
            CustomersInfo cinfo = CustomersBLL.Get(c1info.CustomerID);
            if (null == cinfo)
            {
                Function.AlertBack("数据出错"); return;
            }
            LCustomer.Text = cinfo.Name;
            Lc1.Text = info.Class1Name;
            Lc2.Text = info.Class2Name;
            Lc3.Text = info.Class3Name;
            TxbName.Text = info.Name;
            TxbCount.Text = info.SolveCount.ToString();
        }
    }

    private SolutionInfo GetInfo()
    {
        SolutionInfo info;
        if (ViewState["info"] != null)
        {
            info = (SolutionInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = SolutionBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        SolutionInfo info = GetInfo();
        if (TxbName.Text.Trim().Length>200)
        {
            Function.AlertMsg("方案名不应长于200字"); return;
        }
        if (info.Name != TxbName.Text.Trim() && null != SolutionBLL.Get(TxbName.Text.Trim(), info.Class3))
        {
            Function.AlertMsg("该方案名已存在"); return;
        }
        info.Name = TxbName.Text.Trim();
        info.SolveCount = Function.ConverToInt(TxbCount.Text.Trim());
        if (info.SolveCount < 00)
        {
            Function.AlertMsg("解决次数应为正整数"); return;

        }
        if (SolutionBLL.Edit(info))
        {
            Function.AlertRefresh("修改成功", "main");
        }
        else
        {
            Function.AlertMsg("修改失败");
        }

    }
}
