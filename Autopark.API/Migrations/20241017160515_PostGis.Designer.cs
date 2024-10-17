﻿// <auto-generated />
using System;
using Autopark.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Autopark.API.Migrations
{
    [DbContext(typeof(AutoparkDbContext))]
    [Migration("20241017160515_PostGis")]
    partial class PostGis
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("identity")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Autopark.API.Entities.Brand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("EngineDisplacementLiters")
                        .HasColumnType("numeric");

                    b.Property<int>("FuelTankCapacityLiters")
                        .HasColumnType("integer");

                    b.Property<int>("LiftWeightCapacityKg")
                        .HasColumnType("integer");

                    b.Property<string>("ManufacturerCompany")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SeatsCount")
                        .HasColumnType("integer");

                    b.Property<int>("VehicleType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Brands", "identity");
                });

            modelBuilder.Entity("Autopark.API.Entities.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CurrentVehicleId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EnterpriseId")
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrentVehicleId")
                        .IsUnique();

                    b.HasIndex("EnterpriseId");

                    b.ToTable("Drivers", "identity");
                });

            modelBuilder.Entity("Autopark.API.Entities.Enterprise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TimeZone")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Enterprises", "identity");
                });

            modelBuilder.Entity("Autopark.API.Entities.Manager", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", "identity");
                });

            modelBuilder.Entity("Autopark.API.Entities.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("BrandId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("EnterpriseId")
                        .HasColumnType("uuid");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ManufactureYear")
                        .HasColumnType("integer");

                    b.Property<int>("Mileage")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("EnterpriseId");

                    b.ToTable("Vehicles", "identity");
                });

            modelBuilder.Entity("DriverVehicle", b =>
                {
                    b.Property<Guid>("DriversId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VehiclesId")
                        .HasColumnType("uuid");

                    b.HasKey("DriversId", "VehiclesId");

                    b.HasIndex("VehiclesId");

                    b.ToTable("DriverVehicle", "identity");
                });

            modelBuilder.Entity("EnterpriseManager", b =>
                {
                    b.Property<Guid>("EnterprisesId")
                        .HasColumnType("uuid");

                    b.Property<string>("ManagersId")
                        .HasColumnType("text");

                    b.HasKey("EnterprisesId", "ManagersId");

                    b.HasIndex("ManagersId");

                    b.ToTable("EnterpriseManager", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", "identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", "identity");
                });

            modelBuilder.Entity("Autopark.API.Entities.Driver", b =>
                {
                    b.HasOne("Autopark.API.Entities.Vehicle", "CurrentVehicle")
                        .WithOne("CurrentDriver")
                        .HasForeignKey("Autopark.API.Entities.Driver", "CurrentVehicleId");

                    b.HasOne("Autopark.API.Entities.Enterprise", "Enterprise")
                        .WithMany("Drivers")
                        .HasForeignKey("EnterpriseId");

                    b.Navigation("CurrentVehicle");

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("Autopark.API.Entities.Vehicle", b =>
                {
                    b.HasOne("Autopark.API.Entities.Brand", "Brand")
                        .WithMany("Vehicles")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Autopark.API.Entities.Enterprise", "Enterprise")
                        .WithMany("Vehicles")
                        .HasForeignKey("EnterpriseId");

                    b.Navigation("Brand");

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("DriverVehicle", b =>
                {
                    b.HasOne("Autopark.API.Entities.Driver", null)
                        .WithMany()
                        .HasForeignKey("DriversId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Autopark.API.Entities.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehiclesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EnterpriseManager", b =>
                {
                    b.HasOne("Autopark.API.Entities.Enterprise", null)
                        .WithMany()
                        .HasForeignKey("EnterprisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Autopark.API.Entities.Manager", null)
                        .WithMany()
                        .HasForeignKey("ManagersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Autopark.API.Entities.Manager", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Autopark.API.Entities.Manager", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Autopark.API.Entities.Manager", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Autopark.API.Entities.Manager", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Autopark.API.Entities.Brand", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Autopark.API.Entities.Enterprise", b =>
                {
                    b.Navigation("Drivers");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Autopark.API.Entities.Vehicle", b =>
                {
                    b.Navigation("CurrentDriver");
                });
#pragma warning restore 612, 618
        }
    }
}
