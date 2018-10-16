using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v0013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributes_AspNetUsers_UserId",
                table: "Atributes");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributes_AspNetUsers_UserId",
                table: "Atributes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributes_AspNetUsers_UserId",
                table: "Atributes");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributes_AspNetUsers_UserId",
                table: "Atributes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
