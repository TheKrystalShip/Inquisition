using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class NavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "TargetNickname",
                table: "Notifications",
                newName: "TargetUserId");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUserId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TargetUserId",
                table: "Notifications",
                column: "TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_TargetUserId",
                table: "Notifications",
                column: "TargetUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_TargetUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TargetUserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "TargetUserId",
                table: "Notifications",
                newName: "TargetNickname");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Reminders",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetNickname",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetName",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Memes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Jokes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
