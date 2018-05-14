using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Migrations
{
    public partial class v003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Goals_GoalCharacterTaskId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_GoalCharacterTaskId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "GoalCharacterTaskId",
                table: "Skills");

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Goals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_SkillId",
                table: "Goals",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Skills_SkillId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_SkillId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Goals");

            migrationBuilder.AddColumn<int>(
                name: "GoalCharacterTaskId",
                table: "Skills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_GoalCharacterTaskId",
                table: "Skills",
                column: "GoalCharacterTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Goals_GoalCharacterTaskId",
                table: "Skills",
                column: "GoalCharacterTaskId",
                principalTable: "Goals",
                principalColumn: "CharacterTaskId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
