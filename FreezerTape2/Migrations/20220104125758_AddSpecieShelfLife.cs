using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreezerTape2.Migrations
{
    public partial class AddSpecieShelfLife : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShelfLife",
                table: "Specie",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelfLife",
                table: "Specie");
        }
    }
}
