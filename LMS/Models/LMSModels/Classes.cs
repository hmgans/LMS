using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategory = new HashSet<AssignmentCategory>();
            Enrolled = new HashSet<Enrolled>();
        }

        public short ClassId { get; set; }
        public short CId { get; set; }
        public string SemesterSeason { get; set; }
        public uint SemesterYear { get; set; }
        public string ProfId { get; set; }
        public string Location { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public virtual Courses C { get; set; }
        public virtual Professor Prof { get; set; }
        public virtual ICollection<AssignmentCategory> AssignmentCategory { get; set; }
        public virtual ICollection<Enrolled> Enrolled { get; set; }
    }
}
