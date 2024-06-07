using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Core_Assignment1.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBookToSetNullForDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
