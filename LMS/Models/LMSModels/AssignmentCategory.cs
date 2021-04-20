using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class AssignmentCategory
    {
        public AssignmentCategory()
        {
            Assignments = new HashSet<Assignments>();
        }

        public int AcId { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public uint? Weight { get; set; }

        public virtual Classes Class { get; set; }
        public virtual ICollection<Assignments> Assignments { get; set; }
    }
}
