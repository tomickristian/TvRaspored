using Microsoft.EntityFrameworkCore.Migrations;

namespace TvRaspored.Migrations
{
    public partial class AddPretplateID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "pretplata_id",
            //    table: "pretplata",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_pretplata",
            //    table: "pretplata",
            //    column: "pretplata_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_pretplata",
                table: "pretplata");

            migrationBuilder.DropColumn(
                name: "pretplata_id",
                table: "pretplata");
        }
    }
}
