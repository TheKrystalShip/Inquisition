using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class NotificationsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_AuthorId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Users_AuthorId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "IsPermanent",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Songs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Songs_AuthorId",
                table: "Songs",
                newName: "IX_Songs_UserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Playlists",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_AuthorId",
                table: "Playlists",
                newName: "IX_Playlists_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Users_UserId",
                table: "Songs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Users_UserId",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Songs",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Songs_UserId",
                table: "Songs",
                newName: "IX_Songs_AuthorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Playlists",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                newName: "IX_Playlists_AuthorId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPermanent",
                table: "Notifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_AuthorId",
                table: "Playlists",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Users_AuthorId",
                table: "Songs",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
