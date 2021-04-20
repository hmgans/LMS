using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignments
    {
        public Assignments()
        {
            Submissions = new HashSet<Submissions>();
        }

        public int AssId { get; set; }
        public int AcId { get; set; }
        public string Name { get; set; }
        public uint? Points { get; set; }
        public DateTime? DueDate { get; set; }
        public string Contents { get; set; }

        public virtual AssignmentCategory Ac { get; set; }
        public virtual ICollection<Submissions> Submissions { get; set; }
    }
}
