using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v0019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_AspNetUsers_UserId",
                table: "Catalogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_AspNetUsers_UserId",
                table: "Catalogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_AspNetUsers_UserId",
                table: "Catalogs");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_AspNetUsers_UserId",
                table: "Catalogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
