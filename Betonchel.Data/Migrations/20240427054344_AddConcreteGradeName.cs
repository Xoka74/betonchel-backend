using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Betonchel.Data.Migrations
{
    public partial class AddConcreteGradeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Name",
                table: "ConcreteGrades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades",
                columns: new[] { "Mark", "Class" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ConcreteGrades");
        }
    }
}
