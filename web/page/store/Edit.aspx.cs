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

public partial class page_Store_Edit : _BaseData_Store_Edit
{
    private static readonly ListItem DdlItemDefault = new ListItem("请选择", "0");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlCustomer.DataSource = CustomersBLL.GetList();
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            //StoreTypeBind();           
            StoreInfo info = GetInfo();
            if (null == info)
            {
                DdlProvince.Items.Insert(0, DdlItemDefault);
            }
            else
            {
                LabAction.Text = "编辑";
                TxbName.Text = info.Name;
                TxbNO.Text = info.No;
               // TxbNO.Enabled = false;
                DdlCustomer.SelectedValue = info.CustomerID.ToString();
                DdlCustomer_SelectedIndexChanged(sender, e);
                DdlBrand.SelectedValue = info.BrandID.ToString();
                DdlProvince.SelectedValue = info.ProvinceID.ToString();
                DdlProvince_SelectedIndexChanged(sender, e);
                DdlCity.SelectedValue = info.CityID.ToString();
                TxbAddress.Text = info.Address;
                TxbTel.Text = info.Tel;
                TxbEmail.Text = info.Email;
                RblIsClose.SelectedIndex = info.IsClosed ? 1 : 0;
                DdlCustomer.Enabled = DdlBrand.Enabled = false;
                TxbStoreType.Text = info.StoreType;
            }
        }
    }

    private StoreInfo GetInfo()
    {
        StoreInfo info;
        if (ViewState["info"] != null)
        {
            info = (StoreInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = StoresBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        StoreInfo info = GetInfo();
        if (null == info)
        {
            if (StoresBLL.GetByStoreNo(TxbNO.Text.Trim()) != null)
            {
                Function.AlertMsg("店名名已存在"); return;
            }
            if (!string.IsNullOrEmpty(TxbTel.Text.Trim())&& StoresBLL.TelExit(TxbTel.Text.Trim()))
            {
                Function.AlertMsg("该电话已存在"); return;
            }

            info = new StoreInfo();
        }
        else
        {
            //店铺号不能在编辑时候与其它铺号重复
            StoreInfo tempStoreInfo=StoresBLL.GetByStoreNo(TxbNO.Text.Trim());
            if (tempStoreInfo != null && tempStoreInfo.ID!=info.ID)
            {
                Function.AlertMsg("店铺号已经存在了"); return;
            }
        }
        BrandInfo binfo = BrandBLL.Get(Function.ConverToInt(DdlBrand.SelectedValue));
        if (binfo == null)
        {
            Function.AlertMsg("请选择品牌"); return;
        }

        CustomersInfo cinfo = CustomersBLL.Get(binfo.CustomerID);

        info.No = TxbNO.Text.Trim();
        info.Name = TxbName.Text.Trim();
        info.BrandID = binfo.ID;
        info.BrandName = binfo.Name;
        info.ProvinceID = Function.ConverToInt(DdlProvince.SelectedValue);
        info.ProvinceName = DdlProvince.SelectedItem.Text;
        info.CityID = Function.ConverToInt(DdlCity.SelectedValue);
        info.CityName = DdlCity.SelectedItem.Text;
        info.Address = TxbAddress.Text.Trim();
        info.Tel = TxbTel.Text.Trim();
        info.IsClosed = RblIsClose.SelectedValue == "0";
        info.CustomerID = cinfo.ID;
        info.CustomerName = cinfo.Name;
        info.Email = TxbEmail.Text.Trim();
        info.StoreType = TxbStoreType.Text;
        
        if (info.No.Length > 50)
        {
            Function.AlertMsg("店铺编号过长"); return;
        }
        if (info.Name.Length > 50)
        {
            Function.AlertMsg("店铺名称过长"); return;
        }
        if (info.ProvinceID < 0)
        {
            Function.AlertMsg("请选择省份"); return;
        }
        if (info.CityID < 0)
        {
            Function.AlertMsg("请选择城市"); return;
        }
        if (info.Address.Length > 200)
        {
            Function.AlertMsg("地址过长"); return;
        }
        if (info.Tel.Length > 50)
        {
            Function.AlertMsg("电话过长"); return;
        }
        if (info.Email.Length > 50)
        {
            Function.AlertMsg("邮箱地址过长"); return;
        }

        if (GetInfo() == null)
        {
            info.AddDate = DateTime.Now;
            if (info.CustomerName =="汉堡王"||info.BrandName=="汉堡王")
            {
                Function.AlertRefresh("汉堡王店铺不能直接新增，请从数据导入页面导入。", "main");
            }

            if (StoresBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功","main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (info.CustomerName == "汉堡王" || info.BrandName == "汉堡王")
            {
                if (info.No != StoresBLL.Get(info.ID).No)
                {
                    
                    Function.AlertMsg("请不要修改汉堡王店铺店铺号信息！");
                }
            }
            if (StoresBLL.Edit(info))
            {
                if (info.CustomerName == "汉堡王" || info.BrandName == "汉堡王")
                {
                    
                    if (info.No.StartsWith("BK"))
                    {
                        info.No = info.No.Remove(0,2);
                    }
                    BKStoreInfo BKStore = BKStoreInfoBLL.GetByStoreNo(info.No);
                    if (BKStore != null)
                    {
                        BKStore.Name = info.Name;
                        BKStore.Address = info.Address;
                        BKStore.Tel = info.Tel;
                        BKStore.Email = info.Email;
                        BKStore.City = info.CityName;
                        BKStore.StoreType = info.StoreType;
                        //BKStore.Status = info.IsClosed ? "Closed" : "Open";
                        BKStoreInfoBLL.Edit(BKStore);
                        Function.AlertRefresh("修改成功", "main");
                    }
                    else {
                        Function.AlertMsg("修改成功，但汉堡王转呈系统表中没有对应的店铺信息");
                    }
                }
                Function.AlertRefresh("修改成功", "main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        DdlCity.DataSource = CityBLL.GetList(ID);
        DdlCity.DataBind();
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        if (ID <= 0)
        {
            DdlBrand.DataSource = new BrandInfo();
        }
        else
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
        }
        DdlBrand.DataBind();
        DdlBrand.Items.Insert(0, DdlItemDefault);
    }
    /// <summary>
    /// 绑定店铺类型下拉列表
    /// </summary>
    //protected void StoreTypeBind()
    //{
    //    var ss = Enum.GetNames(typeof(SysEnum.StoreType));
    //    foreach (var t in ss)
    //    {
    //        var j = (int)Enum.Parse(typeof(SysEnum.StoreType), t);
    //        DdlStoreType.Items.Add(new ListItem(t, j.ToString()));
    //    }
    //    DdlStoreType.Items.Insert(0, new ListItem("不确定", "0"));
    //}
}
