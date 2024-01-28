using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class LoanRequestDto
    {
        //public string loanId { get; set; }
        public string loancode { get; set; }
        public List<string> collateralRefId { get; set; }
        //public string memberId { get; set; }
        [Required]
        public double amount { get; set; }
        //public string status { get; set; }
        //public string processState { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime requestDate { get; set; }
        //public DateTime? approvalDate { get; set; }
        //public string approvedBy { get; set; }
        //public string postedBy { get; set; }
        //public int? duration { get; set; }
        //public double? loaninterest { get; set; }
        public string? category { get; set; }
        public List<string> documentIdRefs { get; set; }
        //public double processingFee { get; set; }
    }
}
