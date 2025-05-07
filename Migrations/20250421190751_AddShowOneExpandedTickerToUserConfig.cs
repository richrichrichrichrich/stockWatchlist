using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsxWatchlist.Migrations
{
    /// <inheritdoc />
    public partial class AddShowOneExpandedTickerToUserConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowOneExpandedTicker",
                table: "UserConfigs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowOneExpandedTicker",
                table: "UserConfigs");
        }
    }
}
