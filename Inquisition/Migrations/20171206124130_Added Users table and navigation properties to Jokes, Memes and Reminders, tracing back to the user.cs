using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class AddedUserstableandnavigationpropertiestoJokesMemesandReminderstracingbacktotheuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jokes_Members_MemberIdId",
                table: "Jokes");

            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Members_MemberIdId",
                table: "Memes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Members_MemberIdId",
                table: "Reminders");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_MemberIdId",
                table: "Reminders");

            migrationBuilder.RenameColumn(
                name: "MemberIdId",
                table: "Reminders",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "MemberIdId",
                table: "Memes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Memes_MemberIdId",
                table: "Memes",
                newName: "IX_Memes_UserId");

            migrationBuilder.RenameColumn(
                name: "MemberIdId",
                table: "Jokes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Jokes_MemberIdId",
                table: "Jokes",
                newName: "IX_Jokes_UserId");

            migrationBuilder.RenameColumn(
                name: "ExeDir",
                table: "Games",
                newName: "LaunchArgs");

            migrationBuilder.RenameColumn(
                name: "Args",
                table: "Games",
                newName: "Exe");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DueDate",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateDate",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Memes",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Jokes",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: true),
                    JoinedAt = table.Column<DateTimeOffset>(nullable: true),
                    LastSeenOnline = table.Column<DateTimeOffset>(nullable: false),
                    Nickname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_UserId",
                table: "Reminders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jokes_Users_UserId",
                table: "Jokes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Users_UserId",
                table: "Memes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jokes_Users_UserId",
                table: "Jokes");

            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Users_UserId",
                table: "Memes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_UserId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Reminders",
                newName: "MemberIdId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Memes",
                newName: "MemberIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Memes_UserId",
                table: "Memes",
                newName: "IX_Memes_MemberIdId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Jokes",
                newName: "MemberIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Jokes_UserId",
                table: "Jokes",
                newName: "IX_Jokes_MemberIdId");

            migrationBuilder.RenameColumn(
                name: "LaunchArgs",
                table: "Games",
                newName: "ExeDir");

            migrationBuilder.RenameColumn(
                name: "Exe",
                table: "Games",
                newName: "Args");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<string>(
                name: "MemberIdId",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LastSeenOnline = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_MemberIdId",
                table: "Reminders",
                column: "MemberIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jokes_Members_MemberIdId",
                table: "Jokes",
                column: "MemberIdId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Members_MemberIdId",
                table: "Memes",
                column: "MemberIdId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Members_MemberIdId",
                table: "Reminders",
                column: "MemberIdId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
