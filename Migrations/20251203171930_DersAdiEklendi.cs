using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webOdev.Migrations
{
    /// <inheritdoc />
    public partial class DersAdiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcilanDersler");

            migrationBuilder.DropTable(
                name: "DersProgramlari");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropColumn(
                name: "SeansUcreti",
                table: "Antrenorler");

            migrationBuilder.RenameColumn(
                name: "IslemTarihi",
                table: "Randevular",
                newName: "OlusturulmaTarihi");

            migrationBuilder.RenameColumn(
                name: "TarihSaat",
                table: "DersSeanslari",
                newName: "Tarih");

            migrationBuilder.AddColumn<string>(
                name: "DersAdi",
                table: "DersSeanslari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Kapasite",
                table: "DersSeanslari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SaatAraligi",
                table: "DersSeanslari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Ucret",
                table: "DersSeanslari",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DersAdi",
                table: "DersSeanslari");

            migrationBuilder.DropColumn(
                name: "Kapasite",
                table: "DersSeanslari");

            migrationBuilder.DropColumn(
                name: "SaatAraligi",
                table: "DersSeanslari");

            migrationBuilder.DropColumn(
                name: "Ucret",
                table: "DersSeanslari");

            migrationBuilder.RenameColumn(
                name: "OlusturulmaTarihi",
                table: "Randevular",
                newName: "IslemTarihi");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "DersSeanslari",
                newName: "TarihSaat");

            migrationBuilder.AddColumn<decimal>(
                name: "SeansUcreti",
                table: "Antrenorler",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sure = table.Column<int>(type: "integer", nullable: false),
                    Ucret = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DersProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AntrenorId = table.Column<int>(type: "integer", nullable: false),
                    HizmetId = table.Column<int>(type: "integer", nullable: false),
                    BaslangicSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    BitisSaati = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Gun = table.Column<int>(type: "integer", nullable: false),
                    Kapasite = table.Column<int>(type: "integer", nullable: false),
                    Ucret = table.Column<decimal>(type: "numeric", nullable: false)
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
        }
    }
}
