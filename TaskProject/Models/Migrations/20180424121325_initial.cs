using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskProject.Models.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CurrentExp",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "CurrentGold",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentHealth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentLevel",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Growth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IMT",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDead",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSetDescr",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "MaxExp",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "MaxHealth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Atributes",
                columns: table => new
                {
                    AtributeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributes", x => x.AtributeId);
                });

            migrationBuilder.CreateTable(
                name: "Attainments",
                columns: table => new
                {
                    AttainmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LinkImage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attainments", x => x.AttainmentId);
                    table.ForeignKey(
                        name: "FK_Attainments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Complications",
                columns: table => new
                {
                    ComplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Damage = table.Column<int>(nullable: false),
                    Exp = table.Column<int>(nullable: false),
                    Gold = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complications", x => x.ComplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    DateCreate = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Theme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repeats",
                columns: table => new
                {
                    RepeatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repeats", x => x.RepeatId);
                });

            migrationBuilder.CreateTable(
                name: "UserAtributes",
                columns: table => new
                {
                    UserAtributeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AtributeId = table.Column<int>(nullable: false),
                    CurrentExp = table.Column<int>(nullable: false),
                    MaxExp = table.Column<int>(nullable: false),
                    MaxValue = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAtributes", x => x.UserAtributeId);
                    table.ForeignKey(
                        name: "FK_UserAtributes_Atributes_AtributeId",
                        column: x => x.AtributeId,
                        principalTable: "Atributes",
                        principalColumn: "AtributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAtributes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "GuildsReputations",
                columns: table => new
                {
                    GuildReputationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentValue = table.Column<int>(nullable: false),
                    GuildId = table.Column<int>(nullable: false),
                    MaxValue = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildsReputations", x => x.GuildReputationId);
                    table.ForeignKey(
                        name: "FK_GuildsReputations_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildsReputations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    MissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    GuildId = table.Column<int>(nullable: false),
                    LevelUnlock = table.Column<int>(nullable: false),
                    LevelUp = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RepUp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.MissionId);
                    table.ForeignKey(
                        name: "FK_Missions_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    CharacterTaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AtributeId = table.Column<int>(nullable: false),
                    ComplicationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    RepeatId = table.Column<int>(nullable: false),
                    TaskEnd = table.Column<DateTime>(nullable: true),
                    TaskStart = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.CharacterTaskId);
                    table.ForeignKey(
                        name: "FK_Goals_Atributes_AtributeId",
                        column: x => x.AtributeId,
                        principalTable: "Atributes",
                        principalColumn: "AtributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Goals_Complications_ComplicationId",
                        column: x => x.ComplicationId,
                        principalTable: "Complications",
                        principalColumn: "ComplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Goals_Repeats_RepeatId",
                        column: x => x.RepeatId,
                        principalTable: "Repeats",
                        principalColumn: "RepeatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Goals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MissionsConditions",
                columns: table => new
                {
                    MissionConditionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false),
                    MissionId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionsConditions", x => x.MissionConditionId);
                    table.ForeignKey(
                        name: "FK_MissionsConditions_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "MissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionsConditions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClassId",
                table: "AspNetUsers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attainments_ApplicationUserId",
                table: "Attainments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_AtributeId",
                table: "Goals",
                column: "AtributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ComplicationId",
                table: "Goals",
                column: "ComplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_RepeatId",
                table: "Goals",
                column: "RepeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildsReputations_GuildId",
                table: "GuildsReputations",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildsReputations_UserId",
                table: "GuildsReputations",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Missions_GuildId",
                table: "Missions",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionsConditions_MissionId",
                table: "MissionsConditions",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionsConditions_UserId",
                table: "MissionsConditions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAtributes_AtributeId",
                table: "UserAtributes",
                column: "AtributeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAtributes_UserId",
                table: "UserAtributes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Classes_ClassId",
                table: "AspNetUsers",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Classes_ClassId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attainments");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "GuildsReputations");

            migrationBuilder.DropTable(
                name: "Habits");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MissionsConditions");

            migrationBuilder.DropTable(
                name: "UserAtributes");

            migrationBuilder.DropTable(
                name: "Repeats");

            migrationBuilder.DropTable(
                name: "Complications");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "Atributes");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClassId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentExp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentGold",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentHealth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Growth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IMT",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDead",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsSetDescr",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxExp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
