using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeHotel.Web.Migrations
{
    public partial class UpdateGetGuestsForDate : Migration
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
                    JOIN ProfileReservation pr ON pr.ProfilesId = p.Id
                    JOIN Reservations r ON pr.ReservationsId = r.Id
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
