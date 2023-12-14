using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ErrorFixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Finances_UserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "FinanceUserId",
                table: "Expenses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FinanceUserId",
                table: "Expenses",
                column: "FinanceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Finances_FinanceUserId",
                table: "Expenses",
                column: "FinanceUserId",
                principalTable: "Finances",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Finances_FinanceUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FinanceUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FinanceUserId",
                table: "Expenses");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Finances_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "Finances",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
