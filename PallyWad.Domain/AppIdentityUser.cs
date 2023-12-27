using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class AppIdentityUser: IdentityUser
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string othernames { get; set; }
        public string sex { get; set; }
        public string type { get; set; }
        public virtual ICollection<MemberAccount> account { get; set; }
    }
}
