using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class PV : BaseModel
    {
        public string voucherNo { get; set; }
        public int year { get; set; }
        public DateTime transDate { get; set; }
        public string receivedFrom { get; set; }
        public string cashcodeDesc { get; set; }
        public string chequeNo { get; set; }
        public string ccenter { get; set; }
        public string type { get; set; }
        public string accCode { get; set; }
        public string debit { get; set; }
        public string credit { get; set; }
        public string transType { get; set; }
        public string branch { get; set; }
        public double amount { get; set; }
        public string postedBy { get; set; }
        public DateTime postedDate { get; set; }
        public bool approvalStatus { get; set; }
        public string approvalForm { get; set; }
        public string posterEmail { get; set; }
        public string approvedBy { get; set; }
        public string approvedByEmail { get; set; }
        public string reason { get; set; }
        public string bank { get; set; }
    }
}
