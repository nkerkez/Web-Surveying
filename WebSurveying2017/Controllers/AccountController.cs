using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebSurveying2017.Models;
using WebSurveying2017.Providers;
using WebSurveying2017.Results;
using WebSurveying2017.ViewModels;
using System.Linq;
using Facebook;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Data.Infrastructure;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IUnitOfWork unitOfWork;
        private ApplicationSignInManager _signInManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat, IUnitOfWork _unitOfWork)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            unitOfWork = _unitOfWork;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            var claims = ((ClaimsIdentity)User.Identity).Claims;
           
            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                Role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault() != null ? claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value : "",
                Id = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault() != null ? claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value : "",
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }


        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            

            // IdentityUser user = null;
            ApplicationUser user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (MyLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(int.Parse(User.Identity.GetUserId()), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(int.Parse(User.Identity.GetUserId()), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(int.Parse(User.Identity.GetUserId()),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(int.Parse(User.Identity.GetUserId()));
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(int.Parse(User.Identity.GetUserId()),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                try
                {
                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                       OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                        CookieAuthenticationDefaults.AuthenticationType);

                    var role = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                    var userId = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, role, userId);
                    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                }catch(Exception)
                {

                }
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }/*
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            UserManager.GetClaims()

            var user = UserManager.FindByEmail(model.Username);
            SignInManager.SignIn(user, true, false);

            return Ok();
        }
        */
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> ForgotPassword(LoginViewModel model)
        {

            
            var user = await UserManager.FindByNameAsync(model.Username);
            if (user == null /* || !(await UserManager.IsEmailConfirmedAsync(user.Id))*/)
            {
                // Don't reveal that the user does not exist or is not confirmed
                //  return View("ForgotPasswordConfirmation");
                ModelState.AddModelError("", "Ne postoji korisnik sa unetim email-om");
                return BadRequest(ModelState);
            }
            if(user.PasswordHash == null)
            {
                ModelState.AddModelError("", "Korisnik je registrovan korišćenjem eksternog naloga i nema pravo resetovanja šifre");
                return BadRequest(ModelState);
            }



            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var _code = code.Replace("/", "*");
            /*   var callbackUrl = new Uri(Url.Link("ResetPasswordRoute",
           new {  code = code }));*/
            var callbackUrl = "http://localhost:49681/#!/resetpassword/" + _code;
            await UserManager.SendEmailAsync(user.Id, "Reset Password",
        "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");



            // If we got this far, something failed, redisplay form
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPassword", Name = "ResetPasswordRoute")]
        public IHttpActionResult ResetPassword(string code)
        {
            //TODO: Fix this?
            var resetPasswordBM = new ResetPasswordBindinfModel() { Code = code };
            return Ok(resetPasswordBM);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindinfModel model)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }
            model.Code = model.Code.Replace("*", "/");
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Ne postoji korisnik sa unetim email-om");
                return BadRequest(ModelState);
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(); ;
        }

        //


        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel loginVM)
        {
            ApplicationUser user = UserManager.Find(loginVM.Username, loginVM.Password);
            if (user != null)
            {
                
                if(!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Niste aktivirali profil");
                    return BadRequest(ModelState);
                }
                
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                try
                {
                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                       OAuthDefaults.AuthenticationType);
                
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                
                var role = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                var userId = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, role, userId);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                
                return Ok(properties.Dictionary);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                ModelState.AddModelError("", "Ne postoji korisnik sa unetim Email-om i šifrom");
                return BadRequest(ModelState);
            }
           
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var extendUser = new Model.Model.UserExtend();
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                Birthday = model.Birthday,
                City = model.City,
                User = extendUser,
                SecurityStamp = "101010101010"
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            await UserManager.AddToRoleAsync(user.Id, model.RoleName);


            string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            try
            {
                await this.UserManager.SendEmailAsync(user.Id,
                                                        "Confirm your account",
                                                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }


            IdentityResult result = await this.UserManager.ConfirmEmailAsync(int.Parse(userId), code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }
        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal()
        {
            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }
            string firstName;
            string lastName;
            string gender;
            if (info.Login.LoginProvider == "Google")
            {
                var externalIdentity = Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                //         birthday = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value;
                gender = "male";

            }
            else
            {

                var accessToken = Authentication.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie).FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(accessToken);
                //    dynamic aaa = fb.Get("/me");
                dynamic myInfo = fb.Get("/me?fields=id,email,first_name,last_name,gender");
                lastName = myInfo["last_name"];
                firstName = myInfo["first_name"];
                gender = myInfo["gender"];
                

            }
            Gender _gender = Gender.MALE;
            if (gender.Equals("female"))
                _gender = Gender.FEMALE;



            var userExtend = new Model.Model.UserExtend();

            var user = new ApplicationUser()
            {
                UserName = info.Email,
                Email = info.Email,
                FirstName = firstName,
                LastName = lastName,
                User = userExtend,
                EmailConfirmed = true,
                Birthday = null,
                Gender = _gender
            };

            try
            {

                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }


                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                result = await UserManager.AddToRoleAsync(user.Id, "User");
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                var role = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                var userId = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, role, userId);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                return Ok(properties.Dictionary);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }



        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
