﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SSRD.CommonUtils.Result;
using SSRD.CommonUtils.Specifications;
using SSRD.IdentityUI.Core.Data.Entities.Group;
using SSRD.IdentityUI.Core.Data.Entities.Identity;
using SSRD.IdentityUI.Core.Data.Enums.Entity;
using SSRD.IdentityUI.Core.Interfaces;
using SSRD.IdentityUI.Core.Interfaces.Services;
using SSRD.IdentityUI.Core.Interfaces.Services.Auth;
using SSRD.IdentityUI.Core.Services.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSRD.IdentityUI.Core.Services.Auth.Login
{
    public class ImpersonateService : IImpersonateService
    {
        public const string USER_NOT_FOUND = "user_not_found";
        public const string FAILED_TO_ADD_SESSION = "failed_to_add_session";

        private readonly SignInManager<AppUserEntity> _signInManager;
        private readonly UserManager<AppUserEntity> _userManager;

        private readonly IGroupUserStore _groupUserStore;

        private readonly ISessionService _sessionService;
        private readonly IIdentityUIUserInfoService _identityUIUserInfoService;

        private readonly ILogger<ImpersonateService> _logger;

        public ImpersonateService(
            SignInManager<AppUserEntity> signInManager,
            UserManager<AppUserEntity> userManager,
            IGroupUserStore groupUserStore,
            ISessionService sessionService,
            IIdentityUIUserInfoService identityUIUserInfoService,
            ILogger<ImpersonateService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            _groupUserStore = groupUserStore;

            _sessionService = sessionService;
            _identityUIUserInfoService = identityUIUserInfoService;

            _logger = logger;
        }

        private async Task<Result> Start(AppUserEntity appUser)
        {
            appUser.SessionCode = Guid.NewGuid().ToString();

            Result addSessionResult = await _sessionService.Add(appUser.SessionCode, appUser.Id);
            if (addSessionResult.Failure)
            {
                return Result.Fail(FAILED_TO_ADD_SESSION);
            }

            string loggedInUserId = _identityUIUserInfoService.GetGroupId();

            appUser.ImpersonatorId = loggedInUserId;

            _logger.LogInformation($"User is starting to impersonate another user. ImpersnonazerId {loggedInUserId}, user to be impersonalized {appUser.Id}");

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(appUser, false);

            return Result.Ok();
        }

        public async Task<Result> Start(string userId)
        {
            AppUserEntity appUser = await _userManager.FindByIdAsync(userId);
            if(appUser == null)
            {
                _logger.LogError($"User was not found. UserId {userId}");
                return Result.Fail(USER_NOT_FOUND);
            }

            return await Start(appUser);
        }

        public async Task<Result> Start(long groupUserId)
        {
            var specification = SpecificationBuilder
                .Create<GroupUserEntity>()
                .Where(x => x.Id == groupUserId)
                .Include(x => x.User)
                .Build();

            Result<GroupUserEntity> getGroupUserResult = await _groupUserStore.SingleOrDefault(specification);
            if (getGroupUserResult.Failure)
            {
                return Result.Fail(getGroupUserResult);
            }

            return await Start(getGroupUserResult.Value.User);
        }

        public async Task<Result> Stop()
        {
            string impersonatorId = _identityUIUserInfoService.GetImpersonatorId();

            AppUserEntity appUser = await _userManager.FindByIdAsync(impersonatorId);
            if(appUser == null)
            {
                _logger.LogError($"Impersonator not found. ImpersonatorId {impersonatorId}");
                return Result.Fail(USER_NOT_FOUND);
            }

            appUser.SessionCode = Guid.NewGuid().ToString();

            Result addSessionResult = await _sessionService.Add(appUser.SessionCode, appUser.Id);
            if (addSessionResult.Failure)
            {
                return Result.Fail(FAILED_TO_ADD_SESSION);
            }

            string userId = _identityUIUserInfoService.GetUserId();
            string sessionCode = _identityUIUserInfoService.GetSessionCode();

            await _signInManager.SignOutAsync();

            _sessionService.Logout(sessionCode, userId, SessionEndTypes.ImpersonationLogout);

            await _signInManager.SignInAsync(appUser, false); //TODO: save this when starting impersonating

            _logger.LogInformation($"User is stopped to impersonate another user. ImpersnonazerId {impersonatorId}, user to be impersonalized {userId}");

            return Result.Ok();
        }
    }
}
