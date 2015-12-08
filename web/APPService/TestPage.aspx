<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="APPService_TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="现场工程师姓名："></asp:Label>&nbsp
        <asp:TextBox ID="tbxUserName" runat="server" Text="李意辛"></asp:TextBox>&nbsp
        <asp:Button ID="Button1"   runat="server" Text="获取我的工单" 
            onclick="Button1_Click" /><br />
        <asp:Label ID="lab3" runat="server" Text="我的工单(status,0:待上门,1:上门处理中,2:已离场):"></asp:Label><br />
        <asp:Label ID="labMyOrder" runat="server" >
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
