using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class Controls_ListRec : System.Web.UI.UserControl
{
    public int StoreID;
    public int UnCallID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (StoreID <= 0)
            {
                this.Visible = false;
                return;
            }
            BindHistoryCall(StoreID, UnCallID);
        }
    }

    private const string Strsql = " f_StoreID={0} and ID<>{1} order by ID desc ";
    private void BindHistoryCall(int StoreID, int UnCallID)
    {

        string strWhere = string.Format(Strsql, StoreID, UnCallID);
        int Count = 0;

        List<CallInfo> list = CallBLL.GetList(1000, 1, strWhere, out Count);
        GridView1.DataSource = list;
        GridView1.DataBind();
        this.Visible = Count > 0;

        ViewState["StoreID"] = StoreID;
        ViewState["UnCallID"] = UnCallID;

    }
    public UpdatePanelUpdateMode UpdateMode
    {
        get { return this.UpdatePanelListRec.UpdateMode; }
        set { this.UpdatePanelListRec.UpdateMode = value; }
    }

    public void Update()
    {
        this.UpdatePanelListRec.Update();
    }
    public void Update(int StoreID)
    {
        BindHistoryCall(StoreID, 0);
        this.UpdatePanelListRec.Update();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        int StoreID = Function.ConverToInt(ViewState["StoreID"]);
        int UnCallID = Function.ConverToInt(ViewState["UnCallID"]);

        BindHistoryCall(StoreID, UnCallID);
    }
}
