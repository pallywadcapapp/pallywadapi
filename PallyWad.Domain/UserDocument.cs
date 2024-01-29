using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class UserDocument: BaseModel
    {
        public string documentRefId { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string documentNo { get; set; }
        public DateTime? expiryDate { get; set; }
        public bool status { get; set; } // approved/active or not approved
        public string doctype { get; set; }
    }
}
