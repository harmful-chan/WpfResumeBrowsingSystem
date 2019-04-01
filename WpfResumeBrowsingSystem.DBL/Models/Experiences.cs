using System;

namespace WpfResumeBrowsingSystem.DBL.Models
{
    public class Experiences
    {
        public int Eid { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Location { get; set; }

        public string CompanyName { get; set; }

        public int Staff_Sid { get; set; }

    }
}
