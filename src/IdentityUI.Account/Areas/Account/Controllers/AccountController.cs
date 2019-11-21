﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSRD.IdentityUI.Account.Areas.Account.Models;
using SSRD.IdentityUI.Account.Areas.Account.Models.Account;
using SSRD.IdentityUI.Core.Helper;
using SSRD.IdentityUI.Core.Interfaces.Services;
using SSRD.IdentityUI.Core.Interfaces.Services.Auth;
using SSRD.IdentityUI.Core.Models.Options;
using SSRD.IdentityUI.Core.Models.Result;
using SSRD.IdentityUI.Core.Services.Auth.Credentials.Models;
using SSRD.IdentityUI.Core.Services.Auth.Login.Models;
using SSRD.IdentityUI.Core.Services.User.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SSRD.IdentityUI.Account.Areas.Account.Controllers
{
    [Route("/[area]/[action]")]
    public class AccountController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly IAddUserService _addUserService;
        private readonly IEmailService _emailService;
        private readonly ICredentialsService _credentialsService;
        private readonly IdentityUIEndpoints _identityManagementEndpoints;

        public AccountController(ILoginService loginService, IEmailService emailService, IAddUserService addUserService, ICredentialsService credentialsService,
            IOptionsSnapshot<IdentityUIEndpoints> identityManagmentEndpoints)
        {
            _loginService = loginService;
            _emailService = emailService;
            _addUserService = addUserService;
            _credentialsService = credentialsService;

            _identityManagementEndpoints = identityManagmentEndpoints.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.RetunrUrl = returnUrl;
            ViewBag.RegistrationEnabled = _identityManagementEndpoints.RegisterEnabled;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            Microsoft.AspNetCore.Identity.SignInResult result = await _loginService.Login(GetIp(), GetSessionCode(), loginRequest);
            if(result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else if(result.RequiresTwoFactor)
            {
                return RedirectToAction("LoginWith2fa", new { RememberMe = loginRequest.RememberMe, ReturnUrl = returnUrl});
            }
            else if(result.IsLockedOut)
            {
                return Redirect(PagePath.LOCKOUT);
            }
            else
            {
                ViewBag.RetunrUrl = returnUrl;
                ViewBag.RegistrationEnabled = _identityManagementEndpoints.RegisterEnabled;

                ModelState.AddModelError(string.Empty, "Invalid login attempt");

                return View();
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            ViewBag.RetunrUrl = returnUrl;

            LoginWith2faViewModel loginWith2FaViewModel = new LoginWith2faViewModel(
                rememberMe: rememberMe);

            return View(loginWith2FaViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faRequest loginWith2FaRequest, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            Microsoft.AspNetCore.Identity.SignInResult result = await _loginService.LoginWith2fa(GetIp(), loginWith2FaRequest);
            if(result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else if(result.IsLockedOut)
            {
                return Redirect(PagePath.LOCKOUT);
            }
            else
            {
                ViewBag.RetunrUrl = returnUrl;
                ModelState.AddModelError(string.Empty, "Invalid authenticator code");

                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Result result = await _loginService.Logout(GetUserId(), GetSessionCode());

            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            if (!_identityManagementEndpoints.RegisterEnabled)
            {
                return NotFound();
            }

            ViewBag.RetunrUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest, string returnUrl = null)
        {
            if(!_identityManagementEndpoints.RegisterEnabled)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return View();
            }

            Result result = await _addUserService.Register(registerRequest);
            if(result.Failure)
            {
                ModelState.AddErrors(result.Errors);
                return View();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            return RedirectToAction(nameof(RegisterSuccess));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            ResetPasswordViewModel viewModel = new ResetPasswordViewModel(
                code: code);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            Result result = await _credentialsService.ResetPassword(request);
            if(result.Failure)
            {
                ViewBag.Code = request.Code;

                ModelState.AddErrors(result.Errors);
                return View();
            }

            return Redirect(PagePath.RESET_PASSWORD_SUCCESS);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            Result result = await _credentialsService.RecoverPassword(request);
            if(result.Failure)
            {
                ModelState.AddErrors(result.Errors);
                return View();
            }

            return Redirect(PagePath.RECOVER_PASSWORD_SUCCESS);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RecoverPasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ConfirmEmail(string id, string code)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Result result = await _emailService.ConfirmEmail(id, code);
            if(result.Failure)
            {
                ModelState.AddErrors(result.Errors);
                return View();
            }

            return View();
        }
    }
}
