using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webOdev.Migrations
{
    /// <inheritdoc />
    public partial class UcretVeOnaySistemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ucret",
                table: "DersProgramlari",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ucret",
                table: "DersProgramlari");
        }
    }
}
