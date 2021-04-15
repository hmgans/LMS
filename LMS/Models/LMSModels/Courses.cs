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

        public short CId { get; set; }
        public short Number { get; set; }
        public sbyte DId { get; set; }
        public string Name { get; set; }

        public virtual Department D { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
