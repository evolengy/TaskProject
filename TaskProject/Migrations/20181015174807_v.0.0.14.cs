using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v0014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
