using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class spUpdateProfilesCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                IF OBJECT_ID('UpdateProfilesCountry', 'P') IS NOT NULL
                DROP PROC UpdateProfilesCountry
                GO

                CREATE PROCEDURE [dbo].[UpdateProfilesCountry]
                    @StardId int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE Profiles SET Country = 'Poland' WHERE LEFT(TelNo, 2) = '48' AND Id > @StardId
                END";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROC UpdateProfilesCountry");
        }
    }
}
