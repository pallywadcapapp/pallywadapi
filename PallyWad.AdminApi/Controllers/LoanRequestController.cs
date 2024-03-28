using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PallyWad.Application;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Migrations;
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
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly ICompanyService _companyService;
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        private readonly INotificationsService _notificationsService;
        public LoanRequestController(IHttpContextAccessor contextAccessor, ILogger<LoanRequestController> logger,
            ILoanSetupService loanSetupService, ILoanRequestService loanRequestService, IMembersAccountService membersAccountService,
            IGlAccountService glAccountService, IUserService userService, IGlAccountTransService glAccountTransService,
            ILoanTransService loanTransService, IChargesService chargesService, IMapper mapper, 
            //ILoanCollateralService loanCollateralService,
            IGlAccountTier3Service glAccountTier3Service, ILoanRepaymentService loanRepaymentService, ISmtpConfigService smtpConfigService,
            IConfiguration config, IMailService mailService, ICompanyService companyService, IAppUploadedFilesService appUploadedFilesService,
            INotificationsService notificationsService)
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
            _config = config;
            _smtpConfigService = smtpConfigService;
            _loanRepaymentService = loanRepaymentService;
            _mailService = mailService;
            _companyService = companyService;
            _appUploadedFilesService = appUploadedFilesService;
            _notificationsService = notificationsService;
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
            updateApprProcessed(loanRequest.Id, id);
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
        public async Task<IActionResult> LoanRequestApproved(LoanRequestVM lr)//, int Id, int duration, double interest, double processingFee)
        {
           
            var loan = _loanRequestService.GetLoanRequest(lr.Id);

            var user = _userService.GetUserByEmail(loan.memberId);
            var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";

            double? mtrepay = (lr.amount / lr.duration) + lr.monthlyrepay;

            loan.status = "Approved";
            loan.duration = lr.duration;
            loan.loaninterest = lr.loaninterest; // interest;
            loan.processingFee = lr.processingFee;
            loan.approvalDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            loan.monthlyrepay = lr.monthlyrepay;
            loan.monthtotalrepay = mtrepay;
            loan.loanmonthlyinterest = lr.loanmonthlyinterest;
            _loanRequestService.UpdateLoanRequest(loan);

            var company = _companyService.GetCompany();
            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = loan.memberId,
                Subject = "Loan Request Pre-Approved"
            };
            await SendLoanApprovalMail(mailReq, loan, fullname, company);
            SendNotification(loan.memberId, "Your Loan " + $"{loan.loanId} of " +
                $"{AppCurrFormatter.GetFormattedCurrency(loan.amount, 2, "HA-LATN-NG")}" +
                " has been pre-approved", $"Loan Pre-Approved " );
            return Ok(loan);

        }

        [HttpPut]
        [Route("loanrequestdecline")]
        public async Task<IActionResult> LoanRequestDecline(int id, string reason)
        {
            var loan = _loanRequestService.GetLoanRequest(id);

            var user = _userService.GetUserByEmail(loan.memberId);
            var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";

            loan.status = "Declined";
            loan.reason = reason;
            loan.approvalDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            _loanRequestService.UpdateLoanRequest(loan);

            var company = _companyService.GetCompany();
            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = loan.memberId,
                Subject = "Notice: Your Loan Application Status"
            };
            await SendLoanDeclineMail(mailReq, loan, fullname, company);
            SendNotification(loan.memberId, "Your Loan " + $"{loan.loanId} of " +
               $"{AppCurrFormatter.GetFormattedCurrency(loan.amount, 2, "HA-LATN-NG")}" +
               " has been rejected. Kindly check your email inbox for further details", $"Loan Request Denied ");

            return Ok(loan);

        }

        [HttpPut]
        [Route("ApproveLoanRequestCollateral")]
        public async Task<IActionResult> ApproveLoanRequestCollateral(int Id, double collateralValue)
        {
            var princ = HttpContext.User;
            var username = princ.Identity.Name;
            var loan = _loanRequestService.GetLoanRequest(Id);

            var user = _userService.GetUserByEmail(loan.memberId);
            var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";

            loan.status = "Collaterized";
            loan.processState = "Collaterized";
            loan.collaterizedDate = DateTime.Now;
            loan.updated_date = DateTime.Now;
            loan.approvedBy = username;
            loan.collateralValue = collateralValue;
            loan.isCollateralReceived = true;
            loan.isDocmentProvided = true;
            _loanRequestService.UpdateLoanRequest(loan);

            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = loan.memberId,
                Subject = "Loan Approved "
            };
            await SendLoanCollaterizedMail(mailReq, loan, fullname);
            SendNotification(loan.memberId, "Your Loan " + $"{loan.loanId} of " +
               $"{AppCurrFormatter.GetFormattedCurrency(loan.amount, 2, "HA-LATN-NG")}" +
               " have been fully collateralized.", $"Loan Collateralized ");


            return Ok(loan);

        }

        [Authorize]
        [HttpPut]
        [Route("ProcessLoanRequest")]
        public async Task<IActionResult> ProcessLoanRequests([FromBody] LoanRequestVM lr)
        {
            var princ = HttpContext.User;
            var id = princ.Identity.Name;

            var __loan = _loanRequestService.GetLoanRequest(lr.loanId);
            //var bankAcc = _glAccountServiceD.GetAccByName(_config["BankAccountName"]);
            var bankAcc = _glAccountService.GetAccByName("BANK ACCOUNT");
            //var GLbankAcc = _glAccountService.GetAccByName("POLARIS");
            var loanSetup = _loanSetupService.GetLoanSetup(lr.loanCode);
            var member = _userService.GetUserByEmail(lr.memberId);  //_memberService.Getmember(lr.memberId);
            var loanInterest = lr.loaninterest;
            var monthlyInterest = lr.loanmonthlyinterest;
            var loanDuration = lr.duration??1; 
            var processingFee = lr.processingFee;
            var amount = lr.amount;
            var interest = amount * Convert.ToDouble(loanInterest) / 100;
            var totalinterest = amount * Convert.ToDouble(loanInterest) * loanDuration / 100;
            var repayAmount = amount + totalinterest;
            var accno = "";
            var category = loanSetup.category;
            var repayOrder = loanSetup.repayOrder;

            var pp = _membersAccountService.ListMembersAccounts(lr.memberId).Where(u => u.transtype == category).FirstOrDefault();
            string fullname = member.lastname + " " + member.firstname + " " + member.othernames;
            if (pp == null)
            {
                var sfullname = member.lastname + " " + member.firstname + " " + member.othernames;//.Substring(0,1);
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
            //var monthlyPay = repayAmount / loanDuration;

            //get loan type for specific account


            var loanTrans = new LoanTrans()
            {
                memberid = lr.memberId,
                loanrefnumber = lr.loanId,
                transdate = lr.approvalDate, //DateTime.Now,
                repaystartdate = lr.approvalDate.AddMonths(1), //DateTime.Now,
                loanamount = (lr.amount),
                monthlyInterest = lr.monthlyrepay, // monthly interest amount payable
                totrepayable = (repayAmount),
                repayamount = lr.monthtotalrepay??0, //(monthlyPay ?? 0),
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
                description = $"{loanSetup.category} {loanSetup.loancode}" ,
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
            RegisterLoanDeduction(loanTrans);
            updateProcessed(lr.Id, id);
            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = lr.memberId,
                Subject = "Loan Disbursement "
            };
            await SendLoanProcessedMail(mailReq, loanTrans, __loan, fullname);
            SendNotification(__loan.memberId, "Your Loan " + $"{__loan.loanId} of " +
               $"{AppCurrFormatter.GetFormattedCurrency(__loan.amount, 2, "HA-LATN-NG")}" +
               " has been fully processed and disbursed to your account.", $"Loan Disbursed ");
            return Ok(loanTrans);

        }


        [HttpPost, Route("UploadFile")]
        public async Task<IActionResult> OnPostUploadAsync(UserCollateralFileDto userCollateral)
        {
            List<IFormFile> files = userCollateral.file;
            long size = files.Sum(f => f.Length);
            List<string> filenames = new List<string>();
            List<string> status = new List<string>();

            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;

            var loan = _loanRequestService.GetAllLoanRequests().Where(u => u.loanId == userCollateral.loanId).FirstOrDefault();
            
            if (loan == null)
                return BadRequest(new Response { Status = "error", Message = "loan type not found" });
            if (memberId == null)
                return Unauthorized(new Response { Status = "error", Message = "Token Expired" });
            if (files == null)
                return BadRequest();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    string dataFileName = Path.GetFileName(formFile.FileName);
                    string extension = Path.GetExtension(dataFileName);


                    string[] allowedExtsnions = new string[] { ".png", ".jpg", ".jpeg", ".pdf" };
                    if (!allowedExtsnions.Contains(extension))
                    {
                        status.Add("Invalid File " + dataFileName + " for upload");
                    }
                    else
                    {

                        var filename = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);
                        string dirPath = Path.Combine(_config["StoredFilesPath"]);
                        var filePath = Path.Combine(_config["StoredFilesPath"], filename);
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }

                        filenames.Add(filename);
                        status.Add("File " + dataFileName + " valid for upload");
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName),userCollateral.loanId);
                        }
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filenames, status, id = userCollateral.loanId });
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
            loanreq.status = "Processed";
            loanreq.processState = "Processed";
            loanreq.processedDate = DateTime.Now;
            loanreq.isProcessCleared = true;
            loanreq.approvedBy = userid;
            _loanRequestService.UpdateLoanRequest(loanreq);
        }

        void updateApprProcessed(int id, string userid)
        {
            var loanreq = _loanRequestService.GetLoanRequest(id);
            loanreq.status = "Approved";
            loanreq.processState = "Approved";
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
                loanamount = currLoan.loanamount,
                interestRate = loan.loaninterest,
                loancode = currLoan.loancode,
                loanrefnumber = currLoan.loanrefnumber,
                memberid = currLoan.memberid,
                repayamount = 0,//currLoan.repayamount,
                repayrefnumber = "LRPY/" + refno,
                transdate = DateTime.Now, //DateTime.Now,
                transmonth = DateTime.Now.Month, //DateTime.Now.Month,
                transyear = DateTime.Now.Year, //DateTime.Now.Year,
                created_date = DateTime.Now,
                updated = 1,
                Id = 0,
                cappaymentcount = 0,
                interestbalance = currLoan.interestamt,
                repaymentDate = DateTime.Now.AddMonths(1),
                //Updated = 1
            };
            _loanRepaymentService.AddLoanRepayment(repayment);
        }

        internal async Task SendLoanApprovalMail(MailRequest request, LoanRequest lr, string fullname, Company _company)
        {

            var mailkey = _config.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _config.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else
            {
                var urllink = "<a href=\"https://pallywad.com/tos\">Terms and Conditions</a>";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "loanapproval.html");
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    AppCurrFormatter.GetFormattedCurrency(lr.amount, 2, "HA-LATN-NG"),
                    $"{urllink}", lr.purpose, lr.duration, lr.loaninterest, lr.monthlyrepay, lr.category, lr.collateral,
                    AppCurrFormatter.GetFormattedCurrency(lr.estimatedCollateralValue??0, 2, "HA-LATN-NG"),
                    _company.address, _company.phoneno);
                    //DateTime.Today.Date.ToShortDateString());

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }

        internal async Task SendLoanDeclineMail(MailRequest request, LoanRequest lr, string fullname, Company _company)
        {

            var mailkey = _config.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _config.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else
            {
                var urllink = "<a href=\"https://app.pallywad.com/login\">Here</a>";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "loandeclined.html");
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    AppCurrFormatter.GetFormattedCurrency(lr.amount, 2, "HA-LATN-NG"),
                    lr.reason, lr.purpose, lr.duration, lr.category, _company.phoneno, $"{urllink}");
                //DateTime.Today.Date.ToShortDateString());

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }

        internal async Task SendLoanCollaterizedMail(MailRequest request, LoanRequest lr, string fullname)
        {

            var mailkey = _config.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _config.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else
            {
                var urllink = "<a href=\"https://app.pallywad.com/login\">Here</a>";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "loancollaterized.html");
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    AppCurrFormatter.GetFormattedCurrency(lr.amount, 2, "HA-LATN-NG"),
                    $"{urllink}", lr.purpose, lr.duration, lr.loaninterest, lr.monthlyrepay,
                    lr.category,
                     lr.collateral,
                     AppCurrFormatter.GetFormattedCurrency(lr.collateralValue??0, 2, "HA-LATN-NG"));
                //DateTime.Today.Date.ToShortDateString());

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }

        internal async Task SendLoanProcessedMail(MailRequest request, LoanTrans ltr,  LoanRequest lr, string fullname)
        {

            var mailkey = _config.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _config.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else
            {
                var urllink = "<a href=\"https://app.pallywad.com/login\">Here</a>";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "loancollaterized.html");
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    AppCurrFormatter.GetFormattedCurrency(ltr.loanamount, 2, "HA-LATN-NG"),
                    $"{urllink}",  lr.purpose, lr.duration, lr.loaninterest, lr.monthlyrepay,
                    lr.category,
                     lr.collateral,
                     AppCurrFormatter.GetFormattedCurrency(lr.collateralValue ?? 0, 2, "HA-LATN-NG"));
                //DateTime.Today.Date.ToShortDateString());

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }

        void saveUpload(string filename, string path, string Id, string filetype, string loanId)
        {
            var newAppUpload = new AppUploadedFiles()
            {
                created_date = DateTime.Now,
                comment = loanId,
                filename = filename,
                fileurl = path,
                uploaderId = Id,
                type = filetype,
                year = DateTime.Now.Year,
                transOwner = "admincollateral"
            };
            _appUploadedFilesService.AddAppUploadedFiles(newAppUpload);
        }

        private void SendNotification(string memberId, string message, string subject)
        {
            NotificationHelper.Notification(_notificationsService);
            NotificationHelper.SendUserNotification(memberId, message, subject);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        class AccFormat
        {
            public string accno { get; set; }
            public string accd { get; set; }
        }

        public class UserCollateralFileDto
        {
            public string loanId { get; set; }
            public List<IFormFile> file { get; set; }
            public string otherdetails { get; set; }
        }

        #endregion
    }
}
