using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Courses>();
            Professor = new HashSet<Professor>();
            Student = new HashSet<Student>();
        }

        public int DId { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Courses> Courses { get; set; }
        public virtual ICollection<Professor> Professor { get; set; }
        public virtual ICollection<Student> Student { get; set; }
    }
}
