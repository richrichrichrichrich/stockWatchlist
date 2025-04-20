using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsxWatchlist.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioAndAnalysisFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnalysisNotes",
                table: "WatchlistItems",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageBuyPrice",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CapSize",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Dividend",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsiderBuying",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsiderSelling",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTradeDate",
                table: "WatchlistItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastTradeNote",
                table: "WatchlistItems",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LastTradePrice",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LastTradeQuantity",
                table: "WatchlistItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastTradeType",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PERatio",
                table: "WatchlistItems",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityHeld",
                table: "WatchlistItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrendGraphUrl",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnfulfilledOrders",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valuation",
                table: "WatchlistItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisNotes",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "AverageBuyPrice",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "CapSize",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Dividend",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "InsiderBuying",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "InsiderSelling",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastTradeDate",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastTradeNote",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastTradePrice",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastTradeQuantity",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "LastTradeType",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "PERatio",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "QuantityHeld",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "TrendGraphUrl",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "UnfulfilledOrders",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Valuation",
                table: "WatchlistItems");
        }
    }
}
