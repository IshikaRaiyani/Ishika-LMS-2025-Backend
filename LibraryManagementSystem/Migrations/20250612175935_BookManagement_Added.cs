using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class BookManagement_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookManagement",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BorrowRequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BorrowDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Fine = table.Column<int>(type: "int", nullable: false),
                    BorrowStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookManagement", x => x.TransactionId);
                    table.CheckConstraint("CK_BookManagement_BorrowStatus", "BorrowStatus IN ('Pending', 'Approved','Rejected')");
                    table.CheckConstraint("CK_BookManagement_RequestType", "RequestType IN ('Borrow', 'Return')");
                    table.CheckConstraint("CK_BookManagement_ReturnStatus", "ReturnStatus IN ('Pending', 'Approved','Rejected','None')");
                    table.ForeignKey(
                        name: "FK_BookManagement_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookManagement_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookManagement_BookId",
                table: "BookManagement",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookManagement_UserId",
                table: "BookManagement",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookManagement");
        }
    }
}
