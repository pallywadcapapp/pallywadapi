using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class UserProfile: BaseModel
    {
        public string memberid { get; set; }
        public DateTime? dob { get; set; }
        public string bvn { get; set; }
        public string address { get; set; }
        //[JsonIgnore]
        //public virtual AppIdentityUser AppIdentityUser { get; set; }
    }
    public class AppsRole
    {
        public string users { get; set; }
        public string userId { get; set; }
        public string role { get; set; }
        public string roleId { get; set; }
    }
}
