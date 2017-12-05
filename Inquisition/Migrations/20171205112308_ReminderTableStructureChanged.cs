using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inquisition.Migrations
{
    public partial class ReminderTableStructureChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Reminders",
                newName: "Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Reminders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Reminders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Reminders");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Reminders",
                newName: "Time");
        }
    }
}
