using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Migrations.AccountDb;
using PallyWad.Services;
using PallyWad.Services.Repository;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRequestController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<LoanRequestController> _logger;
        private readonly ILoanSetupService _loanSetupService;
        private readonly ILoanRequestService _loanRequestService;
        private readonly IMembersAccountService _membersAccountService;
        private readonly IGlAccountService _glAccountService;
        private readonly IUserService _userService;
        private readonly IGlAccountTransService _glAccountTransService;
        private readonly ILoanTransService _loanTransService;
        private readonly IChargesService _chargesService;
        private readonly GLPostingRepository gLPostingRepository;
        private readonly IMapper _mapper;
        //private readonly ILoanCollateralService _loanCollateralService;
        private readonly IGlAccountTier3Service _glAccountTier3Service;
        private readonly ILoanRepaymentService _loanRepaymentService;

        public LoanRequestController(IHttpContextAccessor contextAccessor, ILogger<LoanRequestController> logger,
            ILoanSetupService loanSetupService, ILoanRequestService loanRequestService, IMembersAccountService membersAccountService,
            IGlAccountService glAccountService, IUserService userService, IGlAccountTransService glAccountTransService,
            ILoanTransService loanTransService, IChargesService chargesService, IMapper mapper, 
            //ILoanCollateralService loanCollateralService,
            IGlAccountTier3Service glAccountTier3Service, ILoanRepaymentService loanRepaymentService)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _loanSetupService = loanSetupService;
            _loanRequestService = loanRequestService;
            _membersAccountService = membersAccountService;
            _glAccountService = glAccountService;
            _userService = userService;
            _glAccountTransService = glAccountTransService;
            _loanTransService = loanTransService;
            _chargesService = chargesService;
            gLPostingRepository = new GLPostingRepository(_glAccountTransService);
            _mapper = mapper;
            //_loanCollateralService = loanCollateralService;
            _glAccountTier3Service = glAccountTier3Service;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _loanRequestService.GetAllLoanRequests().OrderByDescending(u=>u.requestDate);
            return Ok( new Response { Status = "success", Message = result } );
        }

        [HttpGet("byReqId")]
        public IActionResult Get(int id)
        {
            var result = _loanRequestService.GetLoanRequest(id);
            return Ok(new Response { Status = "success", Message = result });
        }

        [HttpGet]
        [Route("PendingLoanRequests")]
        public IActionResult GetPendingLoanRequests()
        {
            var member = _loanRequestService.GetAllPendingLoanRequests().OrderByDescending(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        [HttpGet]
        [Route("DeclinedLoanRequests")]
        public IActionResult GetDeclinedLoanRequests()
        {
            var member = _loanRequestService.GetAllDeclinedLoanRequests().OrderByDescending(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        [HttpGet]
        [Route("ApprovedLoanRequests")]
        public IActionResult GetApprovedLoanRequests()
        {
            var member = _loanRequestService.GetAllApprovedLoanRequests().OrderByDescending(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        [HttpGet]
        [Route("CollaterizedLoanRequests")]
        public IActionResult GetCollaterizedLoanRequests()
        {
            var member = _loanRequestService.GetAllCollaterizedLoanRequests().OrderByDescending(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        [HttpGet]
        [Route("ProcessedLoanRequests")]
        public IActionResult GetProcessedLoanRequests()
        {
            var member = _loanRequestService.GetAllProcessedLoanRequests().OrderByDescending(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        [HttpGet]
        [Route("UnProcessedLoanRequests")]
        public IActionResult GetUnProcessedLoanRequests()
        {
            var member = _loanRequestService.GetAllUnProcessedLoanRequests().OrderBy(u => u.requestDate).OrderByDescending(u => u.status);
            return Ok(member);

        }

        //[Authorize]
        [HttpGet]
        [Route("loandetail")]
        public IActionResult GetLoanDetail(string loanId)
        {
            var _result = _loanRequestService.GetAllLoanRequests();
            var result = _result.Where(u => u.loanId == loanId).FirstOrDefault();
            return Ok(result);
        }

        [HttpGet]
        [Route("accno")]
        public IActionResult GenerateAccNos(string type)
        {
            var result = GenerateAccNo(type);
            return Ok(result);
        }

        #endregion

        #region Post

        [HttpPost("Approve")]
        public IActionResult ApproveLoanRequest(LoanRequest loanRequest)
        {
            var princ = HttpContext.User;
            var id = princ.Identity.Name;
            var bankAcc = _glAccountService.GetAccByName("BANK ACCOUNT");
            var GLbankAcc = _glAccountService.GetAccByName("POLARIS");
            var loanSetup = _loanSetupService.GetLoanSetup(loanRequest.loancode);
            var category = loanSetup.category;
            var repayOrder = loanSetup.repayOrder;
            var member = _userService.GetUser(loanRequest.memberId);
            var pp = _membersAccountService.ListMembersAccounts(loanRequest.memberId).Where(u => u.transtype == category).FirstOrDefault();
            var accno = "";
            var loanInterest = loanRequest.loaninterest; //loanSetup.Loaninterest;
            var loanDuration = loanRequest.duration; //Convert.ToInt32(loanSetup.Duration);
            var processingFee = loanRequest.processingFee;
            var amount = loanRequest.amount;
            var interest = amount * Convert.ToDouble(loanInterest) / 100;
            var repayAmount = amount + interest;
            var monthlyPay = repayAmount / loanDuration;

            if (pp == null)
            {
                return BadRequest("Member " + member.lastname + " does not have a " + category + " account number set up");
            }
            else
            {
                accno = pp.accountno;
            }

            var loanTrans = new LoanTrans()
            {
                memberid = loanRequest.memberId,
                loanrefnumber = loanRequest.loanId,
                transdate = loanRequest.approvalDate??DateTime.Now, //DateTime.Now,
                repaystartdate = loanRequest.approvalDate?.AddMonths(1)?? DateTime.Now.AddMonths(1), //DateTime.Now,
                loanamount = (loanRequest.amount),
                totrepayable = (repayAmount),
                repayamount = (monthlyPay??0),
                interestamt = (interest),
                payableacctno = loanSetup.accountno,
                loaninterest = (loanInterest??0), //loanSetup.Loaninterest,
                processrate = loanSetup.processrate,
                duration = Convert.ToInt16(loanDuration), //loanSetup.Duration,
                glrefnumber = "",
                glbankaccount = GLbankAcc.accountno,
                gapproved = true,
                accountno = accno,
                description = loanSetup.loandesc,
                repay = 1,
                loancode = loanSetup.loancode,
                repayOrder = repayOrder
            };
            var glref = gLPostingRepository.PostLoanRequest((amount), (interest),
            accno, loanSetup.accountno, bankAcc.accountno, id, loanSetup.loandesc,
            "", member.lastname, loanRequest.approvalDate.GetValueOrDefault());

            if (processingFee > 0)
            {
                try
                {
                    var chargecode = _chargesService.GetCharges(loanSetup.chargecode);
                    //var procFeeAcc = _glAccountServiceD.GetAccByName(chargecode.Accountno);
                    var procFeeref = gLPostingRepository.PostProcessingFeeDeductMemberSaving((processingFee),
                accno, chargecode.accountno, id, "",
                "", loanRequest.approvalDate.GetValueOrDefault(), member.lastname);
                }
                catch
                {

                }

            }
            loanTrans.glrefnumber = glref;
            _loanTransService.AddLoanTrans(loanTrans);
            PostLoanCollaterals(loanRequest, loanRequest.collateralId);
            //PostLoanProcessedForMemberAccount(lr, glref, loanTrans.loanrefnumber, id, repayAmount, loanSetup.loandesc, member.lastname);
            updateProcessed(loanRequest.Id, id);
            return Ok(loanTrans);
        }

        [Authorize]
        [HttpPost, Route("createAccount")]
        public IActionResult createAccount([FromBody] AccSchema accSchema)
        {
            var loanType = _loanSetupService.GetLoanSetup(accSchema.name);
            var accountno = GenerateNewAccountNo(loanType.category);
            //var member = _memberService.Getmember(accSchema.memberId);
            //var suf = GetAccStr(member.Accountno, 6).ToList();
            var memberacc = new MemberAccount()
            {
                memberid = accSchema.memberId,
                deductcode = accSchema.name,
                memgroupacct = loanType.memgroupacct,
                transtype = loanType.category,
                accountno = accountno //loanType.memgroupacct + suf[1]
            };

            _membersAccountService.AddMembersAccount(memberacc);
            return Ok(memberacc);
        }
        #endregion

        #region Put
        [HttpPut]
        [Route("loanrequestapproved")]
        public async Task<IActionResult> LoanRequestApproved(int id, int duration, double interest, double processingFee)
        {
            var loan = _loanRequestService.GetLoanRequest(id);
            loan.status = "Approved";
            loan.duration = duration;
            loan.loaninterest = interest;
            loan.processingFee = processingFee;
            loan.approvalDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            _loanRequestService.UpdateLoanRequest(loan);
            /*var member = _memberService.Getmember(loan.memberId);
            string message = "Dear " + member.Fullname + ", <br/>" +
                " We wish to notify you that your loan with ref no " + loan.loanId +
                " has been approved. <br /> Loan Amount: " +
                GetFormattedCurrency(Convert.ToDecimal(loan.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ".";

            
            string org = header.Coyname;
            try
            {
                await SendEmailNotification(member.Memberid, message, "Loan Request (Approval)", org);
            }
            catch
            {

            }*/

            return Ok(loan);

        }

        [HttpPut]
        [Route("loanrequestdecline")]
        public async Task<IActionResult> LoanRequestDecline(int id, string reason)
        {
            var loan = _loanRequestService.GetLoanRequest(id);
            loan.status = "Declined";
            loan.reason = reason;
            loan.approvalDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            _loanRequestService.UpdateLoanRequest(loan);
            /*var member = _memberService.Getmember(loan.memberId);
            string message = "Dear " + member.Fullname + ", <br/>" +
                " We wish to notify you that your loan with ref no " + loan.loanId +
                " has been declined. <br /> Loan Amount: " +
                GetFormattedCurrency(Convert.ToDecimal(loan.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ".";

            var header = _tblHeaderService.Getheaders();
            string org = header.Coyname;
            try
            {
                await SendEmailNotification(member.Memberid, message, "Loan Request (Declined)", org);
            }
            catch
            {

            }*/

            return Ok(loan);

        }

        [HttpPut]
        [Route("ApproveLoanRequestCollateral")]
        public async Task<IActionResult> ApproveLoanRequestCollateral(int Id)
        {
            var princ = HttpContext.User;
            var username = princ.Identity.Name;
            var loan = _loanRequestService.GetLoanRequest(Id);
            loan.status = "Collaterized";
            loan.processState = "Collaterized";
            loan.approvalDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            loan.approvedBy = username;
            _loanRequestService.UpdateLoanRequest(loan);
            /*var member = _memberService.Getmember(loan.memberId);
            string message = "Dear " + member.Fullname + ", <br/>" +
                " We wish to notify you that your loan with ref no " + loan.loanId +
                " has been declined. <br /> Loan Amount: " +
                GetFormattedCurrency(Convert.ToDecimal(loan.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ".";

            var header = _tblHeaderService.Getheaders();
            string org = header.Coyname;
            try
            {
                await SendEmailNotification(member.Memberid, message, "Loan Request (Declined)", org);
            }
            catch
            {

            }*/

            return Ok(loan);

        }

        [Authorize]
        [HttpPut]
        [Route("ProcessLoanRequest")]
        public IActionResult ProcessLoanRequests([FromBody] LoanRequestVM lr)
        {
            var princ = HttpContext.User;
            var id = princ.Identity.Name;
            //var bankAcc = _glAccountServiceD.GetAccByName(_config["BankAccountName"]);
            var bankAcc = _glAccountService.GetAccByName("BANK ACCOUNT");
            //var GLbankAcc = _glAccountService.GetAccByName("POLARIS");
            var loanSetup = _loanSetupService.GetLoanSetup(lr.loanCode);
            //var baseSetup = _baseSettingService.GetBaseSettings(tenantId);
            var member = _userService.GetUserByEmail(lr.memberId);  //_memberService.Getmember(lr.memberId);
            var loanInterest = lr.loaninterest; //loanSetup.Loaninterest;
            var loanDuration = lr.duration; //Convert.ToInt32(loanSetup.Duration);
            var processingFee = lr.processingFee;
            var amount = lr.amount;
            var interest = amount * Convert.ToDouble(loanInterest) / 100;
            var repayAmount = amount + interest;
            var accno = "";
            var category = loanSetup.category;
            var repayOrder = loanSetup.repayOrder;

            var pp = _membersAccountService.ListMembersAccounts(lr.memberId).Where(u => u.transtype == category).FirstOrDefault();
            string fullname = member.lastname + " " + member.firstname + " " + member.othernames;
            if (pp == null)
            {
                var sfullname = member.lastname + " " + member.firstname + " " + member.othernames.Substring(0,1);
                var accSchema = new AccSchema()
                {
                    desc = $"{sfullname} ({loanSetup.category})",
                    memberId = member.UserName,
                    name = sfullname,
                    type = loanSetup.category,
                    loancode = loanSetup.loancode,
                };
                var memacc = createMemberAccount(accSchema, member.Id);
                accno = memacc.accountno;
                //return BadRequest("Member " + fullname + " does not have a " + category + " account number set up");
            }
            else
            {
                accno = pp.accountno;
            }

            /*if(category == "Long Term")
            {
                accno = member.Loanaccountno;
            }
            else
            {
                accno = member.Shortloanaccountno;
            }*/
            var monthlyPay = repayAmount / loanDuration;

            //get loan type for specific account


            var loanTrans = new LoanTrans()
            {
                memberid = lr.memberId,
                loanrefnumber = lr.loanId,
                transdate = lr.approvalDate, //DateTime.Now,
                repaystartdate = lr.approvalDate.AddMonths(1), //DateTime.Now,
                loanamount = (lr.amount),
                totrepayable = (repayAmount),
                repayamount = (monthlyPay ?? 0),
                interestamt = (interest),
                payableacctno = loanSetup.accountno,
                loaninterest = (loanInterest ?? 0), //loanSetup.Loaninterest,
                processrate = loanSetup.processrate,
                //Processamt
                duration = Convert.ToInt16(loanDuration), //loanSetup.Duration,
                glrefnumber = "",
                glbankaccount = "", // GLbankAcc.accountno,
                gapproved = true,
                accountno = accno,
                description = loanSetup.loandesc,
                repay = 1,
                loancode = loanSetup.loancode,
                repayOrder = repayOrder
            };
            var glref = gLPostingRepository.PostLoanRequest((amount), (interest),
            accno, loanSetup.accountno, bankAcc.accountno, id, loanSetup.loandesc,
            "", fullname, lr.approvalDate);

            if (processingFee > 0)
            {
                try
                {
                    var chargecode = _chargesService.GetCharges(loanSetup.chargecode);
                    //var procFeeAcc = _glAccountServiceD.GetAccByName(chargecode.Accountno);
                    var procFeeref = gLPostingRepository.PostProcessingFeeDeductMemberSaving((processingFee),
                accno, chargecode.accountno, id, "",
                "", lr.approvalDate, fullname);
                }
                catch
                {

                }

            }
            loanTrans.grefnumber = glref;
            loanTrans.glrefnumber = glref;
            _loanTransService.AddLoanTrans(loanTrans);
            //PostLoanGuarantors(lr, tenantId, repayAmount);
            //PostLoanProcessedForMemberAccount(lr, glref, loanTrans.loanrefnumber, tenantId, id, repayAmount, loanSetup.loandesc, member.Fullname);
            updateProcessed(lr.Id, id);
            return Ok(loanTrans);

        }




        #endregion

        #region Helper

        private bool isEligible(string username)
        {
            return false;
        }

        void updateProcessed(int id, string userid)
        {
            var loanreq = _loanRequestService.GetLoanRequest(id);
            loanreq.status = "Approved";
            loanreq.processState = "Processed";
            loanreq.approvalDate = DateTime.Now;
            loanreq.approvedBy = userid;
            _loanRequestService.UpdateLoanRequest(loanreq);
        }

        void PostLoanCollaterals(LoanRequest loanRequest, string collateralId)
        {
            //ILoanCollateralService loanCollateralService
        }

        private IEnumerable<string> GetAccStr(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }

        private string GenerateNewAccountNo(string type)
        {
            var chtacc = _glAccountService.GetAllGlAccountByDesc();
            var tempSav = _glAccountTier3Service.GetGlAccountByDesc(type);// "MEMBERS SAVINGS"
            int ch = int.Parse(chtacc);
            var sch = ch + 1;

            return $"{tempSav.accountno}{sch}";
        }

        private AccFormat GenerateAccNo(string type)
        {
            var tempLoan = _glAccountTier3Service.GetGlAccountByDesc(type); //"LOANS"
            var id = 1;
            var top = _glAccountService
                .GetAllGlAccounts()
                .Where(u => u.glacctc == tempLoan.glacctc)
                        .OrderByDescending(x => x.Id).FirstOrDefault();

            if (top != null)
            {
                //id = top.Id + 1;
                int iaccno = int.Parse(top.glacctd);
                id = iaccno + 1;
            }
            else
            {

            }
            var interim = _glAccountTier3Service.GetGlAccount($"{tempLoan.glaccta}{tempLoan.glacctb}{tempLoan.glacctc}");
            string glacctd = String.Format("{0,4:0000}", id);//"0" + id.ToString(),
            string accountno = interim.accountno + String.Format("{0,4:0000}", id);//"0" + id.ToString()
            var accform = new AccFormat()
            {
                accno = accountno,
                accd = glacctd
            };
            //return accountno;
            return accform;
        }

        private MemberAccount createMemberAccount(AccSchema accSchema, string appId)
        {
            var loanType = _loanSetupService.GetLoanSetup(accSchema.loancode);
            //var member = _userService.GetUserByEmail(accSchema.memberId);
            //var tempLoan = _glAccountTier3Service.GetGlAccountByDesc("LOANS");

            var accnform = GenerateAccNo(accSchema.type);
            //var suf = GetAccStr(member.Accountno, 6).ToList();
            var memberacc = new MemberAccount()
            {
                memberid = accSchema.memberId,
                deductcode = loanType.loancode,
                memgroupacct = loanType.memgroupacct,
                transtype = loanType.category,
                accountno = accnform.accno,
                AppIdentityUserId = appId
            };

            saveNewGLAccount(accnform.accd, accnform.accno, accSchema.name);
            _membersAccountService.AddMembersAccount(memberacc, "");
            return memberacc;
        }

        private void saveNewGLAccount(string accd, string accno, string shortdesc)
        {
            GLAccount gl = new GLAccount();
            var accname = shortdesc.ToUpper();
            gl.shortdesc = accname;
            gl.fulldesc = accname;
            gl.glaccta = accno.Substring(0, 2);
            gl.glacctb = accno.Substring(2, 2);
            gl.glacctc = accno.Substring(4, 2);
            gl.glacctd = accd;
            gl.accountno = accno;
            gl.fulldesc = shortdesc.ToUpper();
            gl.updated_date = DateTime.Now;
            gl.accttype = "ASSETS";
            gl.acctlevel = 4;
            gl.balanceSheetMap = "";
            gl.created_date = DateTime.Now;
            gl.internalind = false;
            gl.reportMap = "";
            gl.reportMapSub = "";
            _glAccountService.AddGlAccount(gl,"");
        }

        private void RegisterLoanDeduction(LoanTrans loan)
        {
            var currLoan = _loanTransService.GetLoanTransByRef(loan.loanrefnumber);
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var repayment = new LoanRepayment()
            {
                description = currLoan.description,
                interestamt = currLoan.interestamt,
                //branchname = currLoan.Branchname,
                loanamount = currLoan.totrepayable,
                loancode = currLoan.loancode,
                loanrefnumber = currLoan.loanrefnumber,
                memberid = currLoan.memberid,
                repayamount = currLoan.repayamount,
                repayrefnumber = "LRPY/" + refno,
                transdate = DateTime.Now, //DateTime.Now,
                transmonth = DateTime.Now.Month, //DateTime.Now.Month,
                transyear = DateTime.Now.Year, //DateTime.Now.Year,
                created_date = DateTime.Now,
                updated = 1,
                Id = 0
                //Updated = 1
            };
            _loanRepaymentService.AddLoanRepayment(repayment);
        }

        class AccFormat
        {
            public string accno { get; set; }
            public string accd { get; set; }
        }

        #endregion
    }
}
