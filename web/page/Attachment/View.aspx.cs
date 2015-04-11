using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Attachment_View : _Sys_Attachment
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int CallID = Function.GetRequestInt("ID");
            if (CallID <= 0)
            {
                Function.AlertBack("参数有误");
                return;
            }
            GridView1.DataSource = AttachmentBLL.GetList(CallID,AttachmentInfo.EUserFor.Call);
            GridView1.DataBind();
        }
    }


    protected void ImgBtnDownLoad_Click(object sender, ImageClickEventArgs e)
    {
        int ID = Function.ConverToInt(((ImageButton)sender).CommandArgument);
        Response.Redirect("/page/sys/DownLoadFile.ashx?ID="+ID);
    }
}
