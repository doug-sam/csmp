using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class PowerInfo
    {
            public static Dictionary<string, int> ToDictionary(Type enumType)
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                foreach (var item in Enum.GetValues(enumType))
                {
                    dic.Add(Enum.GetName(enumType, (int)item), (int)item);
                }
                return dic;
            }

            public static Dictionary<string, int> GetSecond(int First)
            {
                switch (First)
                {
                    case (int)PMain.报修管理: return ToDictionary(typeof(P1_Call));
                    case (int)PMain.系统管理: return ToDictionary(typeof(P2_Manager));
                    case (int)PMain.基础数据管理: return ToDictionary(typeof(P3_BaseData));
                    case (int)PMain.操作人员管理: return ToDictionary(typeof(P4_User));
                    case (int)PMain.报表: return ToDictionary(typeof(P5_Report));
                    case (int)PMain.知识库: return ToDictionary(typeof(P6_KnowledgeBase));
                    case (int)PMain.客户请求管理: return ToDictionary(typeof(P7_CustomerRequest));
                    default:
                        return null;
                }
            }

            public enum PMain { 报修管理 = 1, 系统管理, 基础数据管理, 操作人员管理,报表,知识库,客户请求管理 };
            public enum P1_Call { 新建报修 = 101, 记录查看, 跟进处理, 转派, 回访, 删除记录, 编辑记录, 查看组内所有报修, 处理组内所有报修, 关闭报修, 查询不同状态报修, 数据导出, 店铺催促, 操作回滚, 批量报修, 处理记录编辑, 安排上门, 备件记录, 更换上门人 };
            public enum P2_Manager { 日志管理 = 201, 数据导入, 系统信息, 系统设置,文件查看,文件删除,文件上传 };
            public enum P3_BaseData { 客户品牌管理 = 301, 故障类管理, 地区管理, 店铺查看, 店铺管理, 优先级管理, 第三方管理, 常用收件人, 文件管理, 文件删除, JobCode管理, 工作周期管理, 服务类型管理, 中继号码管理 };
            public enum P4_User { 用户查看 = 401, 用户管理,权限组管理, 工作组管理, 组_品牌关系管理 };
            public enum P5_Report { 城市报修统计 = 501, 城市大类故障, 时段故障分析, pending分析, 品牌pending分析, 明细数据, 品牌_大类故障占比, 品牌_服务方式占比, 品牌大中类故障, 报修总量走势, 门店的TOP10, 二线考核, 一线考核, 考勤管理, 派工邮件统计, 客户_日报修统计品牌, 客户_日报修统计故障, 客户_月报修统计品牌, 客户_月报修统计故障, 客户_日期段统计品牌, 客户_日期段统计故障, PM_日报修统计故障, PM_日报修统计品牌, PM_月报修统计故障, PM_月报修统计品牌, PM_日期段统计品牌, PM_日期段统计故障 };
            public enum P6_KnowledgeBase { 解决方案管理 = 601, 回访管理,评价管理,知识库查看,知识库添加,知识库编辑删除 }
            public enum P7_CustomerRequest { 添加请求 = 701, 关闭请求, 编辑请求, 删除请求,无效数据及受理情况检索 }



    }


}
