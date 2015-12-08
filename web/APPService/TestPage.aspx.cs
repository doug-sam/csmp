using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tool;

public partial class APPService_TestPage : System.Web.UI.Page
{
    string baiduURL = "http://192.168.1.112:8088/APPService/Default.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {

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
}
