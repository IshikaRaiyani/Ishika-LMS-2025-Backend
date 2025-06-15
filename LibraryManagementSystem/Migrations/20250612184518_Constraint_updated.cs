using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Constraint_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BookManagement_RequestType",
                table: "BookManagement");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BookManagement_RequestType",
                table: "BookManagement",
                sql: "RequestType IN ('BorrowRequested','Borrowed','ReturnRequested', 'Returned')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BookManagement_RequestType",
                table: "BookManagement");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BookManagement_RequestType",
                table: "BookManagement",
                sql: "RequestType IN ('Borrow', 'Return')");
        }
    }
}
