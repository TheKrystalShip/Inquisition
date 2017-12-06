using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class UTCDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Reminders",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Memes",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Jokes",
                newName: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Reminders",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Memes",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Jokes",
                newName: "Author");
        }
    }
}
