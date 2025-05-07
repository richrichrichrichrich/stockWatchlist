using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsxWatchlist.Migrations
{
    /// <inheritdoc />
    public partial class AddStopLossFieldsToWatchlistItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "StopLossPercent",
                table: "WatchlistItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StopLossPrice",
                table: "WatchlistItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StopLossPercent",
                table: "UserConfigs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StopLossPercent",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "StopLossPrice",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "StopLossPercent",
                table: "UserConfigs");
        }
    }
}
