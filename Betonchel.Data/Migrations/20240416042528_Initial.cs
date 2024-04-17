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
                name: "ConcretePumps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaximumCapacity = table.Column<float>(type: "real", nullable: false),
                    PipeLength = table.Column<float>(type: "real", nullable: true),
                    PricePerHour = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcretePumps", x => x.Id);
                    table.CheckConstraint("CK_MaximumCapacity", "\"MaximumCapacity\" >= 0");
                    table.CheckConstraint("CK_PipeLength", "\"PipeLength\" >= 0");
                    table.CheckConstraint("CK_PricePerHour", "\"PricePerHour\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "FrostResistanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrostResistanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactData = table.Column<string>(type: "json", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterproofTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterproofTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mark = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    Class = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    WaterproofTypeId = table.Column<int>(type: "integer", nullable: false),
                    FrostResistanceTypeId = table.Column<int>(type: "integer", nullable: false),
                    PricePerCubicMeter = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteGrades", x => x.Id);
                    table.CheckConstraint("CK_PricePerCubicMeter", "\"PricePerCubicMeter\" >= 0");
                    table.ForeignKey(
                        name: "FK_ConcreteGrades_FrostResistanceTypes_FrostResistanceTypeId",
                        column: x => x.FrostResistanceTypeId,
                        principalTable: "FrostResistanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcreteGrades_WaterproofTypes_WaterproofTypeId",
                        column: x => x.WaterproofTypeId,
                        principalTable: "WaterproofTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerName = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ConcreteGradeId = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<double>(type: "numeric", nullable: false),
                    ConcretePumpId = table.Column<int>(type: "integer", nullable: false),
                    ContactData = table.Column<string>(type: "json", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "json", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ApplicationCreationDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "varchar", maxLength: 512, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.CheckConstraint("CK_DeliveryDate", "\"DeliveryDate\" > now()");
                    table.CheckConstraint("CK_TotalPrice", "\"TotalPrice\" >= 0");
                    table.CheckConstraint("CK_Volume", "\"Volume\" >= 0");
                    table.ForeignKey(
                        name: "FK_Applications_ConcreteGrades_ConcreteGradeId",
                        column: x => x.ConcreteGradeId,
                        principalTable: "ConcreteGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_ConcretePumps_ConcretePumpId",
                        column: x => x.ConcretePumpId,
                        principalTable: "ConcretePumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ConcreteGradeId",
                table: "Applications",
                column: "ConcreteGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ConcretePumpId",
                table: "Applications",
                column: "ConcretePumpId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId",
                table: "Applications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_FrostResistanceTypeId",
                table: "ConcreteGrades",
                column: "FrostResistanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_WaterproofTypeId",
                table: "ConcreteGrades",
                column: "WaterproofTypeId");
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
                name: "Users");

            migrationBuilder.DropTable(
                name: "FrostResistanceTypes");

            migrationBuilder.DropTable(
                name: "WaterproofTypes");
        }
    }
}
