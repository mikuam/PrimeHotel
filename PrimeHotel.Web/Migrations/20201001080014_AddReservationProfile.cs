using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class AddReservationProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Reservations_ReservationId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_ReservationId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Profiles");

            migrationBuilder.CreateTable(
                name: "ReservationProfile",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationProfile", x => new { x.ReservationId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ReservationProfile_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationProfile_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationProfile_ProfileId",
                table: "ReservationProfile",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationProfile");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ReservationId",
                table: "Profiles",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Reservations_ReservationId",
                table: "Profiles",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
