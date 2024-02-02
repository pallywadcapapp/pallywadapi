using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using System.Globalization;
using System.Net.Mail;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankDepositController : ControllerBase
    {
        private readonly IBankDepositService _bankDepositService;
        private readonly IGlAccountService _glAccountService;
        private readonly IGlAccountTransService _glAccountTransService;
        private readonly ILoanRepaymentService _loanRepaymentService;
        private readonly ILoanTransService _loanTransService;
        private readonly IMembersAccountService _membersAccountService;
        private readonly ILoanSetupService _loanSetupService;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        GLPostingRepository gLPostingRepository;
        public BankDepositController(IUserService userService, IBankDepositService bankDepositService,
            IGlAccountService glAccountService, IGlAccountTransService glAccountTransService,
            ILoanRepaymentService loanRepaymentService, ILoanTransService loanTransService,
            IMembersAccountService membersAccountService, ILoanSetupService loanSetupService, ISmtpConfigService smtpConfigService,
             IConfiguration config) 
        {
            _bankDepositService = bankDepositService;
            //_memberService = memberService;
            _glAccountService = glAccountService;
            _glAccountTransService = glAccountTransService;
            _loanRepaymentService = loanRepaymentService;
            _loanTransService = loanTransService;
            _membersAccountService = membersAccountService;
            _loanSetupService = loanSetupService;
            _smtpConfigService = smtpConfigService;
            _userService = userService;


            gLPostingRepository = new GLPostingRepository(_glAccountTransService);
            _config = config;
        }

        #region get

        [HttpGet, Route("Deposits")]
        public IActionResult Get()
        {
            var result = _bankDepositService.ListBankDeposits().OrderByDescending(u => u.requestDate);
            return Ok(result);
        }

        [HttpGet, Route("depositDetails")]
        public IActionResult GetDepositDetails(int id)
        {
            var result = _bankDepositService.GetBankDeposits(id);
            return Ok(result);
        }

        [HttpGet, Route("LoanAcc")]
        public string GetAccNo(string loanrefno, string memberId)
        {
            var result = _membersAccountService.GetLoanTransMemberAcc(memberId, loanrefno).accountno;
            return result;
        }
        #endregion

        #region post
        [HttpPost, Route("Deposit")]
        public IActionResult PosttDeposit([FromBody] BankDeposit deposit)
        {
            deposit.requestDate = DateTime.Now;
            _bankDepositService.AddBankDeposit(deposit);
            return Ok(deposit);
        }

        private string loanCategory(string loanCode)
        {
            var loan = _loanSetupService.GetLoanSetup(loanCode);
            if (loan == null)
            {
                return "LOANS";
            }
            else
            {

                var result = loan.category;
                return result;
            }
        }

        [HttpPost, Route("depositdecline")]
        public IActionResult DeclineDeposit(BankDeposit _deposit)
        {
            var deposit = _bankDepositService.GetBankDeposits(_deposit.Id);
            deposit.status = "Declined";
            deposit.processState = "Processed";
            deposit.approvalDate = DateTime.Now;

            _bankDepositService.UpdateBankDeposit(deposit);
            return Ok(deposit);
        }

        
        [HttpPost, Route("processDeposit")]
        public async Task<IActionResult> ProcessDeposit(BankDeposit deposit)
        {
            var princ = HttpContext.User;
            var pid = princ.Identity.Name;

            //var header = _heaerService.Listheaders().FirstOrDefault();

            var bankAcc = _glAccountService.GetAccByName("BANK ACCOUNT");
            var intAcc = _glAccountService.GetAccByName("INTEREST RECEIVED (MEMBER WITHDRAWAL)");
            var member = _userService.GetUser(deposit.memberId); //_memberService.Getmember(deposit.MemberId);
            DateTime endOfMonth = deposit.approvalDate.Value; //DateTime.Now;
            var fullname = member.lastname + " " + member.firstname + " " + member.othernames;

            //var interestRate = header.savingsIntRate;
            //double interest = fundsRequest.amount * interestRate / 100;

            //var glref = gLPostingRepository.PostMemberSaving(Convert.ToDecimal(deposit.amount), member.Accountno,
            //   bankAcc.accountno, member.Memberid, "(" + pid + ")", "", endOfMonth, fullname);
            if (deposit.loanDeductAmount > 0)
            {

                try
                {
                    var loan = _loanTransService.ListLoanHistory(deposit.memberId).Where(u => u.repay == 1 && u.loancode == deposit.loanRefId).FirstOrDefault();//GetLoanTrans(deposit.loanRefId);
                    var category = loanCategory(loan.loancode);
                    var repayment = _loanRepaymentService.GetLoanRepayment(loan.loanrefnumber);
                    if (repayment != null)
                    {

                        var loanAccNo = GetAccNo(loan.loanrefnumber, member.UserName);
                        var deductloan = gLPostingRepository.PostPaymentToLoan((deposit.loanDeductAmount??0),
                        bankAcc.accountno, loanAccNo, member.UserName, "(" + pid + ")", "", endOfMonth, fullname, category);
                        var repay = repayment.loanamount - repayment.repayamount;
                        if (repay > 0)
                        {
                            double loanAmount = repayment.loanamount - (deposit.loanDeductAmount??0);
                            double repayAmount = (deposit.loanDeductAmount??0);

                            lodgeLoanDeductions(deposit, repayment, member.UserName, pid, loanAmount, repayAmount);
                        }
                        else
                        {
                            // update loan as fully paid
                        }

                    }
                    else
                    {
                        //loan repayment record does not exist

                    }
                }
                catch (Exception e)
                {

                }

                //lodgeLoanDeductions(deposit, loan, tenantId, member, id);
            }
            //PostSavingsProcessedForMemberAccount(deposit, glref, glref, pid, member.Fullname);
            //PostFundsWithdrawalInterestProcessedForMemberAccount(fundsRequest.memberId, interest, intglref, intglref, tenantId, id);
            await UpdateRequest(deposit.Id, pid);
            return Ok(deposit);
        }

        [HttpPost, Route("fundrequestdecline")]
        public async Task<IActionResult> ProcessFundRequest(int id)
        {
            var princ = HttpContext.User;
            var user = princ.Identity.Name;
            var fr = _bankDepositService.GetBankDeposits(id);
            fr.status = "Declined";
            fr.processState = "Declined";
            fr.approvedBy = user;
            fr.approvalDate = DateTime.Now;

            _bankDepositService.UpdateBankDeposit(fr);


            var member = _userService.GetUser(fr.memberId);
            string message = "Dear " + member.lastname + ", <br/>" +
                " We wish to notify you that we are unable to process your fund(s) deposit and " +
                " has been declined. <br /> Fund Amount: " +
                GetFormattedCurrency(Convert.ToDecimal(fr.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ".";

            string org ="";
            try
            {
                await SendEmailNotification(member.UserName, message, "Fund Deposit (Declined)", org);
            }
            catch
            {

            }
            return Ok(fr);
        }
        #endregion

        [HttpPut, Route("ApproveDeposit")]
        public IActionResult UpdateDeposit(int id)
        {
            var deposit = _bankDepositService.GetBankDeposits(id);
            deposit.status = "Approved";
            deposit.approvalDate = DateTime.Now;

            return Ok(deposit);
        }


        async Task UpdateRequest(int id, string user)
        {
            var fr = _bankDepositService.GetBankDeposits(id);
            fr.status = "Approved";
            fr.processState = "Processed";
            fr.approvedBy = user;
            fr.approvalDate = DateTime.Now;

            _bankDepositService.UpdateBankDeposit(fr);

            var member = _userService.GetUser(fr.memberId);
            string message = "Dear " + member.lastname + ", <br/>" +
                " We wish to notify you that your fund deposit " +
                " has been received and processed. <br /> Fund Amount: " +
                GetFormattedCurrency(Convert.ToDecimal(fr.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ".";

            string org = "";
            try
            {
                await SendEmailNotification(member.UserName, message, "Fund Deposit (Approved)", org);
            }
            catch
            {

            }
        }

        private void lodgeLoanDeductions(BankDeposit mntly, LoanRepayment loan, string memberId, string postedBy, double loanAmount, double repayAmount)
        {

            var loanrepay = new LoanRepayment() { };
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            //loanrepay.branchname = member.Branchname;
            loanrepay.description = loan.description + " REPAYMENT OF "+ mntly.loanRefId +" IN THE MONTH OF " +
             CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " " + DateTime.Now.Year;
            loanrepay.interestamt = loan.interestamt;
            loanrepay.memberid = memberId;
            loanrepay.loanamount = loanAmount; //  Convert.ToDecimal(mntly.loanDeductAmount);
            loanrepay.loancode = loan.loancode;
            loanrepay.loanrefnumber = loan.loanrefnumber;
            loanrepay.repayamount = repayAmount; //loan.Repayamount;
            loanrepay.repayrefnumber = "LRPY/" + refno;
            loanrepay.transdate = DateTime.Now;
            loanrepay.transmonth = (DateTime.Now.Month);
            loanrepay.transyear = (DateTime.Now.Year);

            _loanRepaymentService.AddLoanRepayment(loanrepay);


        }


        private string GetFormattedCurrency(decimal value, int decimalPlaces, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            return value.ToString($"C{decimalPlaces}", cultureInfo);
        }
        private async Task SendEmailNotification(string receiverId, string message, string subject, string organisation)
        {
            var receiver = _userService.GetUser(receiverId);
            var mailkey = _config.GetValue<string>("AppSettings:DefaultMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            //var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == "mailconfig").FirstOrDefault();
            using (var client = new SmtpClient()
            {
                Port = mailConfig.port,
                Credentials = new System.Net.NetworkCredential(mailConfig.username, mailConfig.password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = mailConfig.isSSL,
                Host = mailConfig.smtp

            })
            using (var mail = new System.Net.Mail.MailMessage())
            {
                mail.To.Add(receiver.Email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(organisation + " <" + mailConfig.username + ">");
                //try
                //{
                await client.SendMailAsync(mail);

            }
        }
    }
}
