using System;

namespace CSMP.Model
{
    [SerializableAttribute]
    public class SolutionInfo
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class3 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Class1 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Class3Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Class2Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Class1Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool EnableFlag { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SuggestCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SolveCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string EnableBy { get; set; }



    }
}
