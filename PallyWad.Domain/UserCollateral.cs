using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class UserCollateral: BaseModel
    {
        public string colleteralId { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public double estimatedValue { get; set; }
        public double approvedValue { get; set; }
        public string otherdetails { get; set; }
        public string loanRefId { get; set; }
        public bool verificationStatus { get; set; }
        public string url { get; set; }
    }
}
