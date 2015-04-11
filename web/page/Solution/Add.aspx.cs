using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Solution_Add : _BaseData_Solution
{

    private const string CookiesName = "solution";
    private const string CookiesKeyCustomer = "sln_Customer";
    private const string CookiesKeyc1 = "sln_c1";
    private const string CookiesKeyc2 = "sln_c2";
    private const string CookiesKeyc3 = "sln_c3";
    private const string CookiesKeyCount = "sln_count";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));

            //CookiesHelper.SetCookie(
            int CustomerID = GetCookiesVal(CookiesKeyCustomer);
           if (CustomerID>0)
           {
               DdlCustomer.SelectedValue = CustomerID.ToString();
               DdlCustomer_SelectedIndexChanged(null, null);
           }
           TxbCount.Text = GetCookiesVal(CookiesKeyCount).ToString();
        }
    }

    private int GetCookiesVal(string Key)
    {
        return Function.ConverToInt(CookiesHelper.GetCookieValue(CookiesName, Key), 0);
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
        SolutionInfo info = new SolutionInfo();

        CustomersInfo cinfo = CustomersBLL.Get(Function.ConverToInt(DdlCustomer.SelectedValue));
        if (null==cinfo)
	    {
		 Function.AlertMsg("请选择客户");return;
	    }
        Class1Info c1info = Class1BLL.Get(Function.ConverToInt(DdlClass1.SelectedValue));
        if (null == c1info)
        {
            Function.AlertMsg("请选择大类"); return;
        }
        Class2Info c2info = Class2BLL.Get(Function.ConverToInt(DdlClass2.SelectedValue));
        if (null == c2info)
        {
            Function.AlertMsg("请选择中类"); return;
        }
        Class3Info c3info = Class3BLL.Get(Function.ConverToInt(DdlClass3.SelectedValue));
        if (null == c3info)
        {
            Function.AlertMsg("请选择小类"); return;
        }

        info.Class1 = c1info.ID;
        info.Class1Name = c1info.Name;
        info.Class2 = c2info.ID;
        info.Class2Name = c2info.Name;
        info.Class3 = c3info.ID;
        info.Class3Name = c3info.Name;
        info.EnableBy = CurrentUser.Name;
        info.EnableFlag = true;
        info.Name = TxbName.Text.Trim();
        info.SolveCount = Function.ConverToInt(TxbCount.Text.Trim());
        info.SuggestCount = 0;



        if (info.Name.Trim().Length > 200)
        {
            Function.AlertMsg("方案名不应长于200字"); return;
        }
        if (null != SolutionBLL.Get(info.Name, info.Class3))
        {
            Function.AlertMsg("该方案名已存在"); return;
        }
        if (info.SolveCount < 0)
        {
            Function.AlertMsg("解决次数应为正整数"); return;

        }
        if (SolutionBLL.Add(info)>0)
        {
            Function.AlertMsg("添加成功");
            TxbName.Text = string.Empty;
            TxbName.Focus();
            //Function.AlertRefresh("添加成功", "main");
        }
        else
        {
            Function.AlertMsg("添加失败");
        }

    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID=Function.ConverToInt(DdlCustomer.SelectedValue,0);
        if (0==ID)
        {
            DdlClass1.DataSource = null;
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
        }
        DdlClass1.DataBind();
        DdlClass1_SelectedIndexChanged(null, null);


    }
    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue, 0);
        if (0 == ID)
        {
            DdlClass2.DataSource = null;
        }
        else
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
        }
        DdlClass2.DataBind();
        int C1ID = GetCookiesVal(CookiesKeyc1);
        if (C1ID>0&&0!=ID)
        {
            DdlClass1.SelectedValue = C1ID.ToString();
        }
        DdlClass2_SelectedIndexChanged(null, null);
    }
    protected void DdlClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass2.SelectedValue, 0);
        if (0 == ID)
        {
            DdlClass3.DataSource = null;
        }
        else
        {
            DdlClass3.DataSource = Class3BLL.GetList(ID);
        }
        DdlClass3.DataBind();
        int C2ID = GetCookiesVal(CookiesKeyc2);
        if (C2ID > 0 && 0 != ID)
        {
            DdlClass2.SelectedValue = C2ID.ToString();
        }
        int C3ID = GetCookiesVal(CookiesKeyc3);
        if (C3ID > 0 && 0 != ID)
        {
            DdlClass3.SelectedValue = C3ID.ToString();
        }

    }
}
