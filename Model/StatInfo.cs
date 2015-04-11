using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public static class StatInfo
    {
        public enum ProjectType { NeverMind = 0, IsProject = 1, NotProject = 2 }
        public enum PendingType { NeverMind = 0, Yes = 1, No = 2 }
        public const string ProjectName = "项目";



    }
}
