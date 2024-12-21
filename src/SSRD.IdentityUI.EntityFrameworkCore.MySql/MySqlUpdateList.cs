using SSRD.IdentityUI.Core.Infrastructure.Data.ReleaseManagment;
using SSRD.IdentityUI.EntityFrameworkCore.MySql.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSRD.IdentityUI.Core.Infrastructure.Data.Providers.MySql
{
    internal class MySqlUpdateList : IUpdateList
    {
        private static readonly List<IUpdate> _updates = new List<IUpdate>()
        {
            new Update_20241220225256_Initial()
        };

        public IEnumerable<IUpdate> Get()
        {
            return _updates
                .OrderBy(x => x.MigrationId);
        }
    }
}
