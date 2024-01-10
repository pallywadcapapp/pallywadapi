﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using PallyWad.Auth.Models;
using PallyWad.Domain;
using PallyWad.Services;
using System.IdentityModel.Tokens.Jwt;
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
        private string baseUrl;
        //private IMapper _mapper

        public AuthController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<AppIdentityUser> signInManager, ISmtpConfigService smtpConfigService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _smtpConfigService = smtpConfigService;
            baseUrl = baseUrl ?? string.Empty;
        }

        [HttpPost, Route("mailconfig"), MapToApiVersion(2.0)]
        public IActionResult AddMailConfig(SmtpConfig config)
        {
            _smtpConfigService.AddSetupSmtpConfig(config);
            return Ok(config);
        }

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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = await _userManager.GetClaimsAsync(user);

                //if (!await _signInManager.CanSignInAsync(user))
                if (!user.EmailConfirmed)
                {
                    var result = new
                    {
                        Status = Status.Error,
                        Data = "Email confirmation required"
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
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("lastname", user.lastname),
                    new Claim("firstname", user.firstname),
                    new Claim("othernames", user.othernames),
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
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
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
                //UserProfile = {},
                sex = ""
            };

            string fullname = user.firstname + " " + user.othernames + " " + user.lastname;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

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

            await SendRegEmail(user, model, mailConfig);
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
            await SendRegEmail(user, model, mailConfig);
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
            var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
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
                //var baseUrl = Request..RequestUri.GetLeftPart(UriPartial.Authority);
                baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}/";
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = GetUserConfirmationRedirectUrl(code, user.Id, baseUrl);
                //var callbackUrl = GetUserConfirmationRedirectUrl(code, user.Id, tenantInfo.ClientUrl);

                //    await _userManager.SendEmailAsync(user.Email, "Reset Password",
                //"Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                string body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";

                try
                {
                    using (MimeMessage emailMessage = new MimeMessage())
                    {
                        MailboxAddress emailFrom = new MailboxAddress(mailConfig.username, mailConfig.username);
                        emailMessage.From.Add(emailFrom);

                        var fullname = user.firstname + " " + user.othernames + " " + user.lastname;

                        MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                        emailMessage.To.Add(emailTo);
                        emailMessage.Subject = $"{company}. Reset Password";

                        //var subject = "Verify your email";
                        string filePath = Directory.GetCurrentDirectory() + "\\Templates\\forgotpassword.html";
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
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
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
                var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
                var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest(new { status = "Error", message = "user not found" });
                }
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);



                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(mailConfig.username, mailConfig.username);
                    emailMessage.From.Add(emailFrom);

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"{company}. Account Registration";
                    //var tenantInfo = HttpContext.GetMultiTenantContext<AppTenantInfo>()?.TenantInfo; // .GetMultiTenantContext()?.TenantInfo;
                    baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}/";
                    var tkvUrl = baseUrl + "/verify?id=" + user.Id + "&token=" + emailConfirmationToken;

                    var subject = "Verify your email";
                    string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
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

        [HttpGet("ResetPasswordToken")]
        //[ServiceFilter(typeof(ModelValidationFilter))]
        public async Task<IActionResult> SendChangePasswordToken(string username)
        {
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            var mailkey = _configuration.GetValue<string>("AppSettings:DefaultMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();

            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return UnprocessableEntity("something went wrong, contact support to resolve the problem");

            var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");
            string body = "Your reset code is: <b>" + token + "</b>";
            using (var client = new SmtpClient()
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

            }
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
        #region helpers
        async Task SendRegEmail(AppIdentityUser user, RegisterModel model, SmtpConfig mailConfig)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    var company = _configuration.GetValue<string>("AppSettings:companyName");
                    MailboxAddress emailFrom = new MailboxAddress(mailConfig.username, mailConfig.username);
                    emailMessage.From.Add(emailFrom);

                    var fullname = user.firstname + " " + user.othernames + " " + user.lastname;

                    MailboxAddress emailTo = new MailboxAddress(fullname, user.Email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = $"{company}. Account Registration";
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var tenantInfo = HttpContext.GetMultiTenantContext<AppTenantInfo>()?.TenantInfo; // .GetMultiTenantContext()?.TenantInfo;
                    baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}/";
                    var tkvUrl = baseUrl + "/verify?id=" + user.Id + "&token=" + emailConfirmationToken;

                    var subject = "Verify your email";
                    string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
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
                    // var body = $"Click <a href=\"{tokenVerificationUrl}\">here</a>  to verify your account";

                    /*using (var client = new SmtpClient()
                    {
                        Port = mailConfig.port,
                        Credentials = new System.Net.NetworkCredential(mailConfig.username, mailConfig.password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = mailConfig.isSSL,
                        Host = mailConfig.smtp
                        //Host = "smtp.zoho.com",//mailConfig.smtp //= 

                    })
                    using (var mail = new System.Net.Mail.MailMessage())
                    {
                        mail.To.Add(model.Email);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;
                        mail.From = new MailAddress($"{company} <" + mailConfig.username + ">");
                        //try
                        //{
                        await client.SendMailAsync(mail);

                    }*/
                }
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

        #endregion
    }
}
