﻿using System;
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
        public string? category { get; set; }
        public List<string> documentIdRefs { get; set; }
        public string? age { get; set; }
        public string purpose { get; set; }
        public string? businessname { get; set; }
        public string? sector { get; set; }
        public double preferredRate { get; set; }
		public int preferredTenor { get; set; }
		
		public string collateral { get; set; }
        public string estimatedCollateralValue { get; set; }
        public string repaymentPlan { get; set; }
    }

    public class LoanRequestVM
    {
        public int Id { get; set; }
        public string loanId { get; set; }
        public string loanCode { get; set; }
        public string memberId { get; set; }
        public string bvn { get; set; }
        public string bankname { get; set; }
        public string bankaccountno { get; set; }
        [Required]
        public double amount { get; set; }
        public double othercoorp { get; set; }
        public double? monthtotalrepay { get; set; }
        public double? monthlyrepay { get; set; }
        public double? monthlyendsalary { get; set; }
        public double? monthlnetsalary { get; set; }
        public string? guarantorId1 { get; set; }
        public string? guarantorId2 { get; set; }
        public string? guarantorId3 { get; set; }
        public string status { get; set; }
        public string processState { get; set; }
        public DateTime? requestDate { get; set; }
        public DateTime approvalDate { get; set; }
        public DateTime? processedDate { get; set; }
        public string approvedBy { get; set; }
        public string? postedBy { get; set; }
        public int? duration { get; set; }
        public double? loaninterest { get; set; }
        public double processingFee { get; set; }
        public double? loanmonthlyinterest { get; set; }
    }
}
