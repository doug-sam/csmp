using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CSMP.Model
{
    public static class SysEnum
    {
        public static Dictionary<int, string> ToDictionary(Type enumType)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(enumType))
            {
                dic.Add((int)item, Enum.GetName(enumType, (int)item));
            }
            return dic;
        }


        public enum LoginState { 登录成功 = 1, 无权登录, 用户不存在, 密码不正确 }
        public enum LogType { 系统出错 = 1, 普通日志, 邮件日志, 邮件发送记录, YUM邮件接口发送失败, 报修数据删除, 报修数据修改, 删除数据, 导出数据,系统任务执行监视 }
        /// <summary>
        /// call主状态
        /// </summary>
        public enum CallStateMain { 未处理 = 1, 处理中 = 2, 已完成 = 3,已关闭=4 }
        /// <summary>
        /// call明细状态
        /// </summary>
        public enum CallStateDetails { 
            系统接单_未处理 = 1,
            电话支持, 
            升级到客户, 
            等待备件, 
            等待安排上门, 
            等待第三方响应, 
            等待工程师上门, 
            工程师接收服务单, 
            上门支持, 
            处理完成, 
            已回访, 
            到达门店处理,
            等待厂商响应,
            二线离场确认,
            上门取消,
            开始处理,
            第三方预约上门,
            第三方预约取消,
            第三方处理离场,
            已回收服务单
        }

        /// <summary>
        /// 系统用户角色
        /// </summary>
        public enum Rule { 一线 = 1, 二线, 现场工程师, 客户, 管理员, 项目经理 }

        /// <summary>
        /// 系统工作组类型
        /// </summary>
        public enum WorkGroupType { 公司 = 1, 合作伙伴, 客户,}
       
        /// <summary>
        /// 问卷问题类型
        /// </summary>
        public enum QuestionType { Radio = 1, Check,Essay }

        /// <summary>
        /// CallStep中，Soluction小于零所代表意思
        /// </summary>
        public enum StepType { 
            回访 = 1,
            上门准备, 
            上门安排, 
            上门详细, 
            升级到客户, 
            远程支持, 
            工程师接收服务单, 
            关单, 
            到达门店处理, 
            店铺催促,
            二线离场确认,
            取消上门,
            开始处理,
            第三方预约上门,
            第三方预约取消,
            第三方上门备注,
            第三方处理离场,
            回收服务单
        }

        public enum ReplacementStatus { 没有备件跟进 = 0, 备件跟进中, 处理完成 }

        public enum SolvedBy { 远程支持 = 1, 客户解决,上门,第三方 }
        /*
         *需要有流程的
         * 上门stateDetail  [等待备件,等待安排上门]                     dropIn.aspx
         *                  [等待第三方响应, 等待工程师上门]            dropIn1.aspx
         *                  [工程师接收服务单(这步不是必然的)]          callstep/ReciveBill.aspx
         *                  [到达门店处理]                              dropIn2.aspx         
         *                  [上门支持]                                  dropIn3.aspx
         *                 
         *StepType流程      [上门准备]
         *                  [上门安排]
         *                  [工程师接收服务单(这步不是必然的)]
         *                  [到达门店处理]
         *                  [上门详细]
         *                  
         */

        /// <summary>
        /// 根据StepType找回StepDetail中的对应步骤ID
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        public static int GetStateDetails(SysEnum.StepType stepType)
        {

            switch (stepType)
            {
                case SysEnum.StepType.回访:
                    return (int)SysEnum.CallStateDetails.已回访;
                case SysEnum.StepType.上门准备:
                    return (int)SysEnum.CallStateDetails.等待安排上门;
                case SysEnum.StepType.上门安排:
                    return (int)SysEnum.CallStateDetails.等待工程师上门;
                case SysEnum.StepType.上门详细:
                    return (int)SysEnum.CallStateDetails.上门支持;
                case SysEnum.StepType.升级到客户:
                    return (int)SysEnum.CallStateDetails.升级到客户;
                case SysEnum.StepType.远程支持:
                    return (int)SysEnum.CallStateDetails.电话支持;
                case SysEnum.StepType.工程师接收服务单:
                    return (int)SysEnum.CallStateDetails.等待工程师上门;
                case SysEnum.StepType.关单:
                    return (int)SysEnum.CallStateDetails.处理完成;
                case SysEnum.StepType.到达门店处理:
                    return (int)SysEnum.CallStateDetails.到达门店处理;
                case SysEnum.StepType.店铺催促:
                    break;
                case SysEnum.StepType.二线离场确认:
                    return (int)SysEnum.CallStateDetails.二线离场确认;
                case SysEnum.StepType.第三方预约上门:
                    return (int)SysEnum.CallStateDetails.第三方预约上门;
                case SysEnum.StepType.第三方处理离场:
                    return (int)SysEnum.CallStateDetails.第三方处理离场;
                case SysEnum.StepType.回收服务单:
                    return (int)SysEnum.CallStateDetails.已回收服务单;
                default:
                    break;

            }
            return 0;
        }

    }
    public static class DicInfo
    {

        public class DicDictionary : Dictionary<int, string>
        {
            public string GetValue(int key)
            {
                return Keys.Contains(key) ? this[key] : string.Empty;
            }
        }

        /// <summary>
        /// 系统能接受的最初日期，1970-01-01
        /// </summary>
        public static readonly DateTime DateZone = new DateTime(1970, 1, 1);
        public const string Admin = "admin";



    }
}