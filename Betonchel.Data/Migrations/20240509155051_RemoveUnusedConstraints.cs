using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Betonchel.Data.Migrations
{
    public partial class RemoveUnusedConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DeliveryDate",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades",
                columns: new[] { "Mark", "Class" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteGrades_Mark_Class",
                table: "ConcreteGrades",
                columns: new[] { "Mark", "Class" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DeliveryDate",
                table: "Applications",
                sql: "\"DeliveryDate\" > now()");
        }
    }
}
