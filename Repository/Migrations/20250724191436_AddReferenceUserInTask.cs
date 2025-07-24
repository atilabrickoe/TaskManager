using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceUserInTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "TaskItens");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TaskItens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TaskItens",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserId",
                table: "TaskItens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItens_Users_UserId",
                table: "TaskItens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
