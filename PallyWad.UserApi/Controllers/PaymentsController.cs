using AutoMapper;
using AutoMapper.Execution;
//using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PallyWad.Application;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Domain.Entities;
using PallyWad.Services;
using System.ComponentModel.DataAnnotations;
//using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBankDepositService _bankDepositService;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        private readonly IMapper _mapper;
        private readonly ICompanyBankService _companyBankService;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly IMailService _mailService;
        public PaymentsController(IPaymentService paymentService, IBankDepositService bankDepositService, IConfiguration configuration,
            IUserService userService, IAppUploadedFilesService appUploadedFilesService, IMapper mapper, ICompanyBankService companyBankService,
            ISmtpConfigService smtpConfigService, IMailService mailService)
        {
            _paymentService = paymentService;
            _bankDepositService = bankDepositService;
            _userService = userService;
            _config = configuration;
            _appUploadedFilesService = appUploadedFilesService;
            _mapper = mapper;
            _companyBankService = companyBankService;
            _smtpConfigService = smtpConfigService;
            _mailService = mailService;
        }

        #region Get

        [HttpGet]
        public IActionResult GwtAllDeposits()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _bankDepositService.ListMemberBankDeposits(memberId)
                .OrderByDescending(u => u.requestDate)
                .Take(20);
            return Ok(result);
        }
        [HttpGet("ByLoanId")]
        public IActionResult GwtAllDeposits(string loanId)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _bankDepositService.ListMemberBankDeposits(memberId)
                .Where(u=>u.loanRefId == loanId)
                .OrderByDescending(u => u.requestDate)
                .Take(20);
            return Ok(result);
        }

        [HttpGet("ByDate")]
        public IActionResult GwtAllDepositsByDate(DateTime start, DateTime end)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _bankDepositService.ListMemberBankDeposits(memberId)
                .Where(u => u.requestDate >= start && u.requestDate <= end);
            return Ok(result);
        }

        [HttpGet, Route("depositDetails")]
        public IActionResult GetDepositDetails(int id)
        {
            var result = _bankDepositService.GetBankDeposits(id);
            return Ok(result);
        }

        [HttpGet, Route("TotalLoanPayment")]
        public IActionResult GetTotalLoanPayment(string loanId)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _bankDepositService.ListMemberBankDeposits(memberId)
                .Where(u=>u.loanRefId == loanId && u.status == "Approved")
                .Sum(u=>u.amount);
            return Ok(result);
        }

        [HttpGet, Route("imageUrl")]
        public async Task<IActionResult> GetPaymentUrl(string id)
        {
            try
            {
                var princ = HttpContext.User;
                var memberId = princ.Identity?.Name;
                var upfile = _appUploadedFilesService.ListAllSetupAppUploadedFiles()
                    .Where(u => u.comment == id && u.transOwner == "Deposit").FirstOrDefault();
                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               upfile.fileurl);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch
            {
                var path = Path.Combine(
                              Directory.GetCurrentDirectory(),
                              "NC","R.png");

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
        }

        //[HttpGet, Route("LoanAcc")]
        //public string GetAccNo(string loanrefno, string memberId)
        //{
        //    var result = _membersAccountService.GetLoanTransMemberAcc(memberId, loanrefno).Accountno;
        //    return result;
        //}


        [HttpGet("CompanyAccount")]
        public async Task<IActionResult> GwtCompanyAccount()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var companyAcc = _companyBankService.ListAllCompanyBank();
            Random rnd = new Random();
            int accSel = rnd.Next(1, companyAcc.Count);
            var result = companyAcc[accSel - 1];

            var user = _userService.GetUser(memberId);

            if (user == null)
            {
                return BadRequest(new Response { Status = "error", Message = "Invalid Token" });
            }


            var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";
            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = memberId,
                Subject = "PallyWad Capital Bank Account"
            };
            await SendBankMail(mailReq, result, fullname);
            return Ok(result);
        }

        #endregion

        #region Post


        /*[HttpPost]
        public async Task<IActionResult> PosttDeposit([FromBody] BankDeposit deposit)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;

            var member = _userService.GetUserByEmail(memberId);
            var fullname = member.lastname + " " + member.firstname + " " + member.othernames;
            deposit.requestDate = DateTime.Now;
            deposit.memberId = memberId;
            deposit.status = "Pending";
            deposit.processState = "Pending";
            deposit.fullname = fullname;
            _bankDepositService.AddBankDeposit(deposit);
            string message = "Dear Admin, <br/>" + fullname +
                " hereby made deposits of funds to the tune of " +
                GetFormattedCurrency(Convert.ToDecimal(deposit.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ". Kindly help treat this as an urgent matter.";

            //var header = _headerService.Getheaders();
            //string org = header.Coyname;
            try
            {
                //await SendAdminEmailNotification(memberId, header.Emailaddress, message, "Funds Deposit Request", org);
            }
            catch
            {

            }
            return Ok(deposit);
        }*/

        //[HttpPost, Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(DepositFildDto userDeposit)
        {
            List<IFormFile> files = userDeposit.file;
            long size = files.Sum(f => f.Length);
            List<string> filenames = new List<string>();
            List<string> status = new List<string>();
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;

            var deposit = _mapper.Map<BankDeposit>(userDeposit);

            if (memberId == null)
                return Unauthorized(new Response { Status = "error", Message = "Token Expired" });

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
                        string dirPath = Path.Combine(_config["StoredFilesPath"], _config["PaymentDir"]);
                        var filePath = Path.Combine(_config["StoredFilesPath"], _config["PaymentDir"], filename);
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        filenames.Add(filename);
                        status.Add("File " + dataFileName + " valid for upload");
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            var depositId = saveUserDeposit(deposit, memberId);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName), depositId.ToString());
                            //saveUserDocumentUploads(userDocument.documentNo, document.type, document.name, userDocument.documentRefId, userDocument.expiryDate, filePath, memberId);
                        }
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filenames, status });
        }
        #endregion

        #region helper
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
        private string GetFormattedCurrency(decimal value, int decimalPlaces, string culture)
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            return value.ToString($"C{decimalPlaces}", cultureInfo);
        }

        void saveUpload(string filename, string path, string Id, string filetype, string depositId)
        {
            var newAppUpload = new AppUploadedFiles()
            {
                created_date = DateTime.Now,
                comment = depositId,
                filename = filename,
                fileurl = path,
                uploaderId = Id,
                type = filetype,
                year = DateTime.Now.Year,
                transOwner = "Deposit"
            };
            _appUploadedFilesService.AddAppUploadedFiles(newAppUpload);
        }

        int saveUserDeposit(BankDeposit deposit, string memberId)
        {
            var member = _userService.GetUserByEmail(memberId);
            var fullname = member.lastname + " " + member.firstname + " " + member.othernames;
            deposit.requestDate = DateTime.Now;
            deposit.memberId = memberId;
            deposit.status = "Pending";
            deposit.processState = "Pending";
            deposit.fullname = fullname;
            _bankDepositService.AddBankDeposit(deposit);
            string message = "Dear Admin, <br/>" + fullname +
                " hereby made deposits of funds to the tune of " +
                GetFormattedCurrency(Convert.ToDecimal(deposit.amount), 2, "HA-LATN-NG")
                //string.Format("{0:C}", Convert.ToDecimal(gr.amount ) )
                + ". Kindly help treat this as an urgent matter.";

            //var header = _headerService.Getheaders();
            //string org = header.Coyname;
            try
            {
                //await SendAdminEmailNotification(memberId, header.Emailaddress, message, "Funds Deposit Request", org);
            }
            catch
            {

            }
            return deposit.Id;
        }

        internal async Task SendBankMail(MailRequest request, CompanyBank bank, string fullname)
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

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "companyaccount.html");
                //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\loanrequest.html";
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    bank.name,bank.accountname, bank.accountno);

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;
                emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }


        #endregion
    }

    public class DepositFildDto: DepositDto
    {
        //[Required]
        //public double amount { get; set; }
        //public string otherdetails { get; set; }
        //public string channel { get; set; }
        public List<IFormFile> file { get; set; }
    }
}
