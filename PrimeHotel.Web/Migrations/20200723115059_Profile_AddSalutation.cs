using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class Profile_AddSalutation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salutation",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salutation",
                table: "Profiles");
        }
    }
}
