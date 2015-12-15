using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tool;
using System.Configuration;

public partial class APPService_TestPage : System.Web.UI.Page
{
    
    string baiduURL = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string AbsoluteUri = Request.Url.AbsoluteUri;
        //string urlAbsolutePath = Request.Url.AbsolutePath;
        //string urlHost = AbsoluteUri.Remove(AbsoluteUri.IndexOf(urlAbsolutePath));
        //baiduURL = urlHost + "/APPService/Default.aspx";
        baiduURL = ConfigurationManager.AppSettings["WebServerUrl"].ToString()+"/APPService/Default.aspx";
    }
    /// <summary>
    /// 获取我的工单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        //string baiduURL = "http://192.168.31.112:8088/APPService/Default.aspx";
        string param = "param={\"code\":\"MyOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        labMyOrder.Text = result;
        labOrderSearch.Text = "";
        labHistoryOrder.Text = "";

    }
    protected void btOnsite_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        //string baiduURL = "http://192.168.31.112:8088/APPService/Default.aspx";
        string param = "param={\"code\":\"AssignOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"orderId\":\"" + txbCallNo.Text.Trim() + "\",\"oper\":\"0\",\"operTarget\":\"\"}";
        //string param = "{\"code\":\"MyOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        Label3.Text = result;

    }
    protected void btLeave_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        //string baiduURL = "http://192.168.31.112:8088/APPService/Default.aspx";
        string param = "param={\"code\":\"AssignOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"orderId\":\"" + txbCallNo.Text.Trim() + "\",\"oper\":\"1\",\"operTarget\":\"\"}";
        //string param = "{\"code\":\"MyOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        Label3.Text = result;
    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbxNewEngineerID.Text.Trim()))
        {
            Label3.Text = "更换工程师不能为空";
            return;
        }
        string result = string.Empty;

        string param = "param={\"code\":\"AssignOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"orderId\":\"" + txbCallNo.Text.Trim() + "\",\"oper\":\"2\",\"operTarget\":\"" + tbxNewEngineerID.Text.Trim() + "\"}";
        //string param = "{\"code\":\"MyOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        Label3.Text = result;
    }
    protected void btnSendRecord_Click(object sender, EventArgs e)
    {
        
        string result = string.Empty;

        //string param = "param={\"code\":\"AssignOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"orderId\":\"" + txbCallNo.Text.Trim() + "\",\"oper\":\"2\",\"operTarget\":\"" + tbxNewEngineerID.Text.Trim() + "\"}";
        string param = "param={\"code\":\"WorkRecord\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"orderId\":\"" + txbCallNo.Text.Trim() + "\",\"desc\":\"" + txtDetails.Text.Trim() + "\",\"technologyScore\":\"" + txtTScore.Text.Trim() + "\",\"attitudeScore\":\"" + txtAScore.Text.Trim() + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        Label3.Text = result;
    }
    protected void OrderSearch_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        string isSearchWorkgroup = string.Empty;
        if (chkWorkgroup.Checked)
        {
            isSearchWorkgroup = "1";
        }
        else {
            isSearchWorkgroup = "0";
        
        }
        string param = "param={\"code\":\"SearchOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"startTime\":\"" + TxtDateBegin.Text.Trim() + "\",\"endTime\":\"" + TxbDateEnd.Text.Trim() + "\",\"state\":\"" + DropDownList1.SelectedValue+ "\",\"all\":\"" + isSearchWorkgroup + "\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        labOrderSearch.Text = result;
        labMyOrder.Text = "";
        labHistoryOrder.Text = "";

    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        int page = 1;
        try
        {
            page = Convert.ToInt32(txtPage.Text);
        }
        catch {
            labHistoryOrder.Text = "请输入正确的页码";
            return;
        }

        string param = "param={\"code\":\"HistoryOrder\",\"userName\":\"" + tbxUserName.Text.Trim() + "\",\"page\":\"1\"}";
        result = WebUtil.DoPost(baiduURL, param, 1);
        labHistoryOrder.Text = result;
        labOrderSearch.Text = "";
        labMyOrder.Text = "";

    }
}
