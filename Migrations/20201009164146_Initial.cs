using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TvRaspored.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "tip_korisnika",
            //    columns: table => new
            //    {
            //        tip_id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        naziv = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_tip_korisnika", x => x.tip_id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "zanr",
            //    columns: table => new
            //    {
            //        zanr_id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        naziv = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_zanr", x => x.zanr_id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "korisnik",
            //    columns: table => new
            //    {
            //        korisnik_id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        tip_id = table.Column<int>(nullable: false),
            //        korisnicko_ime = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        lozinka = table.Column<string>(maxLength: 256, nullable: true),
            //        ime = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        prezime = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        slika = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
            //        datum_prijave = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_korisnik", x => x.korisnik_id);
            //        table.ForeignKey(
            //            name: "fk_korisnik_tip_korisnika",
            //            column: x => x.tip_id,
            //            principalTable: "tip_korisnika",
            //            principalColumn: "tip_id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tvpostaja",
            //    columns: table => new
            //    {
            //        tvpostaja_id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        moderator_id = table.Column<int>(nullable: false),
            //        naziv = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tvpostaja", x => x.tvpostaja_id);
            //        table.ForeignKey(
            //            name: "fk_tvpostaja_korisnik1",
            //            column: x => x.moderator_id,
            //            principalTable: "korisnik",
            //            principalColumn: "korisnik_id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "emisija",
            //    columns: table => new
            //    {
            //        emisija_id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        tvpostaja_id = table.Column<int>(nullable: false),
            //        zanr_id = table.Column<int>(nullable: false),
            //        naziv = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        opis = table.Column<string>(unicode: false, nullable: false),
            //        datum_vrijeme_pocetka = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
            //        datum_vrijeme_zavrsetka = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_emisija", x => x.emisija_id);
            //        table.ForeignKey(
            //            name: "fk_emisija_tvpostaja1",
            //            column: x => x.tvpostaja_id,
            //            principalTable: "tvpostaja",
            //            principalColumn: "tvpostaja_id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "fk_emisija_zanr1",
            //            column: x => x.zanr_id,
            //            principalTable: "zanr",
            //            principalColumn: "zanr_id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "pretplata",
            //    columns: table => new
            //    {
            //        korisnik_id = table.Column<int>(nullable: false),
            //        emisija_id = table.Column<int>(nullable: false),
            //        status = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.ForeignKey(
            //            name: "fk_korisnik_has_emisija_emisija1",
            //            column: x => x.emisija_id,
            //            principalTable: "emisija",
            //            principalColumn: "emisija_id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "fk_korisnik_has_emisija_korisnik1",
            //            column: x => x.korisnik_id,
            //            principalTable: "korisnik",
            //            principalColumn: "korisnik_id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "fk_emisija_tvpostaja1_idx",
            //    table: "emisija",
            //    column: "tvpostaja_id");

            //migrationBuilder.CreateIndex(
            //    name: "fk_emisija_zanr1_idx",
            //    table: "emisija",
            //    column: "zanr_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_korisnik_tip_id",
            //    table: "korisnik",
            //    column: "tip_id");

            //migrationBuilder.CreateIndex(
            //    name: "fk_korisnik_has_emisija_emisija1_idx",
            //    table: "pretplata",
            //    column: "emisija_id");

            //migrationBuilder.CreateIndex(
            //    name: "fk_korisnik_has_emisija_korisnik1_idx",
            //    table: "pretplata",
            //    column: "korisnik_id");

            //migrationBuilder.CreateIndex(
            //    name: "fk_tvpostaja_korisnik1_idx",
            //    table: "tvpostaja",
            //    column: "moderator_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pretplata");

            migrationBuilder.DropTable(
                name: "emisija");

            migrationBuilder.DropTable(
                name: "tvpostaja");

            migrationBuilder.DropTable(
                name: "zanr");

            migrationBuilder.DropTable(
                name: "korisnik");

            migrationBuilder.DropTable(
                name: "tip_korisnika");
        }
    }
}
