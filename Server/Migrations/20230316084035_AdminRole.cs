using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovies.Server.Migrations
{
    /// <inheritdoc />
    public partial class AdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO dbo.AspNetRoles ([Id], [Name], [NormalizedName]) 
            VALUES ('b6f295a1-3456-42c6-b732-10eae02a4117', 'Admin', 'Admin')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
