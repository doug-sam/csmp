using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;


public partial class page_User_edit : _User_User_Edit
{
    private static ListItem defaultitem = new ListItem("请选择", "0");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlWorkGroup.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup.DataBind();
            DdlPower.DataSource = GroupBLL.GetList();
            DdlPower.DataBind();
            DdlPower.Items.Insert(0, defaultitem);
            int ID = Function.GetRequestInt("ID");
            if (ID <= 0)
            {
                LtlAction.Text = "添加";
                DdlWorkGroup.Items.Insert(0, defaultitem);
            }
            else
            {
                LtlAction.Text = "编辑";
                UserInfo info = UserBLL.Get(ID);
                if (null == info)
                {
                    Function.AlertMsg("参数有误"); return;
                }
                ViewState["info"] = info;
                TxbCode.Text = info.Code;
                TxbName.Text = info.Name;
                TxbOperatorID.Text = info.OperatorID;
                TxbTel.Text = info.Tel;
                TxbPhone.Text = info.Phone;
                TxbEmail.Text = info.Email;
                DdlWorkGroup.SelectedValue = info.WorkGroupID.ToString();
                RblSex.SelectedValue = info.Sex ? "1" : "0";
                DdlPower.SelectedValue = info.PowerGroupID.ToString();
                CbEnable.Checked = !info.Enable;
            }

        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (TxbCode.Text.IndexOf(",") >= 0)
        {
            Function.AlertMsg("登录名不能存在特殊符号");
            return;
        }

        UserInfo info;
        if (null == ViewState["info"])
        {
            info = new UserInfo();
            if (TxbPwd.Text.Trim().Length < 6 || TxbPwd.Text.Trim().Length > 20)
            {
                Function.AlertMsg("密码必需6到20位间");
                return;
            }

            if (UserBLL.Get(TxbCode.Text.Trim()) != null)
            {
                Function.AlertMsg("该登录ID已被占用！！");
                return;
            }
            info.PassWord = Md5Helper.Md5(TxbPwd.Text.Trim());
            info.LastDate = info.CreateDate = DateTime.Now;
        }
        else
        {
            info = (UserInfo)ViewState["info"];
            if (!string.IsNullOrEmpty(TxbPwd.Text.Trim()))
            {
                if (TxbPwd.Text.Trim().Length < 6 || TxbPwd.Text.Trim().Length > 20)
                {
                    Function.AlertMsg("密码必需6到20位间");
                    return;
                }
                info.PassWord = Md5Helper.Md5(TxbPwd.Text.Trim());
            }

            UserInfo uinfoDB = UserBLL.Get(TxbCode.Text.Trim());
            if (null==uinfoDB)
            {
                Function.AlertMsg("严重错误");
                return;
            }
            if (uinfoDB.ID!=info.ID)
            {
                Function.AlertMsg("登录ID已被占用");
                return;
            }
        }
        WorkGroupInfo winfo = WorkGroupBLL.Get(Function.ConverToInt(DdlWorkGroup.SelectedValue));
        if (null == winfo)
        {
            Function.AlertMsg("请选择工作组"); return;
        }

        info.Code = TxbCode.Text.Trim();
        info.Name = TxbName.Text.Trim();
        info.OperatorID = TxbOperatorID.Text.Trim();
        info.Tel = TxbTel.Text.Trim();
        info.Phone = TxbPhone.Text.Trim();
        info.Email = TxbEmail.Text.Trim();
        info.CityID = winfo.CityID;
        info.Sex = RblSex.SelectedValue == "1";
        info.PowerGroupID = Function.ConverToInt(DdlPower.SelectedValue);
        GroupInfo ginfo = GroupBLL.Get(info.PowerGroupID);
        if (null == ginfo)
        {
            Function.AlertMsg("请选择权限组"); return;
        }
        info.Rule = ginfo.Rule;
        info.Enable = !CbEnable.Checked;
        info.WorkGroupID = winfo.ID;
        if (info.Code.Length > 50)
        {
            Function.AlertMsg("登录名过长"); return;
        }
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("用户名过长"); return;
        }
        if (info.Tel.Length > 20)
        {
            Function.AlertMsg("电话过长"); return;
        }
        if (info.Phone.Length > 20)
        {
            Function.AlertMsg("手机过长"); return;
        }
        if (info.Email.Length > 50)
        {
            Function.AlertMsg("邮箱过长"); return;
        }
        if (null == ViewState["info"])
        {
            if (UserBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功", "main"); return;
            }
            else
            {
                Function.AlertMsg("添加失败，请重试或联系管理员"); return;
            }
        }
        else
        {
            if (UserBLL.Edit(info))
            {
                Function.AlertRefresh("编辑成功", "main"); return;
            }
            else
            {
                Function.AlertMsg("编辑失败，请重试或联系管理员"); return;
            }
        }
    }
}
