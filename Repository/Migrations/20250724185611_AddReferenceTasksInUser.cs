using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceTasksInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TaskItens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItens_UserId",
                table: "TaskItens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens");

            migrationBuilder.DropIndex(
                name: "IX_TaskItens_UserId",
                table: "TaskItens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskItens");
        }
    }
}
