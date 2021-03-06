using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreezerTape2.Migrations
{
    public partial class InitialMySQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrimalCut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimalCut", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Specie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShelfLife = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specie", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoragePlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoragePlace", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Carcass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShotDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ShotPlace = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShotBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LiveWeight = table.Column<double>(type: "double", nullable: true),
                    DressedWeight = table.Column<double>(type: "double", nullable: true),
                    PositionOfBulkhead = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<double>(type: "double", nullable: true),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carcass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carcass_Specie_SpecieId",
                        column: x => x.SpecieId,
                        principalTable: "Specie",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrimalCutSpecie",
                columns: table => new
                {
                    PrimalCutsId = table.Column<int>(type: "int", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimalCutSpecie", x => new { x.PrimalCutsId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_PrimalCutSpecie_PrimalCut_PrimalCutsId",
                        column: x => x.PrimalCutsId,
                        principalTable: "PrimalCut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimalCutSpecie_Specie_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Specie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Weight = table.Column<double>(type: "double", nullable: true),
                    PackingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CarcassId = table.Column<int>(type: "int", nullable: true),
                    PrimalCutId = table.Column<int>(type: "int", nullable: true),
                    StoragePlaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Carcass_CarcassId",
                        column: x => x.CarcassId,
                        principalTable: "Carcass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Package_PrimalCut_PrimalCutId",
                        column: x => x.PrimalCutId,
                        principalTable: "PrimalCut",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Package_StoragePlace_StoragePlaceId",
                        column: x => x.StoragePlaceId,
                        principalTable: "StoragePlace",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Carcass_SpecieId",
                table: "Carcass",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_CarcassId",
                table: "Package",
                column: "CarcassId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_PrimalCutId",
                table: "Package",
                column: "PrimalCutId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_StoragePlaceId",
                table: "Package",
                column: "StoragePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimalCutSpecie_SpeciesId",
                table: "PrimalCutSpecie",
                column: "SpeciesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "PrimalCutSpecie");

            migrationBuilder.DropTable(
                name: "Carcass");

            migrationBuilder.DropTable(
                name: "StoragePlace");

            migrationBuilder.DropTable(
                name: "PrimalCut");

            migrationBuilder.DropTable(
                name: "Specie");
        }
    }
}
