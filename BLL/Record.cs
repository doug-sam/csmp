using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;

namespace CSMP.BLL
{
    public static class RecordBLL
    {
        private static readonly DAL.RecordDAL dal = new DAL.RecordDAL();

        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static RecordInfo Get(string unickid)
        {
            if (unickid.IndexOf(".")>0)
            {
                unickid = unickid.Split('.')[0];
            }
            return dal.Get(unickid);
        }

        /// <summary>
        /// 获取Info。新版。与新的呼叫中心系统匹配。
        /// </summary>
        /// <param name="uniqueid">uniqueid</param>
        public static RecordInfo GetRecordInfo(string uniqueid)
        {
            return dal.GetRecordInfo(uniqueid);
        }
    }
}