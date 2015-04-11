using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class Log_View : _Sys_Log
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if(!IsPostBack)
        {
            LogInfo info = GetInfo();
            if (info==null)
            {
                Function.AlertBack("参数有误");
            }
        }
    }
    private LogInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (LogInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        LogInfo info = LogBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

}
