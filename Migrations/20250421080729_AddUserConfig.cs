using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AsxWatchlist.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DefaultTradeAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxHoldingAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CheckTradeFrequency = table.Column<string>(type: "text", nullable: false),
                    CheckHoldingsFrequency = table.Column<string>(type: "text", nullable: false),
                    DefaultExpiryLengthDays = table.Column<int>(type: "integer", nullable: false),
                    TradingDayStart = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TradingDayEnd = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CheckMonday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckTuesday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckWednesday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckThursday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckFriday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckSaturday = table.Column<bool>(type: "boolean", nullable: false),
                    CheckSunday = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConfigs");
        }
    }
}
