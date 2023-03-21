using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovies.Server.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                GO

                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                    SET IDENTITY_INSERT [AspNetUsers] ON;
                INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
                VALUES (N'6ab4b43d-bf75-48d1-b398-d1aa3907dc6c', 0, N'f666c926-ca4c-4888-b177-11ee50cbd2e8', N'admin@test.com', CAST(1 AS bit), CAST(0 AS bit), NULL, N'admin@test.com', N'admin@test.com', N'AQAAAAIAAYagAAAAELG4QPpDmPwXQL6yy3cJXtyKd1QiaCSqBK87FEae149xWobla/knl7hd7+P4FpYFaw==', NULL, CAST(0 AS bit), N'7e470d27-e840-41a2-a83b-ab4455282970', CAST(0 AS bit), N'admin@test.com');
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                    SET IDENTITY_INSERT [AspNetUsers] OFF;
                GO

                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                    SET IDENTITY_INSERT [AspNetUserClaims] ON;
                INSERT INTO [AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId])
                VALUES (1, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin', N'6ab4b43d-bf75-48d1-b398-d1aa3907dc6c');
                IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                    SET IDENTITY_INSERT [AspNetUserClaims] OFF;
                GO

                INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                VALUES (N'20230321185852_AdminUser', N'7.0.3');
                GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6ab4b43d-bf75-48d1-b398-d1aa3907dc6c");
        }
    }
}
