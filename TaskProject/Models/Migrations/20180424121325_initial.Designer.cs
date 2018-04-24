﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TaskProject.Models;

namespace TaskProject.Models.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180424121325_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TaskLibrary.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("Age");

                    b.Property<int?>("ClassId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long>("CurrentExp");

                    b.Property<int>("CurrentGold");

                    b.Property<int>("CurrentHealth");

                    b.Property<int>("CurrentLevel");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("Growth");

                    b.Property<int>("IMT");

                    b.Property<bool>("IsDead");

                    b.Property<bool>("IsSetDescr");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<long>("MaxExp");

                    b.Property<int>("MaxHealth");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Sex");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TaskLibrary.Atribute", b =>
                {
                    b.Property<int>("AtributeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("AtributeId");

                    b.ToTable("Atributes");
                });

            modelBuilder.Entity("TaskLibrary.Attainment", b =>
                {
                    b.Property<int>("AttainmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Description");

                    b.Property<string>("LinkImage");

                    b.Property<string>("Name");

                    b.HasKey("AttainmentId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Attainments");
                });

            modelBuilder.Entity("TaskLibrary.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ClassId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("TaskLibrary.Complication", b =>
                {
                    b.Property<int>("ComplicationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Damage");

                    b.Property<int>("Exp");

                    b.Property<int>("Gold");

                    b.Property<string>("Name");

                    b.HasKey("ComplicationId");

                    b.ToTable("Complications");
                });

            modelBuilder.Entity("TaskLibrary.Goal", b =>
                {
                    b.Property<int>("CharacterTaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AtributeId");

                    b.Property<int>("ComplicationId");

                    b.Property<string>("Description");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("RepeatId");

                    b.Property<DateTime?>("TaskEnd");

                    b.Property<DateTime>("TaskStart");

                    b.Property<string>("UserId");

                    b.HasKey("CharacterTaskId");

                    b.HasIndex("AtributeId");

                    b.HasIndex("ComplicationId");

                    b.HasIndex("RepeatId");

                    b.HasIndex("UserId");

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("TaskLibrary.Guild", b =>
                {
                    b.Property<int>("GuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("GuildId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("TaskLibrary.GuildsReputation", b =>
                {
                    b.Property<int>("GuildReputationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentValue");

                    b.Property<int>("GuildId");

                    b.Property<int>("MaxValue");

                    b.Property<string>("UserId");

                    b.HasKey("GuildReputationId");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("GuildsReputations");
                });

            modelBuilder.Entity("TaskLibrary.Habit", b =>
                {
                    b.Property<int>("HabitId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AtributeId");

                    b.Property<int>("ComplicationId");

                    b.Property<int>("DayCount");

                    b.Property<DateTime>("HabitEnd")
                        .HasColumnType("Date");

                    b.Property<DateTime>("HabitStart")
                        .HasColumnType("Date");

                    b.Property<bool>("IsAccepted");

                    b.Property<bool>("IsUseful");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("UserId");

                    b.Property<int>("WarningCount");

                    b.HasKey("HabitId");

                    b.HasIndex("AtributeId");

                    b.HasIndex("ComplicationId");

                    b.HasIndex("UserId");

                    b.ToTable("Habits");
                });

            modelBuilder.Entity("TaskLibrary.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<string>("DateCreate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Theme");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("TaskLibrary.Mission", b =>
                {
                    b.Property<int>("MissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GuildId");

                    b.Property<int>("LevelUnlock");

                    b.Property<int>("LevelUp");

                    b.Property<string>("Name");

                    b.Property<int>("RepUp");

                    b.HasKey("MissionId");

                    b.HasIndex("GuildId");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("TaskLibrary.MissionsCondition", b =>
                {
                    b.Property<int>("MissionConditionId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAccepted");

                    b.Property<bool>("IsComplete");

                    b.Property<int>("MissionId");

                    b.Property<string>("UserId");

                    b.HasKey("MissionConditionId");

                    b.HasIndex("MissionId");

                    b.HasIndex("UserId");

                    b.ToTable("MissionsConditions");
                });

            modelBuilder.Entity("TaskLibrary.Repeat", b =>
                {
                    b.Property<int>("RepeatId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("RepeatId");

                    b.ToTable("Repeats");
                });

            modelBuilder.Entity("TaskLibrary.UserAtribute", b =>
                {
                    b.Property<int>("UserAtributeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AtributeId");

                    b.Property<int>("CurrentExp");

                    b.Property<int>("MaxExp");

                    b.Property<int>("MaxValue");

                    b.Property<string>("UserId");

                    b.Property<int>("Value");

                    b.HasKey("UserAtributeId");

                    b.HasIndex("AtributeId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAtributes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TaskLibrary.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TaskLibrary.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TaskLibrary.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskLibrary.ApplicationUser", b =>
                {
                    b.HasOne("TaskLibrary.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");
                });

            modelBuilder.Entity("TaskLibrary.Attainment", b =>
                {
                    b.HasOne("TaskLibrary.ApplicationUser")
                        .WithMany("Attainments")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("TaskLibrary.Goal", b =>
                {
                    b.HasOne("TaskLibrary.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.Complication", "Complication")
                        .WithMany()
                        .HasForeignKey("ComplicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.Repeat", "Repeat")
                        .WithMany()
                        .HasForeignKey("RepeatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser", "User")
                        .WithMany("Goals")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskLibrary.GuildsReputation", b =>
                {
                    b.HasOne("TaskLibrary.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser", "User")
                        .WithMany("GuildsRep")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskLibrary.Habit", b =>
                {
                    b.HasOne("TaskLibrary.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.Complication", "Complication")
                        .WithMany()
                        .HasForeignKey("ComplicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser", "User")
                        .WithMany("Habits")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskLibrary.Mission", b =>
                {
                    b.HasOne("TaskLibrary.Guild", "Guild")
                        .WithMany("Missions")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskLibrary.MissionsCondition", b =>
                {
                    b.HasOne("TaskLibrary.Mission", "Mission")
                        .WithMany()
                        .HasForeignKey("MissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser", "User")
                        .WithMany("Missions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskLibrary.UserAtribute", b =>
                {
                    b.HasOne("TaskLibrary.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskLibrary.ApplicationUser", "User")
                        .WithMany("Atributes")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
