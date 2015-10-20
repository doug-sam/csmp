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
using System.Collections;

public partial class page_Inport_BKStoreInfo : _Sys_Inport
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Url = Request.Url.AbsoluteUri;
        Response.Write(string.Format("URL:{0}<br/>Port:{1}", Url, Request.Url.Port));

    }

    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        string FileDir = Upload();
        if (string.IsNullOrEmpty(FileDir))
        {
            return;
        }
        DataTable dt = new DataTable();
        dt = Function.ExcelToDatatable(FileDir, "SITES");
        #region 列
        if (dt.Columns.Count < 35)
        {
            Function.AlertBack("格式错误，表中应该拥有35列"); return;
        }
        if (dt.Columns[0].ColumnName.Trim() != "Name")
        {
            Function.AlertBack("格式错误，表中第1列应为《Name》"); return;
        }
        if (dt.Columns[1].ColumnName.Trim() != "Code")
        {
            Function.AlertBack("格式错误，表中第2列应为《Code》"); return;
        }
        if (dt.Columns[2].ColumnName.Trim() != "SIGNED")
        {
            Function.AlertBack("格式错误，表中第3列应为《SIGNED》"); return;
        }
        if (dt.Columns[3].ColumnName.Trim() != "NICK_NAME")
        {
            Function.AlertBack("格式错误，表中第4列应为《NICK_NAME》"); return;
        }
        if (dt.Columns[4].ColumnName.Trim() != "STORE_TYPE")
        {
            Function.AlertBack("格式错误，表中第5列应为《STORE_TYPE》"); return;
        }
        if (dt.Columns[5].ColumnName.Trim() != "REGION")
        {
            Function.AlertBack("格式错误，表中第6列应为《REGION》"); return;
        }
        if (dt.Columns[6].ColumnName.Trim() != "CITY")
        {
            Function.AlertBack("格式错误，表中第7列应为《CITY》"); return;
        }
        if (dt.Columns[7].ColumnName.Trim() != "STATUS")
        {
            Function.AlertBack("格式错误，表中第8列应为《STATUS》"); return;
        }
        if (dt.Columns[8].ColumnName.Trim() != "OPEN_DATE")
        {
            Function.AlertBack("格式错误，表中第9列应为《OPEN_DATE》"); return;
        }
        if (dt.Columns[9].ColumnName.Trim() != "CLOSE_DATE")
        {
            Function.AlertBack("格式错误，表中第10列应为《CLOSE_DATE》"); return;
        }
        if (dt.Columns[10].ColumnName.Trim() != "TEL")
        {
            Function.AlertBack("格式错误，表中第11列应为《TEL》"); return;
        }


        if (dt.Columns[11].ColumnName.Trim() != "ADDRESS")
        {
            Function.AlertBack("格式错误，表中第12列应为《ADDRESS》"); return;
        }
        if (dt.Columns[12].ColumnName.Trim() != "OC")
        {
            Function.AlertBack("格式错误，表中第13列应为《OC》"); return;
        }
        if (dt.Columns[13].ColumnName.Trim() != "COMPANY_NAME")
        {
            Function.AlertBack("格式错误，表中第14列应为《COMPANY_NAME》"); return;
        }
        if (dt.Columns[14].ColumnName.Trim() != "EMAIL1")
        {
            Function.AlertBack("格式错误，表中第15列应为《EMAIL1》"); return;
        }
        if (dt.Columns[15].ColumnName.Trim() != "EMAIL2")
        {
            Function.AlertBack("格式错误，表中第16列应为《EMAIL2》"); return;
        }
        if (dt.Columns[16].ColumnName.Trim() != "LATITUDE")
        {
            Function.AlertBack("格式错误，表中第17列应为《LATITUDE》"); return;
        }
        if (dt.Columns[17].ColumnName.Trim() != "LONGITUDE")
        {
            Function.AlertBack("格式错误，表中第18列应为《LONGITUDE》"); return;
        }
        if (dt.Columns[18].ColumnName.Trim() != "PRICE_TIER")
        {
            Function.AlertBack("格式错误，表中第19列应为《PRICE_TIER》"); return;
        }
        if (dt.Columns[19].ColumnName.Trim() != "LAN_Gateway")
        {
            Function.AlertBack("格式错误，表中第20列应为《LAN_Gateway》"); return;
        }
        if (dt.Columns[20].ColumnName.Trim() != "Server IP")
        {
            Function.AlertBack("格式错误，表中第21列应为《Server IP》"); return;
        }
        if (dt.Columns[21].ColumnName.Trim() != "Tunnel_IP")
        {
            Function.AlertBack("格式错误，表中第22列应为《Tunnel_IP》"); return;
        }
        if (dt.Columns[22].ColumnName.Trim() != "VPN_Type")
        {
            Function.AlertBack("格式错误，表中第23列应为《VPN_Type》"); return;
        }
        if (dt.Columns[23].ColumnName.Trim() != "VPN_Router")
        {
            Function.AlertBack("格式错误，表中第24列应为《VPN_Router》"); return;
        }
        if (dt.Columns[24].ColumnName.Trim() != "OS_USERNAME")
        {
            Function.AlertBack("格式错误，表中第25列应为《OS_USERNAME》"); return;
        }
        if (dt.Columns[25].ColumnName.Trim() != "Teamviewer")
        {
            Function.AlertBack("格式错误，表中第26列应为《Teamviewer》"); return;
        }
        if (dt.Columns[26].ColumnName.Trim() != "Router_hostname")
        {
            Function.AlertBack("格式错误，表中第27列应为《Router_hostname》"); return;
        }
        if (dt.Columns[27].ColumnName.Trim() != "Router_password")
        {
            Function.AlertBack("格式错误，表中第28列应为《Router_password》"); return;
        }
        if (dt.Columns[28].ColumnName.Trim() != "Device_Model")
        {
            Function.AlertBack("格式错误，表中第29列应为《Device_Model》"); return;
        }
        if (dt.Columns[29].ColumnName.Trim() != "Device_Model")
        {
            Function.AlertBack("格式错误，表中第30列应为《Device_Model》"); return;

        }
        if (dt.Columns[30].ColumnName.Trim() != "ADSL_Info")
        {
            Function.AlertBack("格式错误，表中第31列应为《ADSL_Info》"); return;

        }
        if (dt.Columns[31].ColumnName.Trim() != "ADLS_OFFICE_INFO")
        {
            Function.AlertBack("格式错误，表中第32列应为《ADLS_OFFICE_INFO》"); return;
        }
        if (dt.Columns[32].ColumnName.Trim() != "ADSL_WIFI")
        {
            Function.AlertBack("格式错误，表中第33列应为《ADSL_WIFI》"); return;
        }
        if (dt.Columns[33].ColumnName.Trim() != "Alipay")
        {
            Function.AlertBack("格式错误，表中第34列应为《WeiChatPay》"); return;
        }
        if (dt.Columns[34].ColumnName.Trim() != "Alipay")
        {
            Function.AlertBack("格式错误，表中第35列应为《WeiChatPay》"); return;
        }

        
        

        #endregion

       
        List<StoreInfo> list = new List<StoreInfo>();
        StoreInfo info = new StoreInfo();
        List<BKStoreInfo> BKlist = new List<BKStoreInfo>();
        BKStoreInfo BKinfo = new BKStoreInfo();
        BrandInfo binfo = new BrandInfo();
        CityInfo cinfo = new CityInfo();
        CustomersInfo cusinfo = new CustomersInfo();
        int CityErrorFlag = 0;

        Hashtable hshTable = new Hashtable(); //  创建哈希表

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
            {
                continue;// Function.AlertMsg(i.ToString());
            }
            //由于汉堡王店铺写入CSMP系统数据库时前面要加BK两个字母以做区分
            if (dt.Rows[i][1].ToString().Trim().Length-2 > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《Code》过长！", (i + 1))); return;
            }
            if (!CbUpdate.Checked)
            {
                if (null != StoresBLL.GetByStoreNo("BK"+dt.Rows[i][1].ToString().Trim()))
                {
                    Function.AlertBack(string.Format("第{0}行中《Code》数据库中已存在！是否忘记勾选将已存在的更新为Excel中的数据", (i + 1))); return;
                }
            }

            if (hshTable.Contains(dt.Rows[i][0].ToString().Trim()))
            {
                string moreIndex = (string)hshTable[dt.Rows[i][1].ToString().Trim()].ToString(); //取哈希表里指定键的值
                Function.AlertBack(string.Format("Excel第{0}行中《Code》" + dt.Rows[i][1].ToString().Trim() + "与第{1}行重复！", (i + 1), moreIndex)); return;
            }


            if (!string.IsNullOrEmpty(dt.Rows[i][10].ToString().Trim()) && StoresBLL.TelExit(dt.Rows[i][10].ToString().Trim()))
            {
                Function.AlertBack(string.Format("第{0}行中《TEL》已存在！", (i + 1))); return;
            }
            if (dt.Rows[i][0].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《Name》过长！", (i + 1))); return;
            }
            cusinfo = CustomersBLL.Get("汉堡王");
            if (null == cusinfo || cusinfo.ID == 0)
            {
                Function.AlertMsg(string.Format("系统中不存在客户：汉堡王！", (i + 1))); return;
            }

            binfo = BrandBLL.Get("汉堡王", cusinfo.ID);
            if (null == binfo || binfo.ID == 0)
            {
                Function.AlertBack(string.Format("系统中不存在品牌：汉堡王！", (i + 1))); return;
            }
            if (string.IsNullOrEmpty(dt.Rows[i]["CITY"].ToString()))
            {
                continue;
            }
            try
            {
                cinfo = CityBLL.Get(dt.Rows[i]["CITY"].ToString().Trim());
                if (null == cinfo || cinfo.ID == 0)
                {
                    Function.AlertBack(string.Format("第{0}行中《CITY》在系统中不存在！", (i + 1))); return;
                }
            }
            catch (Exception)
            {
                CityErrorFlag++;
                continue;

            }

            if (dt.Rows[i]["ADDRESS"].ToString().Trim().Length > 200)
            {
                Function.AlertBack(string.Format("第{0}行中《ADDRESS》过长！", (i + 1))); return;
            }
            if (dt.Rows[i]["Tel"].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《Tel》过长！", (i + 1))); return;
            }
            if (dt.Rows[i]["EMAIL1"].ToString().Trim().Length > 50)
            {
                Function.AlertBack(string.Format("第{0}行中《EMAIL1》过长！", (i + 1))); return;
            }
            if (dt.Rows[i]["STORE_TYPE"].ToString().Trim().Length > 100)
            {
                Function.AlertBack(string.Format("第{0}行中《STORE_TYPE》过长！", (i + 1))); return;
            }
            info = new StoreInfo();
            info.No = "BK"+dt.Rows[i]["Code"].ToString();
            info.Name = dt.Rows[i]["Name"].ToString();
            info.BrandID = binfo.ID;
            info.BrandName = binfo.Name;
            info.ProvinceID = cinfo.ProvinceID;
            info.ProvinceName = ProvincesBLL.Get(cinfo.ProvinceID).Name;
            info.CityID = cinfo.ID;
            info.CityName = cinfo.Name;
            info.Address = dt.Rows[i]["ADDRESS"].ToString();
            info.Tel = dt.Rows[i]["Tel"].ToString();
            info.IsClosed = dt.Rows[i]["STATUS"].ToString().Trim() == "Opened" ? false : true;
            info.CustomerID = binfo.CustomerID;
            info.CustomerName = CustomersBLL.Get(binfo.CustomerID).Name;
            info.Email = dt.Rows[i]["EMAIL1"].ToString();
            info.StoreType = dt.Rows[i]["STORE_TYPE"].ToString();
            list.Add(info);

            BKinfo = new BKStoreInfo();
            BKinfo.LocalCode = dt.Rows[i]["Code"].ToString();
            BKinfo.GlobalCode = dt.Rows[i]["Code"].ToString();
            BKinfo.Name = dt.Rows[i]["Name"].ToString();
            BKinfo.City = cinfo.Name;
            BKinfo.Address = dt.Rows[i]["ADDRESS"].ToString();
            BKinfo.Tel = dt.Rows[i]["Tel"].ToString();
            BKinfo.Email = dt.Rows[i]["EMAIL1"].ToString();
            BKinfo.StoreType = dt.Rows[i]["STORE_TYPE"].ToString();
            BKlist.Add(BKinfo);

            hshTable.Add(dt.Rows[i]["Code"].ToString(), i + 1);  //  往哈希表里添加键值对
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
        if (null == ViewState["BKlist"]) return;
        List<BKStoreInfo> BKlist = (List<BKStoreInfo>)ViewState["BKlist"];
        int FlagSum = list.Count;
        int FlagAdd = 0;
        int FlagEdit = 0;

        StoreInfo InfoExist = null;
        BKStoreInfo BKinfoExist = null;
        for (int i = 0; i < list.Count;i++ )
        {
            if (CbUpdate.Checked)
            {
                InfoExist = StoresBLL.GetByStoreNo(list[i].No);
                if (null != InfoExist)
                {
                    list[i].ID = InfoExist.ID;
                    list[i].AddDate = InfoExist.AddDate;
                    if (StoresBLL.Edit(list[i]))
                    {
                        FlagEdit++;
                    }
                }
                BKinfoExist = BKStoreInfoBLL.GetByStoreNo(BKlist[i].LocalCode);
                if (null != BKinfoExist)
                {
                    BKlist[i].ID = BKinfoExist.ID;
                    BKStoreInfoBLL.Edit(BKlist[i]);
                
                }
                else {
                    BKStoreInfoBLL.Add(BKlist[i]);
                }
                continue;
            }
            list[i].AddDate = DateTime.Now;
            if (StoresBLL.Add(list[i]) > 0)
            {
                FlagAdd++;
            }
        }
        Function.AlertRefresh("共" + FlagSum + "条数据，" + FlagAdd + "条导入成功，" + FlagEdit + "条覆盖成功！");
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
