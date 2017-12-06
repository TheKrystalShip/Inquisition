using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class Addedmemberstablewithvirtuallists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Reminders",
                newName: "MemberIdId");

            migrationBuilder.AlterColumn<string>(
                name: "MemberIdId",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberIdId",
                table: "Memes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberIdId",
                table: "Jokes",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Memes_MemberIdId",
                table: "Memes",
                column: "MemberIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Jokes_MemberIdId",
                table: "Jokes",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Memes_MemberIdId",
                table: "Memes");

            migrationBuilder.DropIndex(
                name: "IX_Jokes_MemberIdId",
                table: "Jokes");

            migrationBuilder.DropColumn(
                name: "MemberIdId",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "MemberIdId",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "MemberIdId",
                table: "Reminders",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Reminders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
