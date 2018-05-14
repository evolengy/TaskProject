using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "UserAtributes");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "UserAtributes",
                newName: "Lvl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lvl",
                table: "UserAtributes",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "MaxValue",
                table: "UserAtributes",
                nullable: false,
                defaultValue: 0);
        }
    }
}
