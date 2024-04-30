using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Betonchel.Data.Migrations
{
    public partial class SimplifyCocnreteGradeStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConcreteGrades_FrostResistanceTypes_FrostResistanceTypeId",
                table: "ConcreteGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcreteGrades_WaterproofTypes_WaterproofTypeId",
                table: "ConcreteGrades");

            migrationBuilder.DropTable(
                name: "FrostResistanceTypes");

            migrationBuilder.DropTable(
                name: "WaterproofTypes");

            migrationBuilder.DropIndex(
                name: "IX_ConcreteGrades_FrostResistanceTypeId",
                table: "ConcreteGrades");

            migrationBuilder.DropIndex(
                name: "IX_ConcreteGrades_WaterproofTypeId",
                table: "ConcreteGrades");

            migrationBuilder.DropColumn(
                name: "FrostResistanceTypeId",
                table: "ConcreteGrades");

            migrationBuilder.DropColumn(
                name: "WaterproofTypeId",
                table: "ConcreteGrades");

            migrationBuilder.AddColumn<string>(
                name: "FrostResistanceType",
                table: "ConcreteGrades",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterproofType",
                table: "ConcreteGrades",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrostResistanceType",
                table: "ConcreteGrades");

            migrationBuilder.DropColumn(
                name: "WaterproofType",
                table: "ConcreteGrades");

            migrationBuilder.AddColumn<int>(
                name: "FrostResistanceTypeId",
                table: "ConcreteGrades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterproofTypeId",
                table: "ConcreteGrades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FrostResistanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrostResistanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterproofTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterproofTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_FrostResistanceTypeId",
                table: "ConcreteGrades",
                column: "FrostResistanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_WaterproofTypeId",
                table: "ConcreteGrades",
                column: "WaterproofTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConcreteGrades_FrostResistanceTypes_FrostResistanceTypeId",
                table: "ConcreteGrades",
                column: "FrostResistanceTypeId",
                principalTable: "FrostResistanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcreteGrades_WaterproofTypes_WaterproofTypeId",
                table: "ConcreteGrades",
                column: "WaterproofTypeId",
                principalTable: "WaterproofTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
