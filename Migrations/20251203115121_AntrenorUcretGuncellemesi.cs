using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webOdev.Migrations
{
    /// <inheritdoc />
    public partial class AntrenorUcretGuncellemesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_AcilanDersler_AcilanDersId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "KayitTarihi",
                table: "Randevular",
                newName: "RandevuTarihi");

            migrationBuilder.RenameColumn(
                name: "AcilanDersId",
                table: "Randevular",
                newName: "AntrenorId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_AcilanDersId",
                table: "Randevular",
                newName: "IX_Randevular_AntrenorId");

            migrationBuilder.AddColumn<int>(
                name: "HizmetId",
                table: "Randevular",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SeansUcreti",
                table: "Antrenorler",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AntrenorMusaitlikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AntrenorId = table.Column<int>(type: "integer", nullable: false),
                    Gun = table.Column<int>(type: "integer", nullable: false),
                    BaslangicSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    BitisSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorMusaitlikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntrenorMusaitlikleri_Antrenorler_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "Antrenorler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_HizmetId",
                table: "Randevular",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorMusaitlikleri_AntrenorId",
                table: "AntrenorMusaitlikleri",
                column: "AntrenorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular",
                column: "AntrenorId",
                principalTable: "Antrenorler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular",
                column: "HizmetId",
                principalTable: "Hizmetler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular");

            migrationBuilder.DropTable(
                name: "AntrenorMusaitlikleri");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_HizmetId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "HizmetId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "SeansUcreti",
                table: "Antrenorler");

            migrationBuilder.RenameColumn(
                name: "RandevuTarihi",
                table: "Randevular",
                newName: "KayitTarihi");

            migrationBuilder.RenameColumn(
                name: "AntrenorId",
                table: "Randevular",
                newName: "AcilanDersId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_AntrenorId",
                table: "Randevular",
                newName: "IX_Randevular_AcilanDersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_AcilanDersler_AcilanDersId",
                table: "Randevular",
                column: "AcilanDersId",
                principalTable: "AcilanDersler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
