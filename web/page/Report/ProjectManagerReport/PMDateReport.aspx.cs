﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;
using System.Data;

public partial class page_Report_ProjectManagerReport_PMDateReport : _Report_ReportPMDClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (CurrentUser.Rule[0] == "项目经理")
            {
                PMDataBlind();
                string userID = CurrentUser.ID.ToString();
                this.DdlPM.SelectedValue = userID;
                this.DdlPM.Enabled = false;
                DdlCustomer.DataSource = CustomersBLL.GetCustomerByUserID(userID);
                DdlCustomer.DataBind();

                //绑定品牌
                DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(CurrentUser.WorkGroupID);
                DdlBrand.DataBind();
                DdlBrand.Items.Insert(0, new ListItem("请选择", "0"));

            }
            else
            {
                PMDataBlind();
                DdlCustomer.DataSource = CustomersBLL.GetCustomerByUserID(this.DdlPM.SelectedValue);
                DdlCustomer.DataBind();
                DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
                BrandDataBlind();
            }
            
            TxtDateBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");


        }
    }

    //点击查询
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        AspNetPager1.CurrentPageIndex = 1;
        BindGridView();

    }
    /// <summary>
    /// 绑定品牌列表
    /// </summary>
    protected void BrandDataBlind()
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    /// <summary>
    /// 绑定项目经理
    /// </summary>
    protected void PMDataBlind()
    {
        string sqlPM = "select ID,f_Name as name from sys_User where f_Rule = ',项目经理,'";

        this.DdlPM.DataSource = ReportSourceBLL.GetReport(sqlPM);
        this.DdlPM.DataBind();
    }
    /// <summary>
    /// 项目经理选择改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlPM_SelectedIndexChanged(object sender, EventArgs e)
    {

        string sqlCustomer = "SELECT DISTINCT f_CustomerName as Name,f_CustomerID as ID From sys_Customers, sys_User, sys_WorkGroupBrand,sys_Brand WHERE sys_User.ID = '" + this.DdlPM.SelectedValue + "' AND sys_User.f_WorkGroupID = sys_WorkGroupBrand.f_WorkGroupID AND sys_WorkGroupBrand.f_MID = sys_Brand.ID";

        this.DdlCustomer.DataSource = ReportSourceBLL.GetReport(sqlCustomer);

        this.DdlCustomer.DataBind();
        DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));

    }
    /// <summary>
    /// 客户下拉项改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            if (CurrentUser.Rule[0] == "项目经理")
            {
                DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(CurrentUser.WorkGroupID);
                DdlBrand.DataBind();

            }
            else
            {
                DdlBrand.DataSource = BrandBLL.GetList(ID);
                DdlBrand.DataBind();
            }

        }
        else
        {
            this.DdlBrand.Items.Clear();
            //DdlBrand.DataSource = null;
            //DdlBrand.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        //调用绑定分页和GridView
        BindGridView();
    }

    protected void BindGridView()
    {
        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        string CustomerName = DdlCustomer.SelectedItem.Value;
        
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        
        if (DateBegin == Function.ErrorDate)
        {
            Function.AlertMsg("开始日期有误"); return;
        }
        if (DateEnd == Function.ErrorDate)
        {
            Function.AlertMsg("结束日期有误"); return;
        }
        if (DateBegin > DateEnd)
        {
            Function.AlertMsg("开始日期不能大于结束日期"); return;
        }
        if (CustomerID <= 0)
        {
            Function.AlertMsg("请选择客户"); return;
        }
        string BrandName = DdlBrand.SelectedItem.Value;
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
        DateTime delayTime = DateTime.Now;

        //string strSQL = "SELECT sys_calls.f_CustomerName,CONVERT(VARCHAR(10),sys_calls.f_CreateDate,120) '时间',sys_calls.f_BrandName,sys_calls.f_ClassName1,sys_calls.f_ClassName2,";
        //strSQL += "COUNT(*) '开单总量',";
        //strSQL += "SUM(CASE WHEN f_StateMain = 3 OR f_StateMain = 4 THEN 1 ELSE 0 END) '完成总量',";
        //strSQL += "SUM(CASE WHEN f_StateMain = 2 OR f_StateMain = 1 THEN 1 ELSE 0 END) '处理中量'";
        //strSQL += ",SUM(CASE WHEN ( f_StateMain = 2 OR f_StateMain = 1 ) AND f_slaDateEnd < '" + delayTime.ToString() + "' THEN 1 ELSE 0 END) '处理中超期量'";
        //strSQL += " FROM sys_Calls, sys_User, sys_WorkGroupBrand";
        //strSQL += " WHERE sys_User.f_Code = '" + CurrentUser.Code + "'";
        //strSQL += " AND f_CustomerName = '" + CustomerName + "'";
        //if (BrandID != 0)
        //{
        //    strSQL += " AND f_BrandName = '" + BrandName + "'";
        
        //}
        //strSQL += " AND sys_User.f_WorkGroupID = sys_WorkGroupBrand.f_WorkGroupID";
        //strSQL += " AND sys_WorkGroupBrand.f_MID = sys_Calls.f_BrandID";
        //strSQL += " AND (sys_calls.f_CreateDate > '" + DateBegin.ToString("yyyy-MM-dd") + " 00:00:00' AND sys_calls.f_CreateDate < '" + DateEnd.ToString("yyyy-MM-dd") + " 23:59:59')";
        //strSQL += " GROUP BY CONVERT(VARCHAR(10),sys_calls.f_CreateDate,120),f_CustomerName,f_BrandName,f_ClassName1,f_ClassName2";
        //strSQL += " order by f_BrandName ,CONVERT(VARCHAR(10),sys_calls.f_CreateDate,120)";
        string strSQL = string.Empty;
        if (BrandID == 0)
        {
            strSQL += "SELECT  f_CustomerName,CONVERT(VARCHAR(10),f_CreateDate,120) '时间','' as f_BrandName,f_ClassName1,f_ClassName2,";

        }
        else
        {
            strSQL += "SELECT  f_CustomerName,CONVERT(VARCHAR(10),f_CreateDate,120) '时间',f_BrandName,f_ClassName1,f_ClassName2,";
        }
        strSQL += "COUNT(*) '开单总量',";
        strSQL += "SUM(CASE WHEN f_StateMain = 3 OR f_StateMain = 4 THEN 1 ELSE 0 END) '完成总量',";
        strSQL += "SUM(CASE WHEN f_StateMain = 2 OR f_StateMain = 1 THEN 1 ELSE 0 END) '处理中量'";
        strSQL += ",SUM(CASE WHEN ( f_StateMain = 2 OR f_StateMain = 1 ) AND f_slaDateEnd < '" + delayTime.ToString() + "' THEN 1 ELSE 0 END) '处理中超期量'";
        strSQL += " FROM sys_Calls";

        strSQL += " WHERE f_CustomerID = '" + CustomerID + "'";

        if (BrandID != 0)
        {
            strSQL += " AND f_BrandID = '" + BrandID + "'";
        }

        strSQL += " AND (f_CreateDate > '" + DateBegin.ToString("yyyy-MM-dd") + " 00:00:00' AND f_CreateDate < '" + DateEnd.ToString("yyyy-MM-dd") + " 23:59:59')";
        if (BrandID == 0)
        {
            strSQL += " GROUP BY CONVERT(VARCHAR(10),f_CreateDate,120),f_CustomerName,f_ClassName1,f_ClassName2";
        }
        else
        {
            strSQL += " GROUP BY CONVERT(VARCHAR(10),f_CreateDate,120),f_CustomerName,f_BrandName,f_ClassName1,f_ClassName2";

        }
        strSQL += " order by CONVERT(VARCHAR(10),f_CreateDate,120),f_ClassName1";

        DataTable dt = ReportSourceBLL.GetReport(strSQL);
        //初始化分页数据源实例
        PagedDataSource pds = new PagedDataSource();
        //设置总行数
        AspNetPager1.RecordCount = dt.Rows.Count;

        //设置分页的数据源
        pds.DataSource = dt.DefaultView;
        //设置当前页
        pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
        //设置每页显示页数
        pds.PageSize = AspNetPager1.PageSize;
        //启用分页
        pds.AllowPaging = true;
        //设置GridView的数据源为分页数据源
        if (BrandID == 0)
        {
            GridView1.Visible = false;
            hdGridView1.Visible = true;
            hdGridView1.DataSource = pds;
            //绑定GridView
            hdGridView1.DataBind();

        }
        else
        {
            GridView1.Visible = true;
            hdGridView1.Visible = false;
            GridView1.DataSource = pds;
            //绑定GridView
            GridView1.DataBind();
        }

    }
}
