using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using PallyWad.Auth.Models;
using PallyWad.Domain;
using PallyWad.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace PallyWad.Auth.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly ISMSConfigService _smsConfigService;
        private readonly IRedisCacheService _redisCacheService;
        private string baseUrl;
        //private IMapper _mapper

        public AuthController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<AppIdentityUser> signInManager, ISmtpConfigService smtpConfigService,
            ISMSConfigService smsConfigService, IRedisCacheService redisCacheService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _smtpConfigService = smtpConfigService;
            baseUrl = baseUrl ?? string.Empty;
            _smsConfigService = smsConfigService;
            _redisCacheService = redisCacheService;
        }

        /*[HttpPost, Route("mailconfig"), MapToApiVersion(2.0)]
        public IActionResult AddMailConfig(SmtpConfig config)
        {
            _smtpConfigService.AddSetupSmtpConfig(config);
            return Ok(config);
        }*/

        [HttpGet]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.Name,//.GetUserName(), 
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        [HttpGet, HttpPost]
        [AllowAnonymous]
        [Route("BaseUrl")]
        public IActionResult GetBaseUrl()
        {
            //var BaseAddress = new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority));
            var BaseAddress = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}/";
            return Ok(BaseAddress);
        }

        [HttpGet("checkUser")]
        public async Task<IActionResult> CheckUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user != null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [Authorize]
        [HttpPost("unlock")]
        public async Task<IActionResult> Unlock([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = await _userManager.GetClaimsAsync(user);

                if (!user.EmailConfirmed)
                {
                    var result = new Response
                    {
                        Status = Status.Error.ToString(),
                        Message = "Email confirmation required"
                    };

                    return BadRequest(result);
                }

                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true,
                            lockoutOnFailure: false);

                if (signInResult.RequiresTwoFactor)
                {
                    var result = new
                    {
                        Status = Status.Success,
                        Message = "Enter the code generated by your authenticator app",
                        Data = new { requires2FA = true }
                    };
                    return Ok(result);
                }

                var authClaims = new List<Claim>
                {
                    
                    new Claim("username", user.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("lastname", user.lastname),
                    new Claim("firstname", user.firstname),
                    new Claim("othernames", user.othernames),
                    new Claim("address", FillClaimsNull(user.address)),
                    new Claim("sex", FillClaimsNull(user.sex)),
                    new Claim("dob", FillClaimsNull(user.dob.ToString())),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return BadRequest(new Response { Status = "error", Message = "username or password incorrect" });
            }
            //return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            //var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            if (mailConfig == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords don't match!");
            }
            AppIdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email, //model.Username,
                firstname = model.firstname, lastname = model.lastname,
                othernames = model.othernames,
                type = model.type,
                PhoneNumber = model.phoneNo,
                EmailConfirmed = true,
                //UserProfile = {},
                sex = ""
            };

            string fullname = user.firstname + " " + user.othernames + " " + user.lastname;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                // return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.Errors });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Provider))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Provider));

            if (model.type == "2")
            {

                if (await _roleManager.RoleExistsAsync(UserRoles.Provider))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Provider);
                    await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
                    await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.GivenName, fullname));
                    await _userManager.AddClaimAsync(user, new Claim("Provider", UserClaims.Provider));
                }
            }
            else
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
                    await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.GivenName, fullname));
                    await _userManager.AddClaimAsync(user, new Claim("Provider", UserClaims.User));
                }
            }

            //await SendRegEmail(user, model, mailConfig, "register");
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords don't match!");
            }

            AppIdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email, //model.Username,
                                        //SSN = model.SSN,
                firstname = model.firstname,
                lastname = model.lastname,
                othernames = model.othernames,
                type = model.type,
                PhoneNumber = model.phoneNo,
                //UserProfile = { },
                sex = ""
            };
            var fullname = user.firstname + " " + user.othernames + " " + user.lastname;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Provider))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Provider));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
                await _userManager.AddToRoleAsync(user, UserRoles.Provider);
                await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
                await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.GivenName, fullname));
                await _userManager.AddClaimAsync(user, new Claim("Provider", UserClaims.Admin));
            }
            await SendRegEmail(user, model, mailConfig, "register-admin");
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPut, Route("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserPersonalInfoModel model)
        {
            return Ok();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var _userManager = HttpContext.RequestServices
                                            .GetRequiredService<UserManager<AppIdentityUser>>();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest("Invalid Email");
                }

                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return BadRequest("Email Yet to be confirmed");
                }
                baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}/";
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var base64EncodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                var callbackUrl = $"{baseUrl}{HttpContext.Request.Path.Value.Replace("ForgotPassword", "")}resetpassword?id={user.Id}&token={base64EncodedToken}";

                string body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";

                try
                {
                    using (MimeMessage emailMessage = new MimeMessage())
                    {
                        MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                        emailMessage.From.Add(emailFrom);

                        var fullname = user.firstname + " " + user.othernames + " " + user.lastname;

                        MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                        emailMessage.To.Add(emailTo);
                        emailMessage.Subject = $"{company}. Reset Password";

                        //var subject = "Verify your email";
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "forgotpassword.html");
                        //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\forgotpassword.html";
                        string emailTemplateText = System.IO.File.ReadAllText(filePath);
                        emailTemplateText = string.Format(emailTemplateText, fullname, callbackUrl, DateTime.Today.Date.ToShortDateString());

                        BodyBuilder emailBodyBuilder = new BodyBuilder();
                        emailBodyBuilder.HtmlBody = emailTemplateText;
                        //emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                        emailMessage.Body = emailBodyBuilder.ToMessageBody();

                        using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                        {
                            mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                            mailClient.Authenticate(mailConfig.username, mailConfig.password);
                            mailClient.Send(emailMessage);
                            mailClient.Disconnect(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                /*using (var client = new SmtpClient()
        {
            Port = mailConfig.port, //587,//
            Credentials = new System.Net.NetworkCredential(mailConfig.username, mailConfig.password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = mailConfig.isSSL,// true,//
            Host = mailConfig.smtp, //= "smtp.office365.com"//
            UseDefaultCredentials = false

        })
        using (var mail = new System.Net.Mail.MailMessage())
        {
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            mail.Body = body;
            mail.IsBodyHtml = true;

            mail.From = new MailAddress(company +" <" + mailConfig.username + ">"); // "segxy2708@hotmail.com"
            await client.SendMailAsync(mail);

        }*/
                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return BadRequest("Invalid Code");
        }

        [HttpPost]
        [HttpGet]
        [Route("Verify")]
        public async Task<IActionResult> VerifyAccount(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));


            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
                var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
                SendWelcomeEmail(user, mailConfig);
                return Ok("success");
            }
            else
            {
                return BadRequest(result.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("EmailActivationCode")]
        public async Task<IActionResult> GetEmailActivation(string email)
        {
            try
            {
                var company = _configuration.GetValue<string>("AppSettings:companyName");
                var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
                var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest(new { status = "Error", message = "user not found" });
                }
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var base64EncodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));



                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                    emailMessage.From.Add(emailFrom);

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"Confirm Your PallyWad Capital Account";
                    baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                    var tkvUrl = $"{baseUrl}{HttpContext.Request.Path.Value.Replace("EmailActivationCode", "")}verify?id={user.Id}&token={base64EncodedToken}";


                    //var subject = "Verify your email";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "verifyemail.html");
                    //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
                    string emailTemplateText = System.IO.File.ReadAllText(filePath);
                    emailTemplateText = string.Format(emailTemplateText, fullname, tkvUrl, DateTime.Today.Date.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(mailConfig.username, mailConfig.password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                        return Ok(new { status = "Ok", message = "Mail Sent" });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            // return BadRequest("err");
        }

        [HttpGet("newUser")]
        public async Task<IActionResult> Onboard(string username)
        {

            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                user = new AppIdentityUser()
                {
                    Email = username,
                    UserName = username,
                    NormalizedEmail = username.ToUpper(),
                    NormalizedUserName = username.ToUpper(),
                };
                var token = await _userManager.GenerateUserTokenAsync(
                user, "PasswordlessLoginTotpProvider", "passwordless-auth");
                _redisCacheService.SetData<AppIdentityUser>(username, user, expirationTime);
                await SendEmailToken(user, mailConfig, token);
                return Ok(new Response() { Status = "success", Message = token });
            }
            else
            {
                return Ok(new Response() { Status = "Ok", Message = "Email not available"});
            }

        }

        [HttpGet("ValidateNewUser")]
        public async Task<IActionResult> ValidateOnboard(string username, string token)
        {
            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                var cacheData = _redisCacheService.GetData<AppIdentityUser>(username);
                user = cacheData;
                var isValid = await _userManager.VerifyUserTokenAsync(
                  user, "PasswordlessLoginTotpProvider", "passwordless-auth", token);
                return Ok(new Response() { Status = "Ok", Message = isValid });
            }
            else
            {
                return Ok(new Response() { Status = "Ok", Message = "Email not available" });
            }

        }

        [HttpGet("ResetPasswordToken")]
        //[ServiceFilter(typeof(ModelValidationFilter))]
        public async Task<IActionResult> SendChangePasswordToken(string username)
        {
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var smsConfig = _smsConfigService.ListAllSetupSMSConfig().Where(u => u.configname == mailkey).FirstOrDefault();

            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return UnprocessableEntity("something went wrong, contact support to resolve the problem");

            var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");
            string body = "Your reset code is: <b>" + token + "</b>";
            await SendForgotToken(user, mailConfig, token);
            await SendSMSToken(user.PhoneNumber, smsConfig, token);
            /*using (var client = new SmtpClient()
            {
                Port = mailConfig.port, //587,//
                Credentials = new System.Net.NetworkCredential(mailConfig.username, mailConfig.password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = mailConfig.isSSL,// true,//
                Host = mailConfig.smtp, //= "smtp.office365.com"//
                UseDefaultCredentials = false

            })
            using (var mail = new System.Net.Mail.MailMessage())
            {
                mail.To.Add(user.Email);
                mail.Subject = "Reset Password Token";
                mail.Body = body;
                mail.IsBodyHtml = true;

                mail.From = new MailAddress("Coop <" + mailConfig.username + ">"); // "segxy2708@hotmail.com"
                await client.SendMailAsync(mail);

            }*/
            return Ok(token);
            //return NoContent();
        }


        [HttpPost("ValidateResetPasswordToken")]
        public async Task<IActionResult> ValidateResetPasswordToken(string username, string otp)
        {
            var _userManager = HttpContext.RequestServices
                                            .GetRequiredService<UserManager<AppIdentityUser>>();
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return UnprocessableEntity("invalid user");//actually user not found

            var tokenVerified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", otp);
            if (!tokenVerified)
                return UnprocessableEntity("invalid user token");

            return Ok(new { status = "Ok", message = "Valid Token" });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ValidateEmailResetPasswordToken(string id, string code)
        {
            
            var user = await _userManager.FindByNameAsync(id);

            string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            if (user is null)
                return UnprocessableEntity("invalid user token");//actually user not found

            var tokenVerified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", code);
            
            if (!tokenVerified)
                return UnprocessableEntity("invalid user token");

            return NoContent();
        }

        [HttpGet("SendSMS")]
        public async Task<IActionResult> SendSMS(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);

            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var smsConfig = _smsConfigService.ListAllSetupSMSConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            await SendSMSToken(user.PhoneNumber, smsConfig, "677890");
            return Ok();
        }

        #region helpers
        async Task SendRegEmail(AppIdentityUser user, RegisterModel model, SmtpConfig mailConfig, string route)
        {            
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                    MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"{company}. Confirm Your PallyWad Capital Account";
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var base64EncodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));

                    baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                    var tkvUrl = $"{baseUrl}{HttpContext.Request.Path.Value.Replace(route,"")}verify?id={user.Id}&token={base64EncodedToken}";

                    //var subject = "Verify your email";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "verifyemail.html");
                    //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
                    string emailTemplateText = System.IO.File.ReadAllText(filePath);
                    emailTemplateText = string.Format(emailTemplateText, fullname, tkvUrl, DateTime.Today.Date.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(mailConfig.username, mailConfig.password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        async Task SendEmailToken(AppIdentityUser user, SmtpConfig mailConfig, string token)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                    MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"{company}. Confirm Your PallyWad Capital Account";
                    
                    //var subject = "Verify your email";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "verifyemailtoken.html");
                    //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
                    string emailTemplateText = System.IO.File.ReadAllText(filePath);
                    emailTemplateText = string.Format(emailTemplateText, fullname, token, DateTime.Today.Date.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(mailConfig.username, mailConfig.password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        async Task SendWelcomeEmail(AppIdentityUser user, SmtpConfig mailConfig)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                    MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $" Welcome to PallyWad Capital!"; //{company}.
                    //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var loginUrl = _configuration.GetValue<string>("AppSettings:loginUrl");

                    //var subject = "Verify your email";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "welcome.html");
                    //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\welcome.html";
                    string emailTemplateText = System.IO.File.ReadAllText(filePath);
                    emailTemplateText = string.Format(emailTemplateText, fullname, loginUrl, DateTime.Today.Date.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(mailConfig.username, mailConfig.password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        async Task SendForgotToken(AppIdentityUser user, SmtpConfig mailConfig, string token)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                    MailboxAddress emailFrom = new MailboxAddress(company, mailConfig.mailfrom);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"{company}. Password Token";

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "forgotpassword.html");
                    //string filePath = Directory.GetCurrentDirectory() + "\\Templates\\forgotpassword.html";
                    string emailTemplateText = System.IO.File.ReadAllText(filePath);
                    emailTemplateText = string.Format(emailTemplateText, fullname, token, DateTime.Today.Date.ToShortDateString());

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(mailConfig.smtp, mailConfig.port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(mailConfig.username, mailConfig.password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        async Task SendSMSToken(string phoneNo,SMSConfig mailConfig, string token)
        {
            try
            {
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                var message = $"Your token is {token}";
                var querystring = $"{mailConfig.URL}?recipient={phoneNo}&message={message}&subject={mailConfig.username}&channel=1&cid={mailConfig.password}";
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(querystring),

                };
               
                using HttpResponseMessage response = await httpClient.PostAsync(
        "",
        null);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                    
            }
            catch
            {
                throw;
            }
        }

        private string GetUserConfirmationRedirectUrl(string code, string userId, string BaseURL)
        {

            var absoluteUri = BaseURL + "auth/resetpassword?code=" + HttpUtility.UrlEncode(code) + "&userId=" + HttpUtility.UrlEncode(userId);
            return absoluteUri;
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                //expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string FillClaimsNull(string value)
        {
            if(value == null || value == "")
            {
                return "-";
            }
            else
            {
                return value;
            }
        }

        #endregion
    }
}
