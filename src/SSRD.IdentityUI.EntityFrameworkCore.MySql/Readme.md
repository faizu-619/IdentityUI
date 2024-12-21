1. Add Migration 
  - pmc: `Add-Migration {MIGRATION_NAME} -StartUpProject IdentityUI.Dev -Project SSRD.IdentityUI.EntityFrameworkCore.MySql -OutputDir Migrations`
  - dotnet cli: `dotnet ef migrations add {MIGRATION_NAME} --startup-project IdentityUI.Dev --project SSRD.IdentityUI.EntityFrameworkCore.MySql --output-dir Migrations`
2. Script Migration
  - pmc: `Script-Migration -From {LAST_MIGRATION_NAME} -Output SSRD.IdentityUI.EntityFrameworkCore.MySql/Scripts/Migrations/{GENERATED_MIGRATION_NAME}.sql`
  - dotnet cli: `dotnet ef migrations script {LAST_MIGRATION_NAME} --output SSRD.IdentityUI.EntityFrameworkCore.MySql/Scripts/Migrations/{GENERATED_MIGRATION_NAME}.sql --startup-project IdentityUI.Dev --project SSRD.IdentityUI.EntityFrameworkCore.MySql`

	**When sql script is generated remove `GO` statements from the script.**

3. Add Update class to `SSRD.IdentityUI.EntityFrameworkCore.MySql/Updates`
- Update class name format `Update_{GENERATED_MIGRATION_NAME}`
- Update class must implement `MySqlUpdate`
- In Update class you need to override MigrationId with `{GENERATED_MIGRATION_NAME}`
4. Add your Update class to `MySqlUpdateList._updates`