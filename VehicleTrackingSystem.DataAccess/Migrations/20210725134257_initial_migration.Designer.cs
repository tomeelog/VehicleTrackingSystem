﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehicleTrackingSystem.DataAccess.DbCtx;

namespace VehicleTrackingSystem.DataAccess.Migrations
{
    [DbContext(typeof(VehicleTrackingCommandsContext))]
    [Migration("20210725134257_initial_migration")]
    partial class initial_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VehicleTrackingSystem.Entities.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("ImeiNumber")
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(16);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("DeviceId");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Altitude")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("HorizontalAccuracy")
                        .HasColumnType("float");

                    b.Property<double>("Latitude")
                        .HasColumnType("float")
                        .HasMaxLength(500);

                    b.Property<double>("Longitude")
                        .HasColumnType("float")
                        .HasMaxLength(500);

                    b.Property<double>("Speed")
                        .HasColumnType("float");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.Property<double>("VerticalAccuracy")
                        .HasColumnType("float");

                    b.HasKey("LocationId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.UserType", b =>
                {
                    b.Property<int>("UserTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("UserTypeId");

                    b.ToTable("UserType");

                    b.HasData(
                        new
                        {
                            UserTypeId = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            UserTypeId = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BodyType")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("Maker")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.HasKey("VehicleId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("UserId");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.Location", b =>
                {
                    b.HasOne("VehicleTrackingSystem.Entities.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.User", b =>
                {
                    b.HasOne("VehicleTrackingSystem.Entities.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VehicleTrackingSystem.Entities.Vehicle", b =>
                {
                    b.HasOne("VehicleTrackingSystem.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VehicleTrackingSystem.Entities.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
