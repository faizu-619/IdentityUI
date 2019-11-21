﻿using Microsoft.Extensions.DependencyInjection;
using SSRD.AdminUI.Template.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSRD.AdminUI.Template
{
    public static class IdentityManagementTemplateExtension
    {
        /// <summary>
        /// Registers services for AdminUI.Template
        /// </summary>
        /// <param name="services"></param>
        public static void AddAdminTemplate(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(UIConfigureOptions));
        }
    }
}
