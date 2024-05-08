using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Betonchel.Data.Migrations
{
    public partial class RemoveUnusedConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DeliveryDate",
                table: "Applications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_DeliveryDate",
                table: "Applications",
                sql: "\"DeliveryDate\" > now()");
        }
    }
}
