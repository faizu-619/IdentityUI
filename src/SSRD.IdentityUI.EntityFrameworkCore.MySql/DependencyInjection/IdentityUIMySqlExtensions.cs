using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SSRD.IdentityUI.Core.DependencyInjection;
using SSRD.IdentityUI.Core.Infrastructure.Data;
using SSRD.IdentityUI.Core.Infrastructure.Data.Providers.MySql;
using SSRD.IdentityUI.Core.Infrastructure.Data.ReleaseManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSRD.IdentityUI.EntityFrameworkCore.MySql.DependencyInjection
{
    public static class IdentityUIMySqlExtensions
    {
        public static IdentityUIServicesBuilder UseMySql(this IdentityUIServicesBuilder builder)
        {
            builder.UseMySql(builder.DatabaseOptions.ConnectionString);

            return builder;
        }

        public static IdentityUIServicesBuilder UseMySql(this IdentityUIServicesBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseMySQL(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(IdentityUIMySqlExtensions).Assembly.FullName);
                }); 
            });

            builder.Services.AddSingleton<IUpdateList, MySqlUpdateList>();

            return builder;
        }
    }
}
