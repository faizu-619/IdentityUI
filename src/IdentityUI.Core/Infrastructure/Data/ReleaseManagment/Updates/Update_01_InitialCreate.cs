﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SSRD.IdentityUI.Core.Infrastructure.Data.ReleaseManagment.Updates
{
    internal class Update_01_InitialCreate : AbstractUpdate
    {
        protected override string migrationId => "20191105162011_InitialCreate";

        public Update_01_InitialCreate()
        {
        }

        public override void AfterSchemaChange()
        {
        }

        public override void BeforeSchemaChange()
        {
        }

        public override int GetVersion()
        {
            return 1;
        }

        public override void SchemaChange(DatabaseFacade database)
        {
            ExecuteMigrationSql(database, migrationId);
        }
    }
}
