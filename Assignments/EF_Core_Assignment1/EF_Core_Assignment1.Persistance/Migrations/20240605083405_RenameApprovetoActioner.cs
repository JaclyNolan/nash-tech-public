using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Core_Assignment1.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RenameApprovetoActioner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequests_AspNetUsers_ApproverId",
                table: "BookBorrowingRequests");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "BookBorrowingRequests",
                newName: "ActionerId");

            migrationBuilder.RenameIndex(
                name: "IX_BookBorrowingRequests_ApproverId",
                table: "BookBorrowingRequests",
                newName: "IX_BookBorrowingRequests_ActionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequests_AspNetUsers_ActionerId",
                table: "BookBorrowingRequests",
                column: "ActionerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequests_AspNetUsers_ActionerId",
                table: "BookBorrowingRequests");

            migrationBuilder.RenameColumn(
                name: "ActionerId",
                table: "BookBorrowingRequests",
                newName: "ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_BookBorrowingRequests_ActionerId",
                table: "BookBorrowingRequests",
                newName: "IX_BookBorrowingRequests_ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequests_AspNetUsers_ApproverId",
                table: "BookBorrowingRequests",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
