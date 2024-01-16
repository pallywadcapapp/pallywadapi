using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Entities;
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
        private readonly ILoanCollateralService _loanCollateralService;
        private readonly IGlAccountTier3Service _glAccountTier3Service;

        public LoanRequestController(IHttpContextAccessor contextAccessor, ILogger<LoanRequestController> logger,
            ILoanSetupService loanSetupService, ILoanRequestService loanRequestService, IMembersAccountService membersAccountService,
            IGlAccountService glAccountService, IUserService userService, IGlAccountTransService glAccountTransService,
            ILoanTransService loanTransService, IChargesService chargesService, IMapper mapper, ILoanCollateralService loanCollateralService,
            IGlAccountTier3Service glAccountTier3Service)
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
            _loanCollateralService = loanCollateralService;
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
        #endregion
    }
}
