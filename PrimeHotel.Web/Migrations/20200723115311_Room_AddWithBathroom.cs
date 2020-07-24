using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class Room_AddWithBathroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WithBathroom",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithBathroom",
                table: "Rooms");
        }
    }
}
