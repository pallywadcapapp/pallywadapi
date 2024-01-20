using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class BankDeposit : BaseModel
    {
        public string depositId { get; set; }
        [Required]
        public double amount { get; set; }
        public string otherdetails { get; set; }
        public double? loanDeductAmount { get; set; }
        public string loanRefId { get; set; }
        [Required]
        public string memberId { get; set; }
        public string fullname { get; set; }
        public string status { get; set; }
        public string processState { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime requestDate { get; set; }
        public DateTime? approvalDate { get; set; }
        public string approvedBy { get; set; }
        public string postedBy { get; set; }
        public string channel { get; set; }
    }
}
