using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Professor
    {
        public Professor()
        {
            Classes = new HashSet<Classes>();
        }

        public string UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public int DId { get; set; }

        public virtual Department D { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
