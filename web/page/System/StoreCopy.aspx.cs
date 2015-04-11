using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_System_StoreCopy : AdminPage
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
        if (0 == CustomerID)
        {
            DdlCustomer.DataBind();
            DdlTargetCustomer.DataSource = new object();
            DdlTargetCustomer.DataBind();
        }
        else
        {

            CbListBrand.DataSource = BrandBLL.GetList(CustomerID);
            CbListBrand.DataBind();

            List<CustomersInfo> listinfo = CustomersBLL.GetList();
            foreach (CustomersInfo item in listinfo)
            {
                if (item.ID == CustomerID)
                {
                    listinfo.Remove(item);
                    break;
                }
            }
            DdlTargetCustomer.DataSource = listinfo;
            DdlTargetCustomer.DataBind();
        }

    }
    private void Output(string Value)
    {
        Response.Write(DateTime.Now+"__"+Value+"<br/>");
        Response.Flush();

    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        int count = 0;
        int PageSize = 999999999;
        Dictionary<int, int> DicClass1 = new Dictionary<int, int>();
        Dictionary<int, int> DicClass2 = new Dictionary<int, int>();
        Dictionary<int, int> DicClass3 = new Dictionary<int, int>();
        Dictionary<int, int> Dicsln = new Dictionary<int, int>();
        int OldCustomerID = Function.ConverToInt(DdlCustomer.SelectedValue);
        CustomersInfo OldCustomerInfo = CustomersBLL.Get(OldCustomerID);
        int TargetCustomerID = Function.ConverToInt(DdlTargetCustomer.SelectedValue);
        CustomersInfo targetCustomerInfo = CustomersBLL.Get(TargetCustomerID);
        if (null == targetCustomerInfo)
        {
            Function.AlertMsg("TargetCustomer not found");
            return;
        }
        string BrandSQL = GetBrandSQL();


        List<CallInfo> callAlllist = CallBLL.GetList(BrandSQL);

        List<Class1Info> list1 = Class1BLL.GetList(PageSize, 1, " f_CustomerID=" + OldCustomerInfo.ID, out count);



        foreach (Class1Info info1 in list1)
        {
            info1.CustomerID = targetCustomerInfo.ID;
            info1.IsClosed = true;
            int NewClass1ID = Class1BLL.Add(info1);
            DicClass1.Add(info1.ID, NewClass1ID);
            Output("大类故障" + info1.Name + "添加完成，新ID：" + NewClass1ID);
            List<Class2Info> list2 = Class2BLL.GetList(PageSize, 1, " f_Class1ID=" + info1.ID, out count);
            foreach (Class2Info info2 in list2)
            {
                info2.Class1ID = NewClass1ID;
                info2.IsClosed = true;
                int NewClass2ID = Class2BLL.Add(info2);
                DicClass2.Add(info2.ID, NewClass2ID);
                Output("__中类故障" + info2.Name + "添加完成，新ID：" + NewClass2ID);
                List<Class3Info> list3 = Class3BLL.GetList(PageSize, 1," f_Class2ID="+ info2.ID,out count);
                foreach (Class3Info info3 in list3)
                {
                    info3.Class2ID = NewClass2ID;
                    info3.IsClosed = true;
                    int NewClass3ID = Class3BLL.Add(info3);
                    DicClass3.Add(info3.ID, NewClass3ID);
                    Output("____小类故障" + info3.Name + "添加完成，新ID：" + NewClass3ID);
                    List<SolutionInfo> listSLN = SolutionBLL.GetList(info3.ID,99999);
                    foreach (SolutionInfo SLNinfo in listSLN)
                    {
                        #region 解决方案
                        SLNinfo.Class1 = NewClass1ID;
                        SLNinfo.Class2 = NewClass2ID;
                        SLNinfo.Class3 = NewClass3ID;
                        SLNinfo.EnableFlag = true;
                        SLNinfo.SolveCount = 0;
                        SLNinfo.SuggestCount = 0;
                        int NewSLNID = SolutionBLL.Add(SLNinfo);
                        Dicsln.Add(SLNinfo.ID, NewSLNID);
                        Output("______解决方案" + SLNinfo.Name + "添加完成，新ID：" + NewSLNID);
                        #endregion
                    }
                }
            }
        }
        foreach (CallInfo item in callAlllist)
        {
            item.CustomerID = targetCustomerInfo.ID;
            item.CustomerName = targetCustomerInfo.Name;
            item.Class1 = DicClass1[item.Class1];
            item.Class2 = DicClass2[item.Class2];
            item.Class3 = DicClass3[item.Class3];
            if (0 != item.SlnID) item.SlnID = Dicsln[item.SlnID];
            CallBLL.Edit(item);
            Output("Call" + item.No + "|" + item.StoreName + "|" + "更新完成");
        }
        foreach (ListItem item in CbListBrand.Items)
        {
            if (!item.Selected)
            {
                continue;
            }
            BrandInfo binfo = BrandBLL.Get(Function.ConverToInt(item.Value));
            binfo.CustomerID = targetCustomerInfo.ID;
            BrandBLL.Edit(binfo);
            Output("品牌" + binfo.Name + "更新完成");
            List<StoreInfo> listStore = StoresBLL.GetList(9999999, 1, " f_BrandID=" + item.Value, out count);

            foreach (StoreInfo StoreItem in listStore)
            {
                StoreItem.CustomerID = targetCustomerInfo.ID;
                StoreItem.CustomerName = targetCustomerInfo.Name;
                StoresBLL.Edit(StoreItem);
                Output("店铺" + StoreItem.Name + "所属客户更新完成");
            }

        }

        Output("全部执行完成！！！！！！！");
        
    }

    private string GetBrandSQL()
    {
        string BrandSQL = "1=1 and f_brandID in(";
        foreach (ListItem item in CbListBrand.Items)
        {
            if (!item.Selected)
            {
                continue;
            }
            BrandSQL += item.Value + ",";
        }
        BrandSQL = BrandSQL.Substring(0,BrandSQL.Length - 1);
        BrandSQL += ")";
        return BrandSQL;
    }
}