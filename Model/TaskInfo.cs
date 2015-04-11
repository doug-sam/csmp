using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class TaskInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///每次执行间隔时间
        /// </summary>
        public decimal IntervalTime { get; set; }

        /// <summary>
        ///每次执行的具体时间点
        /// </summary>
        public DateTime ExcuteTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime ExcuteTimeLast { get; set; }

        /// <summary>
        ///执行周期类型，如day\week\month\hour
        /// </summary>
        public string CycleMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Enable { get; set; }

       public enum CycleModeInfo{Hour=1,Day=2,Week=3,Month=4}
    }
}
