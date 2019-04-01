using System;
using System.Collections.Generic;

namespace WpfResumeBrowsingSystem.Domain.Models
{
    public partial class Experiences
    {
        public int Eid { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Location { get; set; }
        public int? StaffSid { get; set; }
        public string CompanyName { get; set; }

        public virtual Staffs StaffS { get; set; }
    }
}
