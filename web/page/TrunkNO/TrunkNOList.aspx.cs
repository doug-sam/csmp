using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_TrunkNO_TrunkNOList : _BaseData_TrunkNO
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";
            string PNO = Function.GetRequestSrtring("PNO");
            if (!string.IsNullOrEmpty(PNO))
            {
                strWhere += " and f_PhysicalNo like '%" + PNO.Trim() + "%' ";
                url += "&PNO=" + PNO;
                TxbPNO.Text = PNO;
            }
            string VNO = Function.GetRequestSrtring("VNO");
            if (!string.IsNullOrEmpty(VNO))
            {
                strWhere += " and f_VirtualNo like '%" + VNO.Trim() + "%' ";
                url += "&VNO=" + VNO;
                TxbVNO.Text = VNO;
            }
            string Desc = Function.GetRequestSrtring("Desc");
            if (!string.IsNullOrEmpty(Desc))
            {
                strWhere += " and f_Description like '%" + Desc.Trim() + "%' ";
                url += "&Desc=" + Desc;
                TxbDesc.Text = Desc;
            }
            strWhere += " order by id desc ";
            GridView1.DataSource = TrunkNO.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    protected void Btn_Delete(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        foreach (string item in delList.Split(','))
        {
            if (item.Length > 0)
            {
                TrunkNO.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }

    //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.DataItem == null) return;
    //    BrandInfo info = (BrandInfo)e.Row.DataItem;
    //    SlaModeInfo infom = SlaModeBLL.Get(info.SlaModeID);
    //    if (null != infom)
    //    {
    //        Literal LtlSLA = (Literal)e.Row.FindControl("LtlSLA");
    //        LtlSLA.Text = infom.Name;
    //    }


    //}
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrunkNOList.aspx?PNO=" + TxbPNO.Text.Trim() + "&VNO=" + TxbVNO.Text.Trim() + "&Desc=" + TxbDesc.Text.Trim());
    }
}
