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

using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections.Generic;

public partial class page_System_InitClass : _Sys_Profile
{
    public const string xianm项目 = "项目";
    public static List<PrioritiesInfo> PriorInfo = PrioritiesBLL.GetList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (PriorInfo==null||PriorInfo.Count==0)
            {
                Response.Write("没有优先级<br/>");
                return;
            }

            List<CustomersInfo> listcustomer = new List<CustomersInfo>();

            int CustomerID = Function.GetRequestInt("CustomerID");
            CustomersInfo cinfo = CustomersBLL.Get(CustomerID);
            if (null!=cinfo)
            {
                listcustomer.Add(cinfo);
            }
            else
            {
                listcustomer = CustomersBLL.GetList();
            }


            foreach (CustomersInfo Citem in listcustomer)
            {
                InitClass1(Citem);
            }
            Response.Write("执行完成<br/>");
        }
    }

    private void InitClass1(CustomersInfo Citem)
    {
        bool Flag = false;

        List<Class1Info> listClass1 = Class1BLL.GetList(Citem.ID);
        foreach (Class1Info itemC1 in listClass1)
        {
            if (itemC1.Name == xianm项目)
            {
                Flag = true;
                break;
            }
        }
        if (!Flag)
        {
            Class1Info c1info = new Class1Info();
            c1info.CustomerID = Citem.ID;
            c1info.IsClosed = false;
            c1info.Name = xianm项目;
            c1info.ID = Class1BLL.Add(c1info);
            if (c1info.ID>0)
            {
                Response.Write(string.Format("生成了   {0}   的项目<br/>",Citem.Name));
            }
            InitClass2(c1info);
        }
        else
        {
            Response.Write(string.Format("-_-  -_-  -_-  已存在   {0}   的项目-_-  -_-  -_-  -_-  <br/>", Citem.Name));
        }
    }

    private void InitClass2(Class1Info c1info)
    {
        bool Flag = false;
        List<Class2Info> listClass2 = new List<Class2Info>();
        foreach (Class2Info item in listClass2)
        {
            if (item.Name == xianm项目)
            {
                Flag = true;
                break;
            }
        }
        if (!Flag)
        {
            Class2Info c2info = new Class2Info();
            c2info.Class1ID = c1info.ID;
            c2info.Class1Name = xianm项目;
            c2info.IsClosed = false;
            c2info.Name = xianm项目;
            c2info.ID = Class2BLL.Add(c2info);
            InitClass3(c2info);
        }
    }

    private void InitClass3(Class2Info c2info)
    {
        bool Flag = false;
        List<Class3Info> listClass3 = new List<Class3Info>();
        foreach (Class3Info item in listClass3)
        {
            if (item.Name == xianm项目)
            {
                Flag = true;
                break;
            }
        }
        if (!Flag)
        {
            Class3Info c3info = new Class3Info();
            c3info.Class2ID = c2info.ID;
            c3info.Class2Name = xianm项目;
            c3info.IsClosed = false;
            c3info.Name = xianm项目;
            c3info.PriorityID = PriorInfo[0].ID;
            c3info.PriorityName = PriorInfo[0].Name;
            c3info.SLA = 72;
            Class3BLL.Add(c3info);
        }
    }


}
