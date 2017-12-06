using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class AuthorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Reminders",
                newName: "AuthorName");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Memes",
                newName: "AuthorName");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Jokes",
                newName: "AuthorName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Reminders",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Memes",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Jokes",
                newName: "AuthorId");
        }
    }
}
