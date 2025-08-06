using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_cascade_fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Complaints_ComplaintId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Departments_DepartmentId",
                table: "Complaints");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Complaints_ComplaintId",
                table: "Assignments",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Departments_DepartmentId",
                table: "Complaints",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Complaints_ComplaintId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Departments_DepartmentId",
                table: "Complaints");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Complaints_ComplaintId",
                table: "Assignments",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintHistories_Complaints_ComplaintId",
                table: "ComplaintHistories",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Departments_DepartmentId",
                table: "Complaints",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
