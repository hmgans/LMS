using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Courses
    {
        public Courses()
        {
            Classes = new HashSet<Classes>();
        }

        public int CId { get; set; }
        public int Number { get; set; }
        public int DId { get; set; }
        public string Name { get; set; }

        public virtual Department D { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
