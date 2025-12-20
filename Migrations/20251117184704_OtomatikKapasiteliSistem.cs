using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webOdev.Migrations
{
    /// <inheritdoc />
    public partial class OtomatikKapasiteliSistem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_Randevular_AntrenorId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "AntrenorId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "RandevuTarihi",
                table: "Randevular",
                newName: "KayitTarihi");

            migrationBuilder.RenameColumn(
                name: "HizmetId",
                table: "Randevular",
                newName: "AcilanDersId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_HizmetId",
                table: "Randevular",
                newName: "IX_Randevular_AcilanDersId");

            migrationBuilder.CreateTable(
                name: "DersProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AntrenorId = table.Column<int>(type: "integer", nullable: false),
                    HizmetId = table.Column<int>(type: "integer", nullable: false),
                    Gun = table.Column<int>(type: "integer", nullable: false),
                    BaslangicSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    BitisSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Kapasite = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersProgramlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DersProgramlari_Antrenorler_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "Antrenorler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DersProgramlari_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcilanDersler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DersProgramiId = table.Column<int>(type: "integer", nullable: false),
                    DersTarihiVeSaati = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KalanKapasite = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcilanDersler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcilanDersler_DersProgramlari_DersProgramiId",
                        column: x => x.DersProgramiId,
                        principalTable: "DersProgramlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcilanDersler_DersProgramiId",
                table: "AcilanDersler",
                column: "DersProgramiId");

            migrationBuilder.CreateIndex(
                name: "IX_DersProgramlari_AntrenorId",
                table: "DersProgramlari",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_DersProgramlari_HizmetId",
                table: "DersProgramlari",
                column: "HizmetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_AcilanDersler_AcilanDersId",
                table: "Randevular",
                column: "AcilanDersId",
                principalTable: "AcilanDersler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_AcilanDersler_AcilanDersId",
                table: "Randevular");

            migrationBuilder.DropTable(
                name: "AcilanDersler");

            migrationBuilder.DropTable(
                name: "DersProgramlari");

            migrationBuilder.RenameColumn(
                name: "KayitTarihi",
                table: "Randevular",
                newName: "RandevuTarihi");

            migrationBuilder.RenameColumn(
                name: "AcilanDersId",
                table: "Randevular",
                newName: "HizmetId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_AcilanDersId",
                table: "Randevular",
                newName: "IX_Randevular_HizmetId");

            migrationBuilder.AddColumn<int>(
                name: "AntrenorId",
                table: "Randevular",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AntrenorMusaitlikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AntrenorId = table.Column<int>(type: "integer", nullable: false),
                    BaslangicSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    BitisSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Gun = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Randevular_AntrenorId",
                table: "Randevular",
                column: "AntrenorId");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
