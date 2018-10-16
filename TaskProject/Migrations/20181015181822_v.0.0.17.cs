using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v0017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Healths_HealthId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Healths_AspNetUsers_UserId",
                table: "Healths");

            migrationBuilder.DropIndex(
                name: "IX_Healths_UserId",
                table: "Healths");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HealthId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CatalogId",
                table: "Goals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Healths_UserId",
                table: "Healths",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Healths_AspNetUsers_UserId",
                table: "Healths",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Healths_AspNetUsers_UserId",
                table: "Healths");

            migrationBuilder.DropIndex(
                name: "IX_Healths_UserId",
                table: "Healths");

            migrationBuilder.AlterColumn<int>(
                name: "CatalogId",
                table: "Goals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Healths_UserId",
                table: "Healths",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HealthId",
                table: "AspNetUsers",
                column: "HealthId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Healths_HealthId",
                table: "AspNetUsers",
                column: "HealthId",
                principalTable: "Healths",
                principalColumn: "HealthId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Healths_AspNetUsers_UserId",
                table: "Healths",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
