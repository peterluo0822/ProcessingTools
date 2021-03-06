﻿namespace ProcessingTools.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Services.Web.Managers;
    using ProcessingTools.Web.Abstractions.Controllers;
    using ProcessingTools.Web.Constants;
    using ProcessingTools.Web.ViewModels.Manage;
    using Strings = ProcessingTools.Web.Resources.Controllers.Manage.Strings;

    [RequireHttps]
    [Authorize]
    public class ManageController : BaseMvcController
    {
        public const string ControllerName = "Manage";
        public const string IndexActionName = RouteValues.IndexActionName;
        public const string RemoveLoginActionName = nameof(ManageController.RemoveLogin);
        public const string ManageLoginsActionName = nameof(ManageController.ManageLogins);
        public const string AddPhoneNumberActionName = nameof(ManageController.AddPhoneNumber);
        public const string VerifyPhoneNumberActionName = nameof(ManageController.VerifyPhoneNumber);
        public const string EnableTwoFactorAuthenticationActionName = nameof(ManageController.EnableTwoFactorAuthentication);
        public const string DisableTwoFactorAuthenticationActionName = nameof(ManageController.DisableTwoFactorAuthentication);
        public const string RemovePhoneNumberActionName = nameof(ManageController.RemovePhoneNumber);
        public const string ChangePasswordActionName = nameof(ManageController.ChangePassword);
        public const string SetPasswordActionName = nameof(ManageController.SetPassword);
        public const string LinkLoginActionName = nameof(ManageController.LinkLogin);
        public const string LinkLoginCallbackActionName = nameof(ManageController.LinkLoginCallback);

        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext().Authentication;

        // GET: /Manage/Index
        [HttpGet, ActionName(IndexActionName)]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Strings.ChangePasswordSuccessMessage :
                message == ManageMessageId.SetPasswordSuccess ? Strings.SetPasswordSuccessMessage :
                message == ManageMessageId.SetTwoFactorSuccess ? Strings.SetTwoFactorSuccessMessage :
                message == ManageMessageId.Error ? Strings.ErrorMessage :
                message == ManageMessageId.AddPhoneSuccess ? Strings.AddPhoneSuccessMessage :
                message == ManageMessageId.RemovePhoneSuccess ? Strings.RemovePhoneSuccessMessage :
                string.Empty;

            var userId = this.UserId;
            var viewModel = new IndexViewModel
            {
                HasPassword = this.HasPassword(),
                PhoneNumber = await this.UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await this.UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await this.UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };

            return this.View(viewModel);
        }

        // POST: /Manage/RemoveLogin
        [HttpPost, ActionName(ManageController.RemoveLoginActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await this.UserManager.RemoveLoginAsync(this.UserId, new UserLoginInfo(loginProvider, providerKey));

            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsync(this.UserId);
                if (user != null)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return this.RedirectToAction(ManageLoginsActionName, new { Message = message });
        }

        // GET: /Manage/AddPhoneNumber
        [HttpGet, ActionName(AddPhoneNumberActionName)]
        public ActionResult AddPhoneNumber()
        {
            return this.View();
        }

        // POST: /Manage/AddPhoneNumber
        [HttpPost, ActionName(AddPhoneNumberActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // Generate the token and send it
            var code = await this.UserManager.GenerateChangePhoneNumberTokenAsync(this.UserId, model.Number);

            if (this.UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = string.Format(Strings.SecurityCodeSmsBody, code)
                };

                await this.UserManager.SmsService.SendAsync(message);
            }

            return this.RedirectToAction(VerifyPhoneNumberActionName, new { PhoneNumber = model.Number });
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost, ActionName(EnableTwoFactorAuthenticationActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(this.UserId, true);
            var user = await this.UserManager.FindByIdAsync(this.UserId);
            if (user != null)
            {
                await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return this.RedirectToAction(IndexActionName);
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost, ActionName(DisableTwoFactorAuthenticationActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(this.UserId, false);
            var user = await this.UserManager.FindByIdAsync(this.UserId);
            if (user != null)
            {
                await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return this.RedirectToAction(IndexActionName);
        }

        // GET: /Manage/VerifyPhoneNumber
        [HttpGet, ActionName(VerifyPhoneNumberActionName)]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await this.UserManager.GenerateChangePhoneNumberTokenAsync(this.UserId, phoneNumber);

            // Send an SMS through the SMS provider to verify the phone number
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return this.View(ViewNames.Error);
            }

            return this.View(new VerifyPhoneNumberViewModel
            {
                PhoneNumber = phoneNumber
            });
        }

        // POST: /Manage/VerifyPhoneNumber
        [HttpPost, ActionName(VerifyPhoneNumberActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.UserManager.ChangePhoneNumberAsync(this.UserId, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    var user = await this.UserManager.FindByIdAsync(this.UserId);
                    if (user != null)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return this.RedirectToAction(IndexActionName, new { Message = ManageMessageId.AddPhoneSuccess });
                }

                this.AddErrors(Strings.FailedToVerifyPhoneMessage);
            }

            return this.View(model);
        }

        // POST: /Manage/RemovePhoneNumber
        [HttpPost, ActionName(RemovePhoneNumberActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await this.UserManager.SetPhoneNumberAsync(this.UserId, null);
            if (!result.Succeeded)
            {
                return this.RedirectToAction(IndexActionName, new { Message = ManageMessageId.Error });
            }

            var user = await this.UserManager.FindByIdAsync(this.UserId);
            if (user != null)
            {
                await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return this.RedirectToAction(IndexActionName, new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        // GET: /Manage/ChangePassword
        [HttpGet, ActionName(ChangePasswordActionName)]
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost, ActionName(ChangePasswordActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.UserManager.ChangePasswordAsync(this.UserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await this.UserManager.FindByIdAsync(this.UserId);
                    if (user != null)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return this.RedirectToAction(IndexActionName, new { Message = ManageMessageId.ChangePasswordSuccess });
                }

                this.AddErrors(result);
            }

            return this.View(model);
        }

        // GET: /Manage/SetPassword
        [HttpGet, ActionName(SetPasswordActionName)]
        public ActionResult SetPassword()
        {
            return this.View();
        }

        // POST: /Manage/SetPassword
        [HttpPost, ActionName(SetPasswordActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.UserManager.AddPasswordAsync(this.UserId, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await this.UserManager.FindByIdAsync(this.UserId);
                    if (user != null)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return this.RedirectToAction(IndexActionName, new { Message = ManageMessageId.SetPasswordSuccess });
                }

                this.AddErrors(result);
            }

            return this.View(model);
        }

        // GET: /Manage/ManageLogins
        [HttpGet, ActionName(ManageLoginsActionName)]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? Strings.RemoveLoginSuccessMessage :
                message == ManageMessageId.Error ? Strings.ErrorMessage :
                string.Empty;

            var user = await this.UserManager.FindByIdAsync(this.UserId);
            if (user == null)
            {
                return this.View(ViewNames.Error);
            }

            var userLogins = await this.UserManager.GetLoginsAsync(this.UserId);
            var otherLogins = this.AuthenticationManager.GetExternalAuthenticationTypes()
                .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
                .ToList();

            this.ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return this.View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/LinkLogin
        [HttpPost, ActionName(LinkLoginActionName)]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, this.Url.Action(LinkLoginCallbackActionName, ControllerName), this.UserId);
        }

        // GET: /Manage/LinkLoginCallback
        [HttpGet, ActionName(LinkLoginCallbackActionName)]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(AccountController.ChallengeResult.XsrfKey, this.UserId);
            if (loginInfo == null)
            {
                return this.RedirectToAction(ManageLoginsActionName, new { Message = ManageMessageId.Error });
            }

            var result = await this.UserManager.AddLoginAsync(this.UserId, loginInfo.Login);
            if (result.Succeeded)
            {
                return this.RedirectToAction(ManageLoginsActionName);
            }

            return this.RedirectToAction(ManageLoginsActionName, new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }

        private bool HasPassword()
        {
            var user = this.UserManager.FindById(this.UserId);
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = this.UserManager.FindById(this.UserId);
            if (user != null)
            {
                return user.PhoneNumber != null;
            }

            return false;
        }
    }
}
