﻿using SSRD.IdentityUI.Core.Data.Entities.Identity;
using SSRD.IdentityUI.Core.Models.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSRD.IdentityUI.Core.Interfaces.Services.Auth
{
    public interface IEmailService
    {
        Task<Result> ConfirmEmail(string userId, string code);
        Task<Result> SendVerificationMail(AppUserEntity appUser, string code);
    }
}
