using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class Profile_AddCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Profiles");
        }
    }
}
