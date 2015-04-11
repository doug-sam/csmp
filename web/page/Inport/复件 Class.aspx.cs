using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.IO;
using System.Data;

public partial class page_Inport_Class : _Sys_Inport
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }



    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        string FileDir = Upload();
        if (string.IsNullOrEmpty(FileDir))
        {
            return;
        }
        DataTable dt = new DataTable();
        
        try
        {
            dt = Function.ExcelToDatatable(FileDir, string.Empty);
        }
        catch (Exception ex)
        {

            Response.Write("系统内部错误，请联系管理员，错误代码见：\n" + ex.Message);
            Response.End();
        }
        ViewState["dt"] = dt;
        #region 列
        if (dt.Columns.Count < 6)
        {
            Function.AlertBack("格式错误，表中应该拥有5列"); return;
        }
        if (dt.Columns[0].ColumnName.Trim() != "大类")
        {
            Function.AlertBack("格式错误，表中第一列应为<大类>"); return;
        }
        if (dt.Columns[1].ColumnName.Trim() != "中类")
        {
            Function.AlertBack("格式错误，表中第二列应为<中类>"); return;
        }
        if (dt.Columns[2].ColumnName.Trim() != "小类")
        {
            Function.AlertBack("格式错误，表中第三列应为<小类>"); return;
        }
        if (dt.Columns[3].ColumnName.Trim() != "SLA")
        {
            Function.AlertBack("格式错误，表中第四列应为<SLA>"); return;
        }
        if (dt.Columns[4].ColumnName.Trim() != "客户")
        {
            Function.AlertBack("格式错误，表中第五列应为<客户>"); return;
        }
        if (dt.Columns[5].ColumnName.Trim() != "优先级")
        {
            Function.AlertBack("格式错误，表中第六 列应为<优先级>"); return;
        }
        #endregion

        int SuccessFlag=0;
        List<int> FailedFlag = new List<int>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
            {
                FailedFlag.Add(i+1);
                continue;
            }
            CustomersInfo cinfo = CustomersBLL.Get(dt.Rows[i][4].ToString().Trim());
            if (null == cinfo)
            {
                Function.AlertBack(string.Format("第{0}行中《客户》无法在系统中找到！", (i + 1))); return;
            }
           PrioritiesInfo pinfo= PrioritiesBLL.Get(dt.Rows[i][5].ToString().Trim());
           if (null==pinfo)
           {
               Function.AlertBack(string.Format("第{0}行中《优先级》无法在系统中找到！", (i + 1))); return;
           }
           if (Function.ConverToInt(dt.Rows[i][3].ToString().Trim())<=0)
           {
               Function.AlertBack(string.Format("第{0}行中《sla》有误！", (i + 1))); return;
           }
            SuccessFlag++;
        }

        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][1].ToString()))
        {
            BtnSubmit.Visible = true;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            string msgStr = string.Format("表中有{0}条数据。{1}条数据验证成功。亲可以导入了！", dt.Rows.Count, SuccessFlag);
            if (FailedFlag.Count>0)
            {
                msgStr +=string.Format("\n其中行数为{0}的记录导入失败。原因是大类名为空",FailedFlag.ToString());
            }
            Function.AlertMsg(msgStr); return;
        }
        else
        {
            ViewState["dt"] = null;
            Function.AlertBack("数据为空。");
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (null == ViewState["dt"]) return;
        DataTable dt = (DataTable)ViewState["dt"];
        int SuccessFlagC1 = 0;
        int SuccessFlagC2 = 0;
        int SuccessFlagC3 = 0;

        Class1Info c1;
        Class2Info c2;
        Class3Info c3;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            
            if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
            {
                continue;
            }
            CustomersInfo cinfo = CustomersBLL.Get(dt.Rows[i][4].ToString().Trim());

            c1 = Class1BLL.Get(dt.Rows[i][0].ToString().Trim(), cinfo.ID);
            if (null == c1)
            {
                c1 = new Class1Info();
                c1.CustomerID = cinfo.ID;
                c1.IsClosed = false;
                c1.Name = dt.Rows[i][0].ToString().Trim();
                c1.ID = Class1BLL.Add(c1);
                if (c1.ID<=0)
                {
                    Function.AlertBack("导入过程意外出错。大类故障无法导入。请联系管理员"); return;
                }
                SuccessFlagC1++;
            }
            
            c2 = Class2BLL.Get(dt.Rows[i][1].ToString().Trim(),c1.ID);
            if (null==c2)
            {
                c2 = new Class2Info();
                c2.Class1ID = c1.ID;
                c2.Class1Name = c1.Name;
                c2.IsClosed = false;
                c2.Name = dt.Rows[i][1].ToString().Trim();
                c2.ID = Class2BLL.Add(c2);
                if (c2.ID <= 0)
                {
                    Function.AlertBack("导入过程意外出错。大类故障无法导入。请联系管理员"); return;
                }
                SuccessFlagC2++;
            }

            if (Class3BLL.Get(dt.Rows[i][2].ToString().Trim(), c2.ID)!=null)
            {
                continue;
            }

            PrioritiesInfo pinfo = PrioritiesBLL.Get(dt.Rows[i][5].ToString().Trim());
            c3 = new Class3Info();
            c3.Class2ID = c2.ID;
            c3.Class2Name = c2.Name;
            c3.IsClosed = false;
            c3.Name = dt.Rows[i][2].ToString().Trim();
            c3.PriorityID = pinfo.ID;
            c3.PriorityName = pinfo.Name;
            c3.SLA = Function.ConverToInt(dt.Rows[i][3].ToString().Trim());
            if (Class3BLL.Add(c3)>0)
            {
                SuccessFlagC3++;
            }

        }
        Function.AlertRefresh(string.Format("条{0}大类成功导入，条{1}中类成功导入，条{2}小类成功导入",SuccessFlagC1,SuccessFlagC2,SuccessFlagC3));
    }
    private string Upload()
    {
        if (!FileUpload1.HasFile)
        {
            Function.AlertBack("请选择文件");
            return string.Empty;
        }
        if (FileUpload1.PostedFile.ContentType != "application/vnd.ms-excel" && FileUpload1.PostedFile.ContentType != "application/octet-stream")
        {
            Function.AlertBack("请上传excel，并保证为97-2003格式");
            return string.Empty;
        }
        string Filename = FileUpload1.PostedFile.FileName;
        Filename = Filename.Replace("/", "\\");
        if (Filename.IndexOf("\\") > 0)
        {
            Filename = Filename.Substring(Filename.LastIndexOf("\\"));
        }
        string Path = Server.MapPath("~/excel/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/");
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
        FileUpload1.PostedFile.SaveAs(Path + Filename);
        return Path + Filename;
    }



}
