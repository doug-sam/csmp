using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_System_ClassCopy :AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (0==CustomerID)
        {
            DdlClass1.DataSource = new object();
            DdlCustomer.DataBind();
            DdlTargetCustomer.DataSource = new object();
            DdlTargetCustomer.DataBind();
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(CustomerID);
            DdlClass1.DataBind();

            List<CustomersInfo> listinfo = CustomersBLL.GetList();
            foreach (CustomersInfo item in listinfo)
            {
                if (item.ID==CustomerID)
                {
                    listinfo.Remove(item);
                    break;
                }
            }
            DdlTargetCustomer.DataSource = listinfo;
            DdlTargetCustomer.DataBind();
        }

    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        int Class1ID = Function.ConverToInt(DdlClass1.SelectedValue);
        Class1Info info = Class1BLL.Get(Class1ID);
        if (null==info)
        {
            Function.AlertMsg("class1 not found");
            return;
        }

        int TargetCustomerID = Function.ConverToInt(DdlTargetCustomer.SelectedValue);
        CustomersInfo targetCustomerInfo = CustomersBLL.Get(TargetCustomerID);
        if (null==targetCustomerInfo)
        {
            Function.AlertMsg("TargetCustomer not found");
            return;   
        }

        info.CustomerID = targetCustomerInfo.ID;
        info.IsClosed = CbNewClassEnable.Checked;
        int NewClass1ID = Class1BLL.Add(info);

        List<Class2Info> list2 = Class2BLL.GetList(info.ID);        
        foreach (Class2Info item in list2)
        {
            item.Class1ID = NewClass1ID;
            item.IsClosed = CbNewClassEnable.Checked;
            int NewClass2ID = Class2BLL.Add(item);
            List<Class3Info> list3 = Class3BLL.GetList(item.ID);
            foreach (Class3Info info3 in list3)
            {
                info3.Class2ID = NewClass2ID;
                info3.IsClosed = CbNewClassEnable.Checked;
                int NewClass3ID = Class3BLL.Add(info3);
                List<SolutionInfo> listSLN = SolutionBLL.GetList(info3.ID);
                foreach (SolutionInfo SLNinfo in listSLN)
                {
                    SLNinfo.Class1 = NewClass1ID;
                    SLNinfo.Class2 = NewClass2ID;
                    SLNinfo.Class3 = NewClass3ID;
                    SLNinfo.EnableFlag = !CbNewClassEnable.Checked;
                    SLNinfo.SolveCount = 0;
                    SLNinfo.SuggestCount = 0;
                    SolutionBLL.Add(SLNinfo);
                }

               

            }

        }

    }
}