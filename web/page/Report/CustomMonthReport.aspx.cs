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

public partial class page_Report_CustomMonthReport : _Report_ReportCMClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (CurrentUser.Rule[0] == "客户")
            {
                string userID = CurrentUser.ID.ToString();

                DdlCustomer.DataSource = CustomersBLL.GetCustomerByUserID(userID);
                DdlCustomer.DataBind();

                //绑定品牌
                DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(CurrentUser.WorkGroupID);
                DdlBrand.DataBind();
                if (DdlBrand.Items.Count == 1)
                    DdlBrand.SelectedIndex = 0;
                else
                DdlBrand.Items.Insert(0, new ListItem("请选择", "0"));
                //DdlCustomer.SelectedValue = "71";
                //DdlCustomer.Enabled = false;

            }
            else
            {
                CustomDataBlind();
                BrandDataBlind();
            }
            
            TxtDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM");

        }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        //调用绑定分页和GridView
        BindGridView();
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {

        AspNetPager1.CurrentPageIndex = 1;
        BindGridView();

    }
    /// <summary>
    /// 绑定客户列表
    /// </summary>
    protected void CustomDataBlind()
    {
        string sqlPM = "SELECT ID,f_Name as Name FROM sys_Customers";

        this.DdlCustomer.DataSource = ReportSourceBLL.GetReport(sqlPM);
        this.DdlCustomer.DataBind();
        DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
    }
    protected void BindGridView()
    {
        DateTime DateBegin = Function.ConverToDateTime(TxtDateBegin.Text.Trim()+"-1");
        string txtDateBegin = "Select DATEADD(mm, DATEDIFF(mm,0,'" + DateBegin.ToString("yyyy-MM-dd") + "'), 0)";
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim()+"-1");
        string txtDateEnd = "Select dateadd(ms,-3,DATEADD(mm, DATEDIFF(m,0,'" + DateEnd.ToString("yyyy-MM-dd") + "')+1, 0))";
        int CustomerID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
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

        

        DateTime delayTime = DateTime.Now;

        string strSQL = string.Empty;
        if (BrandID == 0)
        {
            strSQL += "SELECT f_CustomerName,CONVERT(VARCHAR(7),f_CreateDate,120) '时间','' as f_BrandName,f_ClassName1,f_ClassName2,";
        }
        else { 
            strSQL += "SELECT f_CustomerName,CONVERT(VARCHAR(7),f_CreateDate,120) '时间',f_BrandName,f_ClassName1,f_ClassName2,";
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

        strSQL += " AND f_CreateDate > (" + txtDateBegin + ") AND f_CreateDate < ("+txtDateEnd+")";
        if (BrandID == 0)
        {
            strSQL += " GROUP BY CONVERT(VARCHAR(7),f_CreateDate,120),f_CustomerName,f_ClassName1,f_ClassName2";
        }
        else
        {
            strSQL += " GROUP BY CONVERT(VARCHAR(7),f_CreateDate,120),f_CustomerName,f_BrandName,f_ClassName1,f_ClassName2";
        }

        strSQL += " order by CONVERT(VARCHAR(7),f_CreateDate,120),f_ClassName1";
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
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            if (CurrentUser.Rule[0] == "客户")
            {
                DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(CurrentUser.WorkGroupID);
                DdlBrand.DataBind();
                if (DdlBrand.Items.Count == 1)
                    DdlBrand.SelectedIndex = 0;
                else
                    DdlBrand.Items.Insert(0, new ListItem("请选择", "0"));

            }
            else
            {
                DdlBrand.DataSource = BrandBLL.GetList(ID);
                DdlBrand.DataBind();
                DdlBrand.Items.Insert(0, new ListItem("请选择", "0"));
            }

        }
        else
        {
            this.DdlBrand.Items.Clear();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
            //DdlBrand.DataSource = null;
            //DdlBrand.DataBind();
        }

    }
}
