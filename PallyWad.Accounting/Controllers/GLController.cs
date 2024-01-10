using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Accounting.Helpers;
using PallyWad.Domain;
using PallyWad.Services;

namespace PallyWad.Accounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GLController : ControllerBase
    {
        private readonly ILogger<GLController> _logger;
        private readonly IGlAccountTransService _glAccountTransService;
        private readonly IGlAccountService _glAccountService;
        private readonly IPaymentService _paymentService;
        private readonly IJournalService _journalService;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly NumberSystem _numberSystem;
        public GLController(ILogger<GLController> logger, IGlAccountTransService glAccountTransService,
        IPaymentService paymentService, IJournalService journalService, IGlAccountService glAccountService,
         ISmtpConfigService smtpConfigService) 
        {
            _logger = logger;
            _glAccountTransService = glAccountTransService;
            _paymentService = paymentService;
            _journalService = journalService;
            _glAccountService = glAccountService;
            _numberSystem = new NumberSystem();
        }

        [HttpGet]
        [Route("AllChart")]
        public IActionResult ListAllCharts()
        {
            return Ok(_glAccountService.GetAllGlAccounts());
        }

        [HttpGet]
        [Route("Today")]
        public IActionResult GetMemberDetails(string date)
        {
            var mdate = DateTime.Parse(date);
            var result = _glAccountTransService.GetAllGlCompAccounts().Where(u => u.transdate == mdate);
            return Ok(result);
        }

        [HttpGet]
        [Route("MemberPosition")]
        public IActionResult GetMemberPositionDetails(string accountno, string loanNo)
        {
            //var mdate = DateTime.Parse(date);
            var result = _glAccountTransService.GetMemberAccPosition(accountno, loanNo);
            return Ok(result);
        }

        [HttpGet]
        [Route("AccountTrans")]
        public IActionResult GetAccountTrans(string accountno)
        {
            var result = _glAccountTransService.GetAllTransactions(accountno);
            return Ok(result);
        }

        [HttpGet]
        [Route("acctransbyId")]
        public IActionResult GetAccTransById(string id)
        {
            var trans = _glAccountTransService.GetGlAccount(id);
            return Ok(trans);

        }

        [HttpGet]
        [Route("acctransbyRef")]
        public IActionResult GetAccTransByRef(string id)
        {
            var trans = _glAccountTransService.GetGlAccountByRef(id);
            return Ok(trans);

        }

        [HttpGet]
        [Route("ledgers")]
        public IActionResult GetAccountTrans(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            IEnumerable<string> values = new List<string>();
            //this.Request.Headers.TryGetValues("Authorization", out values);
            //values = Request.GetIEnumerableHeader("Authorization"); 
            return Ok(_glAccountTransService.GetGeneralLedgers(startdate, enddate).OrderByDescending(u => u.transdate).OrderByDescending(u => u.Id));
        }

        [HttpGet]
        [Route("internalacc")]
        public IActionResult GetInternalAccountTrans(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            IEnumerable<string> values = new List<string>();
            //this.Request.Headers.TryGetValues("Authorization", out values);
            //values = Request.GetIEnumerableHeader("Authorization"); 
            return Ok(_glAccountTransService.GetInternalAccountsLedgers(startdate, enddate).OrderByDescending(u => u.transdate).OrderByDescending(u => u.Id));
        }

        [HttpGet]
        [Route("savingsledgers")]
        public IActionResult GetSavingsAccount(string startdate, string enddate)
        {
            var result = _glAccountTransService.GetGeneralLedgers(startdate, enddate).OrderByDescending(u => u.transdate)
                .Where(u => u.chequeno == "Normal")
                .OrderByDescending(u => u.transdate);
            return Ok(result);
        }

        [HttpGet]
        [Route("loanledgers")]
        public IActionResult GetLoanAccount(string startdate, string enddate)
        {
            var result = _glAccountTransService.GetGeneralLedgers(startdate, enddate).OrderByDescending(u => u.transdate)
                .Where(u => u.chequeno == "Loans")
                .OrderByDescending(u => u.transdate);
            return Ok(result);
        }

        [HttpGet]
        [Route("trialbalance")]
        public IActionResult GetTrialBalance(string year)
        {
            var result = _glAccountTransService.GetTrialBalance(year).OrderByDescending(u => u.acc_name);
            return Ok(result);
        }

        [HttpGet]
        [Route("glcodeTransactions")]
        public IActionResult GetGlcodeTransactions(string code, string startdate, string enddate)
        {
            var value = _glAccountTransService.GetGeneralLedgers(startdate, enddate).Where(u => u.accountno == code).OrderByDescending(u => u.transdate).ToList();
            return Ok(value);
            //return Ok(_generalLedgerService.GetTransactions(startdate, enddate));
        }


        [HttpGet]
        [Route("PV")]
        public IActionResult GetPV(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _paymentService.ListPaymentByDate(startdate, enddate).Where(u => u.approvalStatus == false).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);

        }

        [HttpGet]
        [Route("PostedPV")]
        public IActionResult GetPostedPV(string startdate, string enddate)
        {

            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _paymentService.ListPaymentByDate(startdate, enddate).Where(u => u.approvalStatus == true).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);

        }

        [HttpGet]
        [Route("RejectedPostings")]
        public IActionResult GetRejectedPostings(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var _value = _paymentService.ListPaymentByDate(startdate, enddate);
            var value = _value.Where(u => u.approvalStatus == true && u.approvalForm == "DECLINED" && u.posterEmail == princ.Identity.Name).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);

        }

        [HttpGet]
        [Route("PVById")]
        public IActionResult GetPVById(string id)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _paymentService.GetPaymentById(id);
            return Ok(value);

        }

        [HttpGet]
        [Route("JVById")]
        public IActionResult GetJVById(string id)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _journalService.GetJournalById(id);
            return Ok(value);

        }

        [HttpGet]
        [Route("GLById")]
        public IActionResult GetGLById(string id)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _glAccountTransService.GetGlAccount(id);
            return Ok(value);

        }

        [HttpGet]
        [Route("JournalById")]
        public IActionResult GetJouralById(string id)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var value = _journalService.GetJournalById(id);
            return Ok(value);

        }


        [HttpGet]
        [Route("Journal")]
        public IActionResult GetJournal(string startdate, string enddate)
        {
            var value = _journalService.ListJournalByDate(startdate, enddate).Where(u => u.approvalStatus == false).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);
        }

        [HttpGet]
        [Route("PostedJournal")]
        public IActionResult GetPostedJournal(string startdate, string enddate)
        {
            var value = _journalService.ListJournalByDate(startdate, enddate).Where(u => u.approvalStatus == true).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);
        }

        [HttpGet]
        [Route("Journals")]
        public IActionResult GetJournals(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            //var value = _generalLedgerService.GetGeneralLedgers(startdate, enddate).Where(u => u.sbu != "TECHNICAL").OrderByDescending(u => u.vgl_dt).ToList();
            //return Ok(value);
            return Ok(_glAccountTransService.GetGeneralLedgers(startdate, enddate).OrderByDescending(u => u.transdate).ToList());
        }

        [HttpGet]
        [Route("RejectedJournals")]
        public IActionResult GetRejectedJournals(string startdate, string enddate)
        {
            var princ = HttpContext.User;
            var memId = princ.Identity.Name;
            var _value = _journalService.ListJournalByDate(startdate, enddate);
            var value = _value.Where(u => u.approvalStatus == true && u.approvalForm == "DECLINED" && u.posterEmail == princ.Identity.Name).OrderByDescending(u => u.transDate).ToList();
            return Ok(value);

        }

        [HttpPost]
        [Route("pv")]
        public IActionResult PostPV(Payment pv)
        {
            var amount = pv.amount;
            pv.amount = amount;
            var sub = 30 * -1;
            var minDate = DateTime.Now.AddDays(sub);
            if (pv.transDate < minDate || pv.transDate > DateTime.Now)
            {
                return BadRequest("Posting date not within range");
            }
            pv.year = ((DateTime)pv.transDate).Year;
            pv.postedDate = DateTime.Now;
            try
            {
                var voucherNo = _numberSystem.getVoucherNo(0);
                pv.voucherNo = voucherNo;
                _paymentService.AddPayment(pv);
                return Ok(pv);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("Journal")]
        public IActionResult PostJournals(PV pv)
        {
            //var sub = JVBound * -1;
            var sub = 30 * -1;
            var minDate = DateTime.Now.AddDays(sub);
            if (pv.transDate < minDate || pv.transDate > DateTime.Now)
            {
                return BadRequest("Posting date not within range");
            }
            pv.year = ((DateTime)pv.transDate).Year;
            pv.postedDate = DateTime.Now;
            try
            {
                var voucherNo = _numberSystem.getVoucherNo(1);
                pv.voucherNo = voucherNo;
                var journal = ConvertJournal(pv);
                _journalService.AddJournal(journal);
                //generalLedger.vgl_no = getJournalVoucher();
                return Ok(pv);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ApproveJV")]
        public IActionResult ApproveJV(Journal journal)
        {
            var _pv = PV2GLConverter(journal);
            try
            {
                foreach (var p in _pv)
                {
                    _glAccountTransService.AddGlAccount(p);
                }
                //var payment = ConvertPayment(pv);
                journal.approvalStatus = true;
                journal.approvalForm = "APPROVED";
                _journalService.UpdateJournal(journal);
                //generalLedger.vgl_no = getJournalVoucher();
                return Ok(journal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeclineJV")]
        public IActionResult DeclineJV(Journal journal)
        {
            try
            {

                //generalLedger.vgl_no = getJournalVoucher();

                string staus = "DECLINED";
                journal.approvalStatus = true;
                journal.approvalForm = "DECLINED";
                _journalService.UpdateJournal(journal);
                //generalLedger.vgl_no = getJournalVoucher();
                string message = "The following Journal posting with details: " + journal.voucherNo + " has been declined due to the following reasons: " + journal.reason;
                //PolicySettings<Journal>.SendPostingNotification(journal.posterEmail, staus, journal.postedBy, message, "", _smtpConfigService);

                return Ok(journal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Financeadmin")]
        [Route("ApprovePV")]
        public IActionResult ApprovePV(Payment payment)
        {
            var _pv = PV2GLConverter(payment);
            try
            {
                foreach (var p in _pv)
                {
                    _glAccountTransService.AddGlAccount(p);
                    //_generalLedgerService.AddLedger(p);
                }
                //var payment = ConvertPayment(pv);
                payment.approvalStatus = true;
                payment.approvalForm = "APPROVED";
                _paymentService.UpdatePayment(payment);
                //generalLedger.vgl_no = getJournalVoucher();
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Financeadmin")]
        [Route("DeclinePV")]
        public IActionResult DeclinePV(Payment payment)
        {
            try
            {
                string staus = "DECLINED";
                payment.approvalStatus = true;
                payment.approvalForm = staus;
                _paymentService.UpdatePayment(payment);
                //generalLedger.vgl_no = getJournalVoucher();
                string message = "The following posting with details: " + payment.voucherNo + " has been declined due to the following reasons: " + payment.reason;
                //PolicySettings<Payment>.SendPostingNotification(payment.posterEmail, staus, payment.postedBy, message, "", _smtpConfigService);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpOptions("getGLDetails")]
        public string getGLDetails(string glcode)
        {
            return _glAccountService.GetChartByGL(glcode).shortdesc;
            //return _chartsOfAccService.GetChartByGL(glcode).account_name;
        }

        [HttpOptions("PV2GLConverters")]
        public GL PV2GLConverter(PV pv, string glcode, string slide)
        {
            GL gl = new GL();
            double credit = 0.00;
            double debit = 0.00;

            if (slide.Equals("CR"))
            {
                credit = pv.amount;
            }
            else
            {
                debit = pv.amount;
            }
            gl.acc_name = getGLDetails(glcode);
            gl.balance = debit - credit;
            gl.batchno = pv.chequeNo;
            gl.chequeno = pv.transType; // PV or JV
            gl.creditamt = credit;
            gl.debitamt = debit;
            gl.month = pv.transDate.Month.ToString();
            gl.ref_trans = pv.chequeNo;
            gl.accountno = glcode;
            gl.description = pv.cashcodeDesc;
            gl.userid = pv.receivedFrom;
            gl.refnumber = pv.voucherNo;
            gl.transdate = pv.transDate;
            gl.year = pv.transDate.Year;
            return gl;
        }

        [HttpOptions("PV2GLConverter")]
        public List<GL> PV2GLConverter(PV pv)
        {
            GL crGL = new GL();
            GL drGL = new GL();

            var cr = pv.credit;
            var dr = pv.debit;

            crGL = PV2GLConverter(pv, cr, "CR"); //"PAYMENT VOUCHER"
            drGL = PV2GLConverter(pv, dr, "DR"); //"PAYMENT VOUCHER"
            List<GL> gl = new List<GL>();
            gl.Add(drGL);
            gl.Add(crGL);
            return gl;
        }


        private Journal ConvertJournal(PV pv)
        {
            Journal journal = new Journal();
            journal.accCode = pv.accCode;
            journal.amount = pv.amount;
            journal.branch = pv.branch;
            journal.cashcodeDesc = pv.cashcodeDesc;
            journal.ccenter = pv.ccenter;
            journal.chequeNo = pv.chequeNo;
            journal.credit = pv.credit;
            journal.debit = pv.debit;
            journal.postedBy = pv.postedBy;
            journal.receivedFrom = pv.receivedFrom;
            journal.transDate = pv.transDate;
            journal.transType = pv.transType;
            journal.type = pv.type;
            journal.voucherNo = pv.voucherNo;
            journal.year = pv.year;
            journal.postedDate = pv.postedDate;
            return journal;
        }
    }
}
