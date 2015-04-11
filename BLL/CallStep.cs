using System;
using System.Collections.Generic;
using CSMP.DAL;
using CSMP.Model;
using Tool;
using System.Text;
using System.Linq;

namespace CSMP.BLL
{
    public static class CallStepBLL
    {
        private static readonly DAL.CallStepDAL dal = new DAL.CallStepDAL();

        #region Get
        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallStepInfo> GetList(int PageSize, int CurPage, string StrWhere, out int Count)
        {
            return dal.GetList(PageSize, CurPage, StrWhere, out Count);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallStepInfo> GetList(string StrWhere)
        {
            return dal.GetList(StrWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public static List<CallStepInfo> GetList(int CallID, SysEnum.StepType StepType)
        {
            string StrWhere = string.Format(" 1=1 and f_CallID={0} and f_StepType={1} ", CallID, (int)StepType);
            return GetList(StrWhere);
        }


        /// <summary>
        /// 根据CallID获取记录
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static List<CallStepInfo> GetList(int CallID)
        {
            return dal.GetList(CallID);
        }
        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CallStepInfo GetLast(int CallID)
        { return dal.GetLast(CallID, null); }

        /// <summary>
        /// 根据CallID获取所在上门详细步的记录
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static List<CallStepInfo> GetListSln1(int CallID)
        {
            return dal.GetListSln1(CallID);
        }


        /// <summary>
        /// 获取最后一条指定操作的Info
        /// </summary>
        /// <param name="id">id</param>
        public static CallStepInfo GetLast(int CallID, SysEnum.StepType StepType)
        { return dal.GetLast(CallID, StepType); }


        /// <summary>
        /// 获取Info
        /// </summary>
        /// <param name="id">id</param>
        public static CallStepInfo Get(int id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 取得当前call共有多少步。没有则返回0
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public static int GetMaxStepIndex(int CallID)
        {
            return dal.GetMaxStepIndex(CallID);
        }

        public static List<CallStepInfo> GetListJoin(CallInfo info)
        {
            List<CallStepInfo> liststep = CallStepBLL.GetList(info.ID);
            List<CallStepInfo> listass = AssignToStep(AssignBLL.GetList(info.ID));
            List<CallStepInfo> listdrop = DropInMemoToStep(info, DropInMemoBLL.GetListOrderByID(info.ID));
            liststep.AddRange(listass);
            liststep.AddRange(listdrop);
            return liststep.OrderBy(CallStepInfo => CallStepInfo.AddDate).ToList();
        }



        /// <summary>
        /// 转派换成操作步骤
        /// </summary>
        /// <param name="ListAss"></param>
        /// <returns></returns>
        private static List<CallStepInfo> AssignToStep(List<AssignInfo> ListAss)
        {

            List<CallStepInfo> list = new List<CallStepInfo>();
            if (null == ListAss) return list;
            CallStepInfo info;
            foreach (AssignInfo item in ListAss)
            {
                info = new CallStepInfo();
                info.AddDate = item.AddDate;
                info.CallID = item.CallID;
                info.DateBegin = info.DateEnd = item.AddDate;
                info.Details = item.OldName + "把call转给了" + item.UserName;
                info.IsSolved = false;
                info.MajorUserID = item.OldID;
                info.MajorUserName = item.OldName;
                info.SolutionID = 0;
                info.SolutionName = string.Empty;
                info.StepIndex = 0;
                info.StepName = "转派";
                info.StepType = 0;
                info.UserID = item.CreatorID;
                info.UserName = item.CreatorName;
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 转派换成操作步骤
        /// </summary>
        /// <param name="ListAss"></param>
        /// <returns></returns>
        private static List<CallStepInfo> DropInMemoToStep(CallInfo cinfo, List<DropInMemoInfo> ListMemo)
        {
            List<CallStepInfo> list = new List<CallStepInfo>();
            if (null == ListMemo) return null;
            CallStepInfo info;
            foreach (DropInMemoInfo item in ListMemo)
            {
                info = new CallStepInfo();
                info.AddDate = item.AddDate;
                info.CallID = cinfo.ID;
                info.DateBegin = info.DateEnd = item.MemoDate;
                info.Details = item.Details;
                info.IsSolved = false;
                info.MajorUserID = item.UserID;
                info.MajorUserName = item.UserName;
                info.SolutionID = 0;
                info.SolutionName = string.Empty;
                info.StepIndex = 0;
                info.StepName =string.Format("{0} 备注",item.TypeName);
                info.StepType = 0;
                info.UserID = item.UserID;
                info.UserName = item.UserName;
                list.Add(info);

            }

            return list;
        }


        #endregion

        #region Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info">info</param>
        public static int Add(CallStepInfo info)
        {
            return dal.Add(info);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info">info</param>
        public static bool Edit(CallStepInfo info)
        {
            return dal.Edit(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public static bool Delete(int id)
        {
            return dal.Delete(id);
        }

        public static bool AddCallStep_UpdateCall(CallInfo cinfo, CallStepInfo sinfo)
        {
            bool Result = dal.AddCallStep_UpdateCall(cinfo, sinfo);
            if (Result && cinfo.StateDetail == (int)SysEnum.CallStateDetails.处理完成)
            {
                SolutionInfo slninfo = SolutionBLL.Get(sinfo.SolutionID);
                if (slninfo != null)
                {
                    slninfo.SolveCount++;
                    SolutionBLL.Edit(slninfo);
                }
            }
            return Result;
        }

        /// <summary>
        /// 将操作步骤后退，接收服务单，店铺催促将默认删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DeleteCallStep_UpdateCall(CallInfo info)
        {

            CallStepInfo sinfoPre = null;                           //要保留的一步
            List<CallStepInfo> listinfo = CallStepBLL.GetList(info.ID);

            for (int i = listinfo.Count - 1; i > 0; i--)
            {
                if (listinfo[i - 1].StepType != (int)SysEnum.StepType.工程师接收服务单 && listinfo[i - 1].StepType != (int)SysEnum.StepType.店铺催促)//以后步骤变更时这里得注意
                {
                    sinfoPre = listinfo[i - 1];
                    break;
                }
            }

            if (listinfo.Count == 1 || sinfoPre == null)
            {
                return "无法回滚!!!已经是第一步，或系统无法对该call继续做回滚。";
            }
            
            info.StateMain =sinfoPre.IsSolved?(int)SysEnum.CallStateMain.已完成: (int)SysEnum.CallStateMain.处理中;
            info.StateDetail = SysEnum.GetStateDetails((SysEnum.StepType)Enum.Parse(typeof(SysEnum.StepType), sinfoPre.StepType.ToString(), true));
            if (info.StateDetail == 0)
            {
                return "无法回滚!!!系统无法对当前状态进行回滚。";
            }

            OperationRecInfo opinfo = new OperationRecInfo();
            opinfo.AddDate = DateTime.Now;
            opinfo.CallID = info.ID;
            opinfo.Details = "";
            opinfo.FlagID = 0;
            opinfo.LogType = "报修操作回滚";
            opinfo.Memo = info.No;
            opinfo.UserID = UserBLL.GetCurrentEmployeeID();
            opinfo.UserName = UserBLL.GetCurrentEmployeeName();
           
            if (!FeedbackAnswerBLL.DeleteByCallID(info.ID))
            {
                return "回滚出错，无法删除问卷，请联系管理员";
            }

            if (!dal.DeleteCallStep_UpdateCall(info, sinfoPre, opinfo))
            {
                return "操作失败。无法保存数据";
            }
             return string.Empty;
        }

        #endregion
    }
}