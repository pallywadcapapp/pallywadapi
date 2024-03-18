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
        public double? monthtotalrepay { get; set; }
        public double? monthlyrepay { get; set; }
        public double? monthlyendsalary { get; set; }
        public double? monthlnetsalary { get; set; }
        public string status { get; set; }
        public string processState { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime requestDate { get; set; } = DateTime.Now;
        public DateTime? approvalDate { get; set; }
        public DateTime? collaterizedDate { get; set; }
        public DateTime? processedDate { get; set; }
        public string approvedBy { get; set; }
        public string postedBy { get; set; }
        public int? duration { get; set; }
        public double? loaninterest { get; set; }
        public string category { get; set; }
        public double processingFee { get; set; }
        public string collateralId { get; set; }
        public string collateral { get; set; }
        public virtual ICollection<LoanUserCollateral> loanUserCollaterals { get; set; }
        public virtual ICollection<LoanUserDocument> loanUserDocuments { get; set; }
        public string? reason { get; set; }
        public string purpose { get; set; }
        public string? age { get; set; }
        public string? sector { get; set; }
        public string? businessname { get; set; }
        public double preferredRate { get; set; }
		public int preferredTenor { get; set; }
		public double? loanmonthlyinterest { get; set; }
        public bool runningState { get; set; }

        public bool isDocmentProvided { get; set; }
        public bool isCollateralReceived { get; set; }
        public bool isProcessCleared { get; set; }
        public string? admiDocumentRef { get; set; }
        public double? collateralValue { get; set; }
        public double? estimatedCollateralValue { get; set; }
    }
}
