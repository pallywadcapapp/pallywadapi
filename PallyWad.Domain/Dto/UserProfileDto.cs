using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class UserProfileDto
    {
        public string memberid { get; set; }
        public DateTime dob { get; set; }
        public string bvn { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string othernames { get; set; }
        public string? sex { get; set; }
        public string email { get; set; }
        //[JsonIgnore]
        //public virtual AppIdentityUser AppIdentityUser { get; set; } = new AppIdentityUser();
    }
}
