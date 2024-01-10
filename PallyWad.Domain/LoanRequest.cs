using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class LoanRequest : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string loanId { get; set; }
        public string loancode { get; set; }
        public string memberId { get; set; }
        public string bvn { get; set; }
        public string bankname { get; set; }
        public string bankaccountno { get; set; }
        [Required]
        public double amount { get; set; }
        public double? savBal { get; set; }
        public double? loanBal { get; set; }
        public double? netBal { get; set; }
        public double? monthtotalrepay { get; set; }
        public double? monthlyrepay { get; set; }
        public double? monthlyendsalary { get; set; }
        public double? monthlnetsalary { get; set; }
        public string status { get; set; }
        public string processState { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime requestDate { get; set; }
        public DateTime? approvalDate { get; set; }
        public string approvedBy { get; set; }
        public string postedBy { get; set; }
        public int? duration { get; set; }
        public double? loaninterest { get; set; }
        public string category { get; set; }
        public double processingFee { get; set; }
    }
}
