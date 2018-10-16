using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskProject.Models;
using TaskProject.Models.AccountViewModels;
using TaskProject.Services.EmailSender;
using TaskProject.Services.UriHelper;

namespace TaskProject.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            ApplicationDbContext db)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        [TempData] public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                model.RememberMe = true;
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await db.AddLogAccess(model.Email, "Пользователь вошел",
                        Request.Headers["User-Agent"].ToString());

                    return RedirectToLocal(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    await db.AddLogAccess(model.Email, "Пользователю необходима двухфакторная аутентификация",
                        Request.Headers["User-Agent"].ToString());

                    return RedirectToAction(nameof(LoginWith2fa), new {returnUrl, model.RememberMe});
                }

                if (result.IsLockedOut)
                {
                    await db.AddLogAccess(model.Email, "Пользователь заблокирован",
                        Request.Headers["User-Agent"].ToString());

                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    await db.AddLogAccess(model.Email, "Пользователь неверно ввел пароль",
                        Request.Headers["User-Agent"].ToString());

                    ModelState.AddModelError(string.Empty, "Неверно набран логин");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                logger.LogWarning($"Не удается установить двухфакторную аутентификацию пользователя");
                throw new ApplicationException($"Не удается установить двухфакторную аутентификацию пользователя");
            }

            var model = new LoginWith2faViewModel {RememberMe = rememberMe};
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe,
            string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                logger.LogWarning( $"Не удается загрузить пользователя с ID '{userManager.GetUserId(User)}'.");

                throw new ApplicationException(
                    $"Не удается загрузить пользователя с ID '{userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result =
                await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe,
                    model.RememberMachine);

            if (result.Succeeded)
            {
                await db.AddLogAccess(user.Email, "Пользователь вошел с помощью 2fa",
                    Request.Headers["User-Agent"].ToString());

                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                await db.AddLogAccess(user.Email, "Пользователь с 2fa заблокирован",
                    Request.Headers["User-Agent"].ToString());
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                await db.AddLogAccess(user.Email, "Неверный код аутентификации пользователя",
                    Request.Headers["User-Agent"].ToString());

                ModelState.AddModelError(string.Empty, "Неверный код аутентификации");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                logger.LogWarning($"Не удается установить двухфакторную аутентификацию пользователя");
                throw new ApplicationException($"Не удается установить двухфакторную аутентификацию пользователя");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model,
            string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                logger.LogWarning( $"Не удается установить двухфакторную аутентификацию пользователя");
                throw new ApplicationException($"Не удается установить двухфакторную аутентификацию пользователя");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                await db.AddLogAccess(user.Email, "Пользователь вошел с кодом восстановления",
                   Request.Headers["User-Agent"].ToString());
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                await db.AddLogAccess(user.Email, "Пользователь заблокирован",
                    Request.Headers["User-Agent"].ToString());
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                await db.AddLogAccess(user.Email, "Пользователь неверно ввел код восстановления",
                    Request.Headers["User-Agent"].ToString());
                ModelState.AddModelError(string.Empty, "Неверно введен код восстановления");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await db.AddLogAccess(user.Email, "Новый пользователь создан",
                        Request.Headers["User-Agent"].ToString());

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            string email = (User != null) ? User.Identity.Name : "";
            await db.AddLogAccess( email, "Пользователь вышел",
                Request.Headers["User-Agent"].ToString());

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new {returnUrl});
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                logger.LogWarning( $"Ошибка внешнего сервиса: {remoteError}");
                ErrorMessage = $"Ошибка внешнего сервиса: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                await db.AddLogAccess( info.Principal.Identity.Name , string.Format("Пользователь зашел с помощью {0}",info.LoginProvider),
                    Request.Headers["User-Agent"].ToString());

                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel {Email = email});
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model,
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    logger.LogWarning( $"Ошибка загрузки информации от внешнего сервиса");
                    throw new ApplicationException("Ошибка загрузки информации от внешнего сервиса");
                }

                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);

                        await db.AddLogAccess(info.Principal.Identity.Name, string.Format("Пользователь создал аккаунт с помощью {0}", info.LoginProvider),
                            Request.Headers["User-Agent"].ToString());
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning( $"Невозможно загрузить пользователя с ID '{userId}'.");
                throw new ApplicationException($"Невозможно загрузить пользователя с ID '{userId}'.");
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await emailSender.SendEmailToUserAsync(model.Email, "Сброс пароля",
                    $"Пожалуйста сбросьте свой пароль нажав здесь: <a href='{callbackUrl}'>Сбросить пароль</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                logger.LogWarning( $"A code must be supplied for password reset.");
                throw new ApplicationException("A code must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel {Code = code};
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}