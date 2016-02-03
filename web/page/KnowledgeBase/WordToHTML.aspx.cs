using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_KnowledgeBase_WordToHTML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void toHTML_Click(object sender, EventArgs e)
    {
        string filePath = "~/file/download/普通镜头配置说明-Gazelle.docx";
        string htmlPath = "~/file/download/";
        string htmlName = DateTime.Now.ToString("普通镜头配置说明");

        Tool.WordToHTML.Word2Html(MapPath(filePath),MapPath(htmlPath),htmlName);
    }
}
