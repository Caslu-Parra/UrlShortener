using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class PkAdjustment_ModelAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_addresses",
                table: "addresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_addresses",
                table: "addresses",
                column: "Shortned");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_Url",
                table: "addresses",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_addresses",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "IX_addresses_Url",
                table: "addresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_addresses",
                table: "addresses",
                columns: new[] { "Url", "Shortned" });
        }
    }
}
