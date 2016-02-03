using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace Tool
{
    public class WordToHTML
    {
        /// <summary>
        /// Word转成Html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成html的保存路径</param>
        /// <param name="wordFileName">转换成html的文件名字</param>
        public static bool Word2Html(string path, string savePath, string wordFileName)
        {
            try
            {

            //Logger.GetLogger("WordToHTML").Info("Word转HTML函数被调用,word文档路径：" + path+"\r\n", null);
            //Logger.GetLogger("WordToHTML").Info("Word转HTML函数被调用,html文档路径：" + savePath+wordFileName + "\r\n", null);

                Word.ApplicationClass word = new Word.ApplicationClass();
                Type wordType = word.GetType();
                Word.Documents docs = word.Documents;
                Type docsType = docs.GetType();
                Word.Document doc = (Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
                Type docType = doc.GetType();
                string strSaveFileName = savePath + wordFileName + ".html";
                object saveFileName = (object)strSaveFileName;
                docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Word.WdSaveFormat.wdFormatFilteredHTML });
                docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
                wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("WordToHTML").Info("Word转HTML函数被调用,错误原因：" + ex.Message + "\r\n", null);
                Logger.GetLogger("WordToHTML").Info("Word转HTML函数被调用,word文档路径：" + path + "\r\n", null);
                Logger.GetLogger("WordToHTML").Info("Word转HTML函数被调用,html文档路径：" + savePath+wordFileName + "\r\n", null);

                return false;
                //ex.Message;
            }

        }
    }
}
