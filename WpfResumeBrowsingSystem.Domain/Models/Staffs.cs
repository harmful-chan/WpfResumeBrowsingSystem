using System;
using System.Collections.Generic;

namespace WpfResumeBrowsingSystem.Domain.Models
{
    public partial class Staffs
    {
        public Staffs()
        {
            Experiences = new HashSet<Experiences>();
        }

        public int Sid { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int? Age { get; set; }
        public string Post { get; set; }
        public string School { get; set; }
        public string Tel { get; set; }
        public DateTime? Birthday { get; set; }
        public string Education { get; set; }
        public string Specialty { get; set; }
        public string Address { get; set; }
        public short? Image { get; set; }

        public virtual ICollection<Experiences> Experiences { get; set; }
    }
}
