﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TaskProject.Models;

namespace TaskProject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180426074624_v0.0.1")]
    partial class v001
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

            modelBuilder.Entity("TaskProject.ApplicationUser", b =>
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

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TaskProject.Atribute", b =>
                {
                    b.Property<int>("AtributeId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("AtributeId");

                    b.ToTable("Atributes");
                });

            modelBuilder.Entity("TaskProject.Complication", b =>
                {
                    b.Property<int>("ComplicationId");

                    b.Property<int>("Damage");

                    b.Property<int>("Exp");

                    b.Property<int>("Gold");

                    b.Property<string>("Name");

                    b.HasKey("ComplicationId");

                    b.ToTable("Complications");
                });

            modelBuilder.Entity("TaskProject.Goal", b =>
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

            modelBuilder.Entity("TaskProject.Habit", b =>
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

            modelBuilder.Entity("TaskProject.Message", b =>
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

            modelBuilder.Entity("TaskProject.Repeat", b =>
                {
                    b.Property<int>("RepeatId");

                    b.Property<string>("Name");

                    b.HasKey("RepeatId");

                    b.ToTable("Repeats");
                });

            modelBuilder.Entity("TaskProject.UserAtribute", b =>
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
                    b.HasOne("TaskProject.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TaskProject.ApplicationUser")
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

                    b.HasOne("TaskProject.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TaskProject.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskProject.Goal", b =>
                {
                    b.HasOne("TaskProject.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.Complication", "Complication")
                        .WithMany()
                        .HasForeignKey("ComplicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.Repeat", "Repeat")
                        .WithMany()
                        .HasForeignKey("RepeatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.ApplicationUser", "User")
                        .WithMany("Goals")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskProject.Habit", b =>
                {
                    b.HasOne("TaskProject.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.Complication", "Complication")
                        .WithMany()
                        .HasForeignKey("ComplicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.ApplicationUser", "User")
                        .WithMany("Habits")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TaskProject.UserAtribute", b =>
                {
                    b.HasOne("TaskProject.Atribute", "Atribute")
                        .WithMany()
                        .HasForeignKey("AtributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskProject.ApplicationUser", "User")
                        .WithMany("Atributes")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}