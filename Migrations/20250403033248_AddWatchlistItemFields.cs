using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsxWatchlist.Migrations
{
    /// <inheritdoc />
    public partial class AddWatchlistItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistItems_AspNetUsers_UserId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId",
                table: "WatchlistItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetSellPrice",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetBuyPrice",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<decimal>(
                name: "LastKnownPrice",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "WatchlistItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WatchlistItems",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastKnownPrice",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WatchlistItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetSellPrice",
                table: "WatchlistItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetBuyPrice",
                table: "WatchlistItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId",
                table: "WatchlistItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistItems_AspNetUsers_UserId",
                table: "WatchlistItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
