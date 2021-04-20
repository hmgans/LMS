using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submissions
    {
        public string UId { get; set; }
        public int AssId { get; set; }
        public DateTime? Time { get; set; }
        public string Contents { get; set; }
        public uint? Score { get; set; }

        public virtual Assignments Ass { get; set; }
        public virtual Student U { get; set; }
    }
}
