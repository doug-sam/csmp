using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_FeedbackChoose_list : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FeedbackQuestionInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("参数有误");
                return;
            }
            if (info.Type!=(int)SysEnum.QuestionType.Check&&info.Type!=(int)SysEnum.QuestionType.Radio)
            {
                Function.AlertBack("非选择题！");
                return;
            }
            LabName.Text = info.Name;
            LabType.Text = Enum.GetName(typeof(SysEnum.QuestionType), info.Type);

           GridView1.DataSource= FeedbackChooseBLL.GetList(info.ID);
           GridView1.DataBind();

        }
    }


    private FeedbackQuestionInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (FeedbackQuestionInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("QuestionID");
        if (ID <= 0)
        {
            return null;
        }
        FeedbackQuestionInfo info = FeedbackQuestionBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }



    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        //int ProvinceID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ProvinceID").ToString());
        //Literal Literal1 = (Literal)e.Row.FindControl("LtlProvince");
        //ProvincesInfo info = ProvincesBLL.Get(ProvinceID);
        //if (null != info)
        //{
        //    Literal1.Text = info.Name;
        //}

    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        //string delList = Function.GetRequestSrtring("ckDel");
        //if (string.IsNullOrEmpty(delList))
        //{
        //    Function.AlertBack("没有选中数据");
        //    return;
        //}
        //int Flag = 0;
        //foreach (string item in delList.Split(','))
        //{
        //    int ID = Function.ConverToInt(item);
        //    if (ID > 0)
        //    {
        //        if (CityBLL.Delete(ID))
        //        {
        //            Flag++;
        //        }
        //    }
        //}
        //Function.AlertRefresh(Flag + "条数据删除成功，\n如果数据未能删除，是由于有店铺属于该城市，\n请先删除在该城市下的店铺");
    }


}
