using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Betonchel.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    ManagerName = table.Column<string>(type: "text", nullable: false),
                    ConcreteId = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    ConcretePumpId = table.Column<int>(type: "integer", nullable: false),
                    ContactDate = table.Column<string>(type: "json", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "json", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ApplicationCreationDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteGrades",
                columns: table => new
                {
                    ConcreteGradeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Make = table.Column<string>(type: "text", nullable: false),
                    Class = table.Column<string>(type: "text", nullable: false),
                    WaterproofId = table.Column<int>(type: "integer", nullable: false),
                    FrostResistanceId = table.Column<int>(type: "integer", nullable: false),
                    PricePerCubicMeter = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteGrades", x => x.ConcreteGradeId);
                });

            migrationBuilder.CreateTable(
                name: "ConcretePumps",
                columns: table => new
                {
                    ConcretePumpId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaximumCapacity = table.Column<float>(type: "real", nullable: false),
                    PipeLength = table.Column<float>(type: "real", nullable: true),
                    PricePerHour = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcretePumps", x => x.ConcretePumpId);
                });

            migrationBuilder.CreateTable(
                name: "FrostResistanceTypes",
                columns: table => new
                {
                    FrostResistanceTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrostResistanceTypes", x => x.FrostResistanceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "WaterproofTypes",
                columns: table => new
                {
                    WaterproofTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterproofTypes", x => x.WaterproofTypeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ConcreteGrades");

            migrationBuilder.DropTable(
                name: "ConcretePumps");

            migrationBuilder.DropTable(
                name: "FrostResistanceTypes");

            migrationBuilder.DropTable(
                name: "WaterproofTypes");
        }
    }
}
