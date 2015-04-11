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

public partial class page_Inport_Store : _Sys_Inport
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Url = Request.Url.AbsoluteUri;
            Response.Write(string.Format("URL:{0}<br/>Port:{1}", Url, Request.Url.Port));
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
        dt = Function.ExcelToDatatable(FileDir, string.Empty);
        #region 列
        if (dt.Columns.Count < 10)
        {
            Function.AlertBack("格式错误，表中应该拥有10列"); return;
        }
        if (dt.Columns[0].ColumnName.Trim() != "店铺号")
        {
            Function.AlertBack("格式错误，表中第1列应为《店铺号》"); return;
        }
        if (dt.Columns[1].ColumnName.Trim() != "店铺名称")
        {
            Function.AlertBack("格式错误，表中第2列应为《店铺名称》"); return;
        }
        if (dt.Columns[2].ColumnName.Trim() != "对应品牌")
        {
            Function.AlertBack("格式错误，表中第3列应为《对应品牌》"); return;
        }
        if (dt.Columns[3].ColumnName.Trim() != "对应客户")
        {
            Function.AlertBack("格式错误，表中第4列应为《对应客户》"); return;
        }
        if (dt.Columns[4].ColumnName.Trim() != "所属省份")
        {
            Function.AlertBack("格式错误，表中第5列应为《所属省份》"); return;
        }
        if (dt.Columns[5].ColumnName.Trim() != "所属城市")
        {
            Function.AlertBack("格式错误，表中第6列应为《所属城市》"); return;
        }
        if (dt.Columns[6].ColumnName.Trim() != "地址")
        {
            Function.AlertBack("格式错误，表中第7列应为《地址》"); return;
        }
        if (dt.Columns[7].ColumnName.Trim() != "电话")
        {
            Function.AlertBack("格式错误，表中第8列应为《电话》"); return;
        }
        if (dt.Columns[8].ColumnName.Trim() != "邮箱")
        {
            Function.AlertBack("格式错误，表中第9列应为《邮箱》"); return;
        }
        if (dt.Columns[9].ColumnName.Trim() != "是否可用")
        {
            Function.AlertBack("格式错误，表中第10列应为《是否可用》"); return;
        }
        #endregion


        List<StoreInfo> list = new List<StoreInfo>();
        StoreInfo info = new StoreInfo();
        BrandInfo binfo = new BrandInfo();
        CityInfo cinfo = new CityInfo();
        CustomersInfo cusinfo = new CustomersInfo();
        int CityErrorFlag = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
            {
                continue;// Function.AlertMsg(i.ToString());
            }
            if (dt.Rows[i][0].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《店铺号》过长！", (i + 1))); return;
            }
            if (!CbUpdate.Checked)
            {
                if (null != StoresBLL.GetByStoreNo(dt.Rows[i][0].ToString().Trim()))
                {
                    Function.AlertBack(string.Format("第{0}行中《店铺号》已存在！", (i + 1))); return;
                }
            }
            if (!string.IsNullOrEmpty(dt.Rows[i][6].ToString().Trim()) && StoresBLL.TelExit(dt.Rows[i][6].ToString().Trim()))
            {
                Function.AlertBack(string.Format("第{0}行中《电话》已存在！", (i + 1))); return;
            }
            if (dt.Rows[i][1].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《店铺名称》过长！", (i + 1))); return;
            }
            cusinfo = CustomersBLL.Get(dt.Rows[i][3].ToString());
            if (null == cusinfo || cusinfo.ID == 0)
            {
                Function.AlertMsg(string.Format("第{0}行中《对应客户》在系统中不存在！", (i + 1))); return;
            }

            binfo = BrandBLL.Get(dt.Rows[i][2].ToString().Trim(), cusinfo.ID);
            if (null == binfo || binfo.ID == 0)
            {
                Function.AlertBack(string.Format("第{0}行中《对应品牌》在系统中不存在！", (i + 1))); return;
            }
            if (string.IsNullOrEmpty(dt.Rows[i]["所属城市"].ToString()))
            {
                continue;
            }
            try
            {
                cinfo = CityBLL.Get(dt.Rows[i]["所属城市"].ToString().Trim());
                if (null == cinfo || cinfo.ID == 0)
                {
                    Function.AlertBack(string.Format("第{0}行中《所属城市》在系统中不存在！", (i + 1))); return;
                }
            }
            catch (Exception)
            {
                CityErrorFlag++;
                continue;

            }

            if (dt.Rows[i]["地址"].ToString().Trim().Length > 200)
            {
                Function.AlertBack(string.Format("第{0}行中《地址》过长！", (i + 1))); return;
            }
            if (dt.Rows[i]["电话"].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《电话》过长！", (i + 1))); return;
            }
            if (dt.Rows[i]["邮箱"].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《邮箱》过长！", (i + 1))); return;
            }
            info = new StoreInfo();
            info.No = dt.Rows[i]["店铺号"].ToString();
            info.Name = dt.Rows[i]["店铺名称"].ToString();
            info.BrandID = binfo.ID;
            info.BrandName = binfo.Name;
            info.ProvinceID = cinfo.ProvinceID;
            info.ProvinceName = ProvincesBLL.Get(cinfo.ProvinceID).Name;
            info.CityID = cinfo.ID;
            info.CityName = cinfo.Name;
            info.Address = dt.Rows[i]["地址"].ToString();
            info.Tel = dt.Rows[i]["电话"].ToString();
            info.IsClosed = dt.Rows[i]["是否可用"].ToString().Trim() == "可用" ? false : true;
            info.CustomerID = binfo.CustomerID;
            info.CustomerName = CustomersBLL.Get(binfo.CustomerID).Name;
            info.Email = dt.Rows[i]["邮箱"].ToString();
            list.Add(info);
        }
        //         
        if (list.Count > 0)
        {
            Function.AlertMsg("共发现" + dt.Rows.Count + "条数据，其中 " + list.Count + "条数据验证成功。\n请检查表中如果出现空铺号的记录不被导入\n请注意把excel单元格设置成纯文本格式！");
            GridView1.DataSource = dt;
            GridView1.DataBind();
            ViewState["list"] = list;
            BtnSubmit.Visible = true;
            //Function.AlertMsg(CityErrorFlag.ToString()+"条数据无法导入");
        }
        else
        {
            Function.AlertBack("表中没有任何数据。");
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (null == ViewState["list"]) return;
        List<StoreInfo> list = (List<StoreInfo>)ViewState["list"];
        int FlagAdd = 0;
        int FlagEdit = 0;

        StoreInfo InfoExist = null;
        foreach (StoreInfo item in list)
        {
            if (CbUpdate.Checked)
            {
                InfoExist = StoresBLL.GetByStoreNo(item.No);
                if (null != InfoExist)
                {
                    item.ID = InfoExist.ID;
                    item.AddDate = InfoExist.AddDate;
                    if (StoresBLL.Edit(item))
                    {
                        FlagEdit++;
                        continue;
                    }
                }
            }
            item.AddDate = DateTime.Now;
            if (StoresBLL.Add(item) > 0)
            {
                FlagAdd++;
            }
        }
        Function.AlertRefresh(FlagAdd + "条数据成功导入");
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
            Function.AlertBack("请上传excel");
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
