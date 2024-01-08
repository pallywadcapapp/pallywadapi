using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class AppIdentityUser: IdentityUser
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string othernames { get; set; }
        public string? sex { get; set; }
        public string type { get; set; }
        public virtual ICollection<MemberAccount> account { get; set; }
        //[JsonIgnore]
        //public virtual UserProfile UserProfile { get; set; }
        public DateTime? dob { get; set; }
        public string bvn { get; set; }
        public string address { get; set; }
    }
}
