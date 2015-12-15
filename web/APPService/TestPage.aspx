<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="APPService_TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="现场工程师姓名："></asp:Label>&nbsp
        <asp:TextBox ID="tbxUserName" runat="server" Text="李意辛"></asp:TextBox>&nbsp
        <asp:Button ID="Button1"   runat="server" Text="获取我的工单" 
            onclick="Button1_Click" />&nbsp
        <asp:Label ID="Label13" runat="server" Text="页码："></asp:Label>&nbsp
        <asp:TextBox ID="txtPage" runat="server" Text="1"></asp:TextBox>&nbsp
        <asp:Button ID="btnHistory"   runat="server" Text="获取我的历史工单" onclick="btnHistory_Click" /><br />
        <asp:Label ID="Label8" runat="server" Text="状态："></asp:Label>&nbsp
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem Value="">不选</asp:ListItem>
            <asp:ListItem Value="0">待上门</asp:ListItem>
            <asp:ListItem Value="1">到达现场处理中</asp:ListItem>
            <asp:ListItem Value="2">已离场</asp:ListItem>
            <asp:ListItem Value="3">已完成</asp:ListItem>
        </asp:DropDownList>&nbsp
        <asp:Label ID="Label9" runat="server" Text="开始时间"></asp:Label>&nbsp
        <asp:TextBox ID="TxtDateBegin" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>&nbsp
        <asp:Label ID="Label10" runat="server" Text="结束时间"></asp:Label>&nbsp
        <asp:TextBox ID="TxbDateEnd" runat="server"  onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>&nbsp
        <asp:CheckBox ID="chkWorkgroup" runat="server"  Text="查同组"/>&nbsp
        <asp:Button ID="OrderSearch"   runat="server" Text="工单查询" onclick="OrderSearch_Click" 
            />&nbsp
        
        <br />
        <asp:Label ID="lab3" runat="server" Text="我的工单(status,0:待上门,1:上门处理中,2:已离场):"></asp:Label><br />
        <asp:Label ID="Label12" runat="server" Text="历史工单(status,0:已完成,1:已关闭):"></asp:Label><br />
        <asp:Label ID="Label11" runat="server" Text="工单查询(status,0:待上门,1:上门处理中,2:已离场,3:已完成):"></asp:Label><br />
        
        <asp:Label ID="labMyOrder" runat="server" ></asp:Label><br />
        <asp:Label ID="labOrderSearch" runat="server" >
        </asp:Label><br />
        <asp:Label ID="labHistoryOrder" runat="server" >
        </asp:Label><br /><br />
        <asp:Label ID="Label2" runat="server" Text="单号："></asp:Label>&nbsp
        <asp:TextBox ID="txbCallNo" runat="server"></asp:TextBox>&nbsp
        
        <asp:Button ID="btOnsite" runat="server" Text="到场签到" onclick="btOnsite_Click" />&nbsp
        <asp:Button ID="btLeave" runat="server" Text="离场" onclick="btLeave_Click" />&nbsp
        <asp:Label ID="Label4" runat="server" Text="更换工程师为（ID）："></asp:Label>&nbsp
        <asp:TextBox ID="tbxNewEngineerID" runat="server"></asp:TextBox>&nbsp
        <asp:Button ID="btnChange" runat="server" Text="更换现场工程师"  onclick="btnChange_Click"  />
            
        <br /><br />
        <asp:Label ID="Label5" runat="server" Text="对二线技术评分："></asp:Label>
        <asp:TextBox ID="txtTScore" runat="server"></asp:TextBox>&nbsp
        <asp:Label ID="Label6" runat="server" Text="对二线态度评分："></asp:Label>
        <asp:TextBox ID="txtAScore" runat="server"></asp:TextBox>&nbsp
        <asp:Label ID="Label7" runat="server" Text="现场描述："></asp:Label>
        <asp:TextBox ID="txtDetails" runat="server"></asp:TextBox>&nbsp
        <asp:Button ID="btnSendRecord" runat="server" Text="提交工作记录" 
            onclick="btnSendRecord_Click" />
        <br />
        <asp:Label ID="labResult"   runat="server" Text="执行结果：">
        </asp:Label><br />
        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
    </div>
    <div>
    
    </div>
    </form>
</body>
</html>
