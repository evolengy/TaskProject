using Microsoft.EntityFrameworkCore.Metadata;
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
                name: "FK_Goals_Atributes_AtributeId",
                table: "Goals");

            migrationBuilder.DropTable(
                name: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_Goals_AtributeId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "AtributeId",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "AspNetUsers",
                newName: "AligmentId");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "IMT",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "Growth",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Aligments",
                columns: table => new
                {
                    AligmentId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aligments", x => x.AligmentId);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AtributeId = table.Column<int>(nullable: true),
                    CurrentExp = table.Column<int>(nullable: false),
                    GoalCharacterTaskId = table.Column<int>(nullable: true),
                    Lvl = table.Column<int>(nullable: false),
                    MaxExp = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RatingId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_Atributes_AtributeId",
                        column: x => x.AtributeId,
                        principalTable: "Atributes",
                        principalColumn: "AtributeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skills_Goals_GoalCharacterTaskId",
                        column: x => x.GoalCharacterTaskId,
                        principalTable: "Goals",
                        principalColumn: "CharacterTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skills_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AligmentId",
                table: "AspNetUsers",
                column: "AligmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AtributeId",
                table: "Skills",
                column: "AtributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_GoalCharacterTaskId",
                table: "Skills",
                column: "GoalCharacterTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_RatingId",
                table: "Skills",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UserId",
                table: "Skills",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Aligments_AligmentId",
                table: "AspNetUsers",
                column: "AligmentId",
                principalTable: "Aligments",
                principalColumn: "AligmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Aligments_AligmentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Aligments");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AligmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AligmentId",
                table: "AspNetUsers",
                newName: "ClassId");

            migrationBuilder.AddColumn<int>(
                name: "AtributeId",
                table: "Goals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "IMT",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "Growth",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.CreateTable(
                name: "Habits",
                columns: table => new
                {
                    HabitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AtributeId = table.Column<int>(nullable: false),
                    ComplicationId = table.Column<int>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    HabitEnd = table.Column<DateTime>(type: "Date", nullable: false),
                    HabitStart = table.Column<DateTime>(type: "Date", nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsUseful = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    WarningCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habits", x => x.HabitId);
                    table.ForeignKey(
                        name: "FK_Habits_Atributes_AtributeId",
                        column: x => x.AtributeId,
                        principalTable: "Atributes",
                        principalColumn: "AtributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habits_Complications_ComplicationId",
                        column: x => x.ComplicationId,
                        principalTable: "Complications",
                        principalColumn: "ComplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_AtributeId",
                table: "Goals",
                column: "AtributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_AtributeId",
                table: "Habits",
                column: "AtributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_ComplicationId",
                table: "Habits",
                column: "ComplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_UserId",
                table: "Habits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Atributes_AtributeId",
                table: "Goals",
                column: "AtributeId",
                principalTable: "Atributes",
                principalColumn: "AtributeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
