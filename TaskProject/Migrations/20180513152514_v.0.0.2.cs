using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Atributes_AtributeId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "AtributeId",
                table: "Skills",
                newName: "AtributeUserAtributeId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_AtributeId",
                table: "Skills",
                newName: "IX_Skills_AtributeUserAtributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_UserAtributes_AtributeUserAtributeId",
                table: "Skills",
                column: "AtributeUserAtributeId",
                principalTable: "UserAtributes",
                principalColumn: "UserAtributeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_UserAtributes_AtributeUserAtributeId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "AtributeUserAtributeId",
                table: "Skills",
                newName: "AtributeId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_AtributeUserAtributeId",
                table: "Skills",
                newName: "IX_Skills_AtributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Atributes_AtributeId",
                table: "Skills",
                column: "AtributeId",
                principalTable: "Atributes",
                principalColumn: "AtributeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
