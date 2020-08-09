﻿using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SSRD.IdentityUI.Core.Data.Entities.Identity;
using SSRD.IdentityUI.Core.Data.Models.Constants;
using SSRD.IdentityUI.Core.DependencyInjection;
using SSRD.IdentityUI.Core.Infrastructure.Data;
using SSRD.IdentityUI.Core.Infrastructure.Data.ReleaseManagment;
using SSRD.IdentityUI.Core.Infrastructure.Data.Repository;
using SSRD.IdentityUI.Core.Infrastructure.Data.Seeders;
using SSRD.IdentityUI.Core.Infrastructure.Services;
using SSRD.IdentityUI.Core.Interfaces;
using SSRD.IdentityUI.Core.Interfaces.Data.Repository;
using SSRD.IdentityUI.Core.Interfaces.Services;
using SSRD.IdentityUI.Core.Interfaces.Services.Auth;
using SSRD.IdentityUI.Core.Interfaces.Services.Group;
using SSRD.IdentityUI.Core.Interfaces.Services.Role;
using SSRD.IdentityUI.Core.Models.Options;
using SSRD.IdentityUI.Core.Services;
using SSRD.IdentityUI.Core.Services.Auth;
using SSRD.IdentityUI.Core.Services.Auth.Email;
using SSRD.IdentityUI.Core.Services.Auth.TwoFactorAuth;
using SSRD.IdentityUI.Core.Services.Email;
using SSRD.IdentityUI.Core.Services.Group;
using SSRD.IdentityUI.Core.Services.Identity;
using SSRD.IdentityUI.Core.Services.Permission;
using SSRD.IdentityUI.Core.Services.Role;
using SSRD.IdentityUI.Core.Services.Role.Models;
using SSRD.IdentityUI.Core.Services.User;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSRD.IdentityUI.Core
{
    public static class IdentityUICoreExtension
    {
        /// <summary>
        /// Configures IdentityUI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder ConfigureIdentityUI(this IServiceCollection services, IConfiguration configuration)
        {
            return ConfigureIdentityUI(services, configuration, null);
        }

        /// <summary>
        /// Configures IdentityUI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="endpointAction"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder ConfigureIdentityUI(this IServiceCollection services, IConfiguration configuration,
            Action<IdentityUIEndpoints> endpointAction)
        {
            IdentityUIOptions identityUIOptions = configuration.GetSection("IdentityUI").Get<IdentityUIOptions>();

            if (identityUIOptions == null)
            {
                identityUIOptions = new IdentityUIOptions();
            }

            services.Configure<IdentityUIOptions>(options =>
            {
                options.BasePath = identityUIOptions.BasePath;
                options.Database = identityUIOptions.Database;
                options.EmailSender = identityUIOptions.EmailSender;
            });

            services.Configure<DatabaseOptions>(options =>
            {
                options.Type = identityUIOptions.Database?.Type ?? DatabaseTypes.InMemory;
                options.ConnectionString = identityUIOptions.Database?.ConnectionString;
            });

            services.Configure<EmailSenderOptions>(options =>
            {
                options.Ip = identityUIOptions.EmailSender?.Ip;
                options.Port = identityUIOptions.EmailSender?.Port ?? -1;
                options.UserName = identityUIOptions.EmailSender?.UserName;
                options.Password = identityUIOptions.EmailSender?.Password;
                options.SenderName = identityUIOptions.EmailSender?.SenderName;
            });

            IdentityUIEndpoints identityManagementEndpoints = new IdentityUIEndpoints();
            endpointAction?.Invoke(identityManagementEndpoints);

            if (!identityManagementEndpoints.UseEmailSender.HasValue)
            {
                if (identityUIOptions.EmailSender == null || string.IsNullOrEmpty(identityUIOptions.EmailSender.Ip))
                {
                    identityManagementEndpoints.UseEmailSender = false;
                }
                else
                {
                    identityManagementEndpoints.UseEmailSender = true;
                }
            }

            services.Configure<IdentityUIEndpoints>(options =>
            {
                options.Home = identityManagementEndpoints.Home;

                options.Login = identityManagementEndpoints.Login;
                options.Logout = identityManagementEndpoints.Logout;
                options.AccessDenied = identityManagementEndpoints.AccessDenied;

                options.Manage = identityManagementEndpoints.Manage;

                options.ConfirmeEmail = identityManagementEndpoints.ConfirmeEmail;
                options.ResetPassword = identityManagementEndpoints.ResetPassword;
                options.AcceptInvite = identityManagementEndpoints.AcceptInvite;

                options.RegisterEnabled = identityManagementEndpoints.RegisterEnabled;
                options.UseEmailSender = identityManagementEndpoints.UseEmailSender;
                options.UseSmsGateway = identityManagementEndpoints.UseSmsGateway;
                options.InviteValidForTimeSpan = identityManagementEndpoints.InviteValidForTimeSpan;
            });

            IdentityUIServicesBuilder builder = new IdentityUIServicesBuilder(services, identityManagementEndpoints);

            builder.Services.AddScoped<IEmailSender, NullEmailSender>();
            builder.Services.AddScoped<ISmsSender, NullSmsSender>();

            return builder;
        }

        /// <summary>
        /// Registers IdentityUI services
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="identityOptions"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder AddIdentityUI(this IdentityUIServicesBuilder builder,
            Action<IdentityOptions> identityOptions)
        {
            builder.Services.AddDbContext<IdentityDbContext>((provider, options) =>
            {
                DatabaseOptions databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>().Value;
                if (databaseOptions == null)
                {
                    throw new Exception("No DatabaseOptions");
                }

                switch (databaseOptions.Type)
                {
                    case DatabaseTypes.PostgreSql:
                        {
                            options.UseNpgsql(databaseOptions.ConnectionString);

                            break;
                        }
                    case DatabaseTypes.InMemory:
                        {
                            options.UseInMemoryDatabase(databaseOptions.ConnectionString);
                            break;
                        }
                    default:
                        {
                            throw new Exception($"Unsupported Database: {databaseOptions.Type}");
                        }
                }
            });

            builder.Services.AddIdentity<AppUserEntity, RoleEntity>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            builder.AddValidators();
            builder.AddRepositories();
            builder.AddSeeders();
            builder.AddServices();

            builder.Services.AddScoped<IGroupStore, GroupStore>();
            builder.Services.AddScoped<IGroupUserStore, GroupUserStore>();

            builder.Services.AddScoped<ReleaseManagement>();

            builder.Services.Configure<IdentityOptions>(identityOptions);
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(1);
                options.OnRefreshingPrincipal = context =>
                {
                    Claim sessionCode = context.CurrentPrincipal.FindFirst(IdentityUIClaims.SESSION_CODE);
                    if (sessionCode != null)
                    {
                        context.NewPrincipal.Identities.First().AddClaim(sessionCode);
                    }

                    return Task.CompletedTask;
                };
            });

            builder.Services.AddSession();

            return builder;
        }

        /// <summary>
        /// Registers IdentityUI services
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder AddIdentityUI(this IdentityUIServicesBuilder builder)
        {
            Action<IdentityOptions> identityOptions = new Action<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            return builder.AddIdentityUI(identityOptions);
        }

        class Config
        {
            public bool IsEmailConfigured { get; set; }
        }

        /// <summary>
        /// Registers EmailSender
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder AddEmailSender(this IdentityUIServicesBuilder builder)
        {
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            return builder;
        }

        /// <summary>
        /// Configures AuthenticationCookie
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder AddAuth(this IdentityUIServicesBuilder builder)
        {
            Action<CookieAuthenticationOptions> cookieAuthenticationOptions = new Action<CookieAuthenticationOptions>(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = builder.IdentityManagementEndpoints.Login;
                options.AccessDeniedPath = builder.IdentityManagementEndpoints.AccessDenied;
                options.SlidingExpiration = true;
                options.LogoutPath = builder.IdentityManagementEndpoints.Logout;
            });

            builder.AddAuth(cookieAuthenticationOptions);

            return builder;
        }

        /// <summary>
        /// Configures AuthenticationCookie
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IdentityUIServicesBuilder AddAuth(this IdentityUIServicesBuilder builder, Action<CookieAuthenticationOptions> optionsAction)
        {
            builder.Services.ConfigureApplicationCookie(optionsAction);

            return builder;
        }

        private static void AddRepositories(this IdentityUIServicesBuilder builder)
        {
            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IRoleRepository, RoleRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();

            builder.Services.AddTransient<ISessionRepository, SessionRepository>();

            builder.Services.AddTransient(typeof(IBaseRepositoryAsync<>), typeof(BaseRepositoryAsync<>));
        }

        private static void AddSeeders(this IdentityUIServicesBuilder builder)
        {
            builder.Services.AddScoped<SystemEntitySeeder>();
            builder.Services.AddScoped<AdminSeeder>();
            builder.Services.AddScoped<UserSeeder>();
        }

        private static void AddServices(this IdentityUIServicesBuilder builder)
        {
            builder.Services.AddScoped<IManageUserService, ManageUserService>();
            builder.Services.AddScoped<IAddUserService, AddUserService>();

            builder.Services.AddScoped<IRoleService, RoleService>();

            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IRoleAssignmentService, RoleAssignmentService>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
            builder.Services.AddScoped<IGroupUserService, GroupUserService>();
            builder.Services.AddScoped<IGroupAttributeService, GroupAttributeService>();

            builder.Services.AddScoped<IPermissionService, PermissionService>();

            builder.Services.AddScoped<IInviteService, InviteService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IManageEmailService, ManageEmailService>();

            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();
            builder.Services.AddScoped<Interfaces.Services.Auth.IEmailConfirmationService, EmailConfirmationService>();

            builder.Services.AddScoped<ICredentialsService, Core.Services.Auth.Credentials.CredentialsService>();

            builder.Services.AddScoped<ISessionService, Services.Auth.Session.SessionService>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUserEntity>, CustomClaimsPrincipalFactory>();
            builder.Services.AddScoped<ISecurityStampValidator, CustomSecurityStampValidator>();
        }

        private static void AddValidators(this IdentityUIServicesBuilder builder)
        {
            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.EditUserRequest>, Core.Services.User.Models.EditUserRequestValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.SetNewPasswordRequest>, Core.Services.User.Models.SetNewPasswordValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.NewUserRequest>, Core.Services.User.Models.NewUserRequestValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.RegisterRequest>, Core.Services.User.Models.RegisterRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.User.Models.AcceptInviteRequest>, Services.User.Models.AcceptInviteRequestValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Login.Models.LoginRequest>, Core.Services.Auth.Login.Models.LoginRequestValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Login.Models.LoginWith2faRequest>, Core.Services.Auth.Login.Models.LoginWith2faRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Auth.Login.Models.LoginWithRecoveryCodeRequest>, Services.Auth.Login.Models.LoginWithRecoveryCodeRequestValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorAuthenticatorRequest>, Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorAuthicatorValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorPhoneAuthenticationRequest>, Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorPhoneAuthenticationRequestValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorEmailAuthenticationRequest>, Core.Services.Auth.TwoFactorAuth.Models.AddTwoFactorEmailAuthenticationRequestValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.Role.Models.NewRoleRequest>, Core.Services.Role.Models.NewRoleValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Role.Models.EditRoleRequest>, Core.Services.Role.Models.EditRoleValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Credentials.Models.RecoverPasswordRequest>, Core.Services.Auth.Credentials.Models.RecoverPasswordValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Credentials.Models.ResetPasswordRequest>, Core.Services.Auth.Credentials.Models.ResetPasswordValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Credentials.Models.ChangePasswordRequest>, Core.Services.Auth.Credentials.Models.ChangePasswordValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.EditProfileRequest>, Core.Services.User.Models.EditProfileValidator>();

            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.UnlockUserRequest>, Core.Services.User.Models.UnlockUserValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.User.Models.SendEmailVerificationMailRequest>, Core.Services.User.Models.SendEmailVerificationMailValidtor>();

            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Session.Models.LogoutSessionRequest>, Core.Services.Auth.Session.Models.LogoutSessionValidator>();
            builder.Services.AddSingleton<IValidator<Core.Services.Auth.Session.Models.LogoutUserSessionsRequest>, Core.Services.Auth.Session.Models.LogoutUserSessionValidator>();

            builder.Services.AddSingleton<IValidator<AddRoleAssignmentRequest>, AddRoleAssignmentRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.Group.Models.AddGroupRequest>, Services.Group.Models.AddGroupRequestValidator>();

            builder.Services.AddSingleton<IValidator<AddRolePermissionRequest>, Services.Role.Models.AddRolePermissionRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.Group.Models.AddExistingUserRequest>, Services.Group.Models.AddExisingUserRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Group.Models.InviteUserToGroupRequest>, Services.Group.Models.InviteUserToGroupRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.Group.Models.AddGroupAttributeRequest>, Services.Group.Models.AddGroupAttributeRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Group.Models.EditGroupAttributeRequest>, Services.Group.Models.EditGroupAttributeRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.Permission.Models.AddPermissionRequest>, Services.Permission.Models.AddPermissionRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Permission.Models.EditPermissionRequest>, Services.Permission.Models.EditPermissionRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.Email.Models.AddEmailRequest>, Services.Email.Models.AddEmailRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Email.Models.EditEmailRequest>, Services.Email.Models.EditEmailRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.Email.Models.SendTesEmailRequest>, Services.Email.Models.SendTestEmailRequestValidator>();

            builder.Services.AddSingleton<IValidator<Services.User.Models.InviteToGroupRequest>, Services.User.Models.InviteToGroupRequestValidator>();
            builder.Services.AddSingleton<IValidator<Services.User.Models.InviteRequest>, Services.User.Models.InviteRequestValidatior>();
        }

        /// <summary>
        /// Adds IdentityUI to the specified Microsoft.AspNetCore.Builder.IApplicationBuilder
        /// </summary>
        /// <param name="app"></param>
        /// <param name="enableMigrations">Flag indicating if migrations should be run</param>
        /// <returns></returns>
        public static IdentityUIAppBuilder UseIdentityUI(this IApplicationBuilder app, bool enableMigrations = false)
        {
            app.UseSession();
            app.UseAuthentication();
#if NET_CORE3
            app.UseAuthorization();
#endif

            if (enableMigrations)
            {
                app.RunIdentityMigrations();
            }

            return new IdentityUIAppBuilder(app);
        }
    }
}
