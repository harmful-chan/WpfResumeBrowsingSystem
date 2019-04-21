using System;
using System.Collections.Generic;

namespace WpfResumeBrowsingSystem.Domain.Models
{
    public partial class UserInfo
    {
        public int Uid { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
