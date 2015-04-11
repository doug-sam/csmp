using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class AttachmentInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///上传者ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///是否共享
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CallID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CallStepID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int DirID { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///后缀名
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        ///文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        ///文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        ///发布日期
        /// </summary>
        public DateTime Addtime { get; set; }

        public string UseFor { get; set; }

       public enum EUserFor { Call=1,KnowledgeBase=2}
    }
}
