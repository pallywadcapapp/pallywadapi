using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        public readonly ILoanTransService _loanTransService;
        private readonly ILoanRepaymentService _loanRepaymentService;
        private readonly IUserService _userService;
        private readonly ILoanSetupService _loanSetupService;
        private readonly IMembersAccountService _membersAccountService;
        private readonly GLPostingRepository gLPostingRepository;
        private readonly IGlAccountTransService _glAccountTransService;
        private readonly IGlAccountService _glAccountService;
        public LoansController(ILoanTransService loanTransService, ILoanRepaymentService loanRepaymentService, IUserService userService,
            ILoanSetupService loanSetupService, IGlAccountTransService glAccountTransService, IMembersAccountService membersAccountService,
            IGlAccountService glAccountService)
        {
            _loanTransService = loanTransService;
            _loanRepaymentService = loanRepaymentService;
            _userService = userService;
            _loanSetupService = loanSetupService;
            _membersAccountService = membersAccountService;
            _glAccountTransService = glAccountTransService;
            _glAccountService = glAccountService;
            gLPostingRepository = new GLPostingRepository(_glAccountTransService);

        }

        #region Get
        [HttpGet]
        [Route("LoanTransactions")]
        public IActionResult GetLoanTransactions(DateTime startdate, DateTime enddate)
        {
            var loans = _loanTransService.GetAllLoanTrans()
            .Where(u => u.repay == 1 && (u.transdate >= startdate && u.transdate <= enddate))
            .OrderByDescending(u => u.transdate);
            return Ok(loans);

        }

        [HttpGet]
        [Route("loanrepaymentsByDate")]
        public IActionResult GetAllLoanRepayments(DateTime start, DateTime end)
        {
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.transdate >= start && u.transdate <= end)
                .OrderByDescending(x => x.transdate);
            return Ok(result);
        }

        [HttpGet]
        [Route("LoanTransDetail")]
        public IActionResult GetLoanTransDetail(string loanId)
        {
            var loans = _loanTransService.GetAllLoanTrans()
            .Where(u => u.loanrefnumber == loanId)
            .OrderByDescending(u => u.transdate).FirstOrDefault();
            return Ok(loans);

        }

        [HttpGet]
        [Route("ActiveLoan")]
        public IActionResult GetctiveLoanHistory(string memberId)
        {
            var princ = HttpContext.User;
            var id = princ.Identity.Name;
            var loanHistory = _loanTransService.ListLoanHistory(memberId)
                .Where(u => u.repay == 1)
            .OrderByDescending(u => u.transdate);
            return Ok(loanHistory);
        }
        #endregion

        #region Post

        [HttpPost, Route("ProcessLoanPayment")]
        public IActionResult PostLoanProcess([FromBody] DeductionDto savingsDeduction)
        {
            var princ = HttpContext.User;
            var pid = princ.Identity.Name;

            var amt = (savingsDeduction.amount);

            var repayment = _loanRepaymentService.GetLoanRepayment(savingsDeduction.loanRef);
            if (repayment == null)
            {
                var currLoan = _loanTransService.GetLoanTransByRef(savingsDeduction.loanRef);
                repayment = new LoanRepayment()
                {
                    description = currLoan.description,
                    interestamt = currLoan.interestamt,
                    //branchname = currLoan.Branchname,
                    loanamount = currLoan.totrepayable,
                    loancode = currLoan.loancode,
                    loanrefnumber = currLoan.loanrefnumber,
                    memberid = currLoan.memberid,
                    repayamount = currLoan.repayamount,
                    repayrefnumber = "",
                    transdate = DateTime.Now,
                    transmonth = DateTime.Now.Month,
                    transyear = DateTime.Now.Year,
                    Id = 0
                    //Updated = 1
                };
            }
            var lamt = repayment.loanamount;
            if (amt > lamt)
            {
                return BadRequest("amount payable exceeds outstanding loan amount");
            }

            var member = _userService.GetUserByEmail(savingsDeduction.memberId);
            var loanSetup = _loanSetupService.GetLoanSetup(repayment.loancode);
            var accno = "";
            var fullname = member.lastname + " " + member.firstname + " " + member.othernames;
            var category = loanSetup.category;

            var pp = _membersAccountService.ListMembersAccounts(savingsDeduction.memberId).Where(u => u.transtype == category).FirstOrDefault();

            if (pp == null)
            {
                return BadRequest("Member " + fullname + " does not have a " + category + " account number set up");
            }
            else
            {
                accno = pp.accountno;
            }

            DateTime endOfMonth = DateTime.Now;
            /*var deductloan = gLPostingRepository.PostSavingToLoan(amt,
            member.Accountno, accno, member.UserName, "(" + pid + ")", "", endOfMonth, fullname, category);
            double loanAmount = repayment.loanamount - amt;
            double repayAmount = amt;

            lodgeLoanDeductions(savingsDeduction, repayment, member, pid, loanAmount, repayAmount);
            if (savingsDeduction.chargesAmount > 0)
            {
                var bankAcc = _glAccountService.GetAccByName("BANK ACCOUNT");
                var chargeDeduct = gLPostingRepository.PostDeductChargesMemberSaving(Convert.ToDecimal(savingsDeduction.chargesAmount),
            member.Accountno, bankAcc.accountno, member.UserName, "(" + pid + ")", "", endOfMonth, fullname);

                PostLoanInterestDeductionProcessedForMemberAccount(member.UserName, savingsDeduction.chargesAmount, chargeDeduct,
                    "", pid, fullname);
            }*/
            return Ok(savingsDeduction);
        }
        #endregion

        #region Put


        [Authorize]
        [HttpPost("loanclearance")]
        public IActionResult PostLoanClearance(LoanTrans loanTrans)
        {
            _loanTransService.UpdateLoanTrans(loanTrans);
            return Ok(loanTrans);
        }
        #endregion
    }
}
