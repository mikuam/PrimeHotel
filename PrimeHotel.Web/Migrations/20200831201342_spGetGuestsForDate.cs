using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class spGetGuestsForDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                IF OBJECT_ID('GetGuestsForDate', 'P') IS NOT NULL
                DROP PROC GetGuestsForDate
                GO

                CREATE PROCEDURE [dbo].[GetGuestsForDate]
                    @StartDate varchar(20)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT p.Forename, p.Surname, p.TelNo, r.[From], r.[To], ro.Number As RoomNumber
                    FROM Profiles p
                    JOIN Reservations r ON p.ReservationId = p.ReservationId
                    JOIN Rooms ro ON r.RoomId = ro.Id
                    WHERE CAST([From] AS date) = CONVERT(date, @StartDate, 105)
                END";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROC GetGuestsForDate");
        }
    }
}
