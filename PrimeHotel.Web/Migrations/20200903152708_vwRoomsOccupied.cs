using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class vwGuestArrivals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                CREATE OR ALTER VIEW [dbo].[vwRoomsOccupied] AS
                    SELECT r.[From], r.[To], ro.Number As RoomNumber, ro.Level, ro.WithBathroom
                    FROM Reservations r
                    JOIN Rooms ro ON r.RoomId = ro.Id";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW vwRoomsOccupied");
        }
    }
}
