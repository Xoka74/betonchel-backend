﻿// <auto-generated />
using System;
using Betonchel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Betonchel.Data.Migrations
{
    [DbContext(typeof(BetonchelContext))]
    [Migration("20240508142015_RemoveUnusedConstraints")]
    partial class RemoveUnusedConstraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Betonchel.Domain.DBModels.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ApplicationCreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("ConcreteGradeId")
                        .HasColumnType("integer");

                    b.Property<int?>("ConcretePumpId")
                        .HasColumnType("integer");

                    b.Property<string>("ContactData")
                        .IsRequired()
                        .HasMaxLength(124)
                        .HasColumnType("character varying(124)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DeliveryAddress")
                        .HasMaxLength(124)
                        .HasColumnType("character varying(124)");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<double>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ConcreteGradeId");

                    b.HasIndex("ConcretePumpId");

                    b.HasIndex("UserId");

                    b.ToTable("Applications");

                    b.HasCheckConstraint("CK_TotalPrice", "\"TotalPrice\" >= 0");

                    b.HasCheckConstraint("CK_Volume", "\"Volume\" >= 0");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.ConcreteGrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("FrostResistanceType")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Mark")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<int>("Name")
                        .HasColumnType("integer");

                    b.Property<double>("PricePerCubicMeter")
                        .HasColumnType("double precision");

                    b.Property<string>("WaterproofType")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.HasIndex("Mark", "Class")
                        .IsUnique();

                    b.ToTable("ConcreteGrades");

                    b.HasCheckConstraint("CK_PricePerCubicMeter", "\"PricePerCubicMeter\" >= 0");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.ConcretePump", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("MaximumCapacity")
                        .HasColumnType("real");

                    b.Property<float?>("PipeLength")
                        .HasColumnType("real");

                    b.Property<double>("PricePerHour")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("ConcretePumps");

                    b.HasCheckConstraint("CK_MaximumCapacity", "\"MaximumCapacity\" >= 0");

                    b.HasCheckConstraint("CK_PipeLength", "\"PipeLength\" >= 0");

                    b.HasCheckConstraint("CK_PricePerHour", "\"PricePerHour\" >= 0");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasCheckConstraint("CK_Email", "\"Email\" LIKE '%@%'");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.Application", b =>
                {
                    b.HasOne("Betonchel.Domain.DBModels.ConcreteGrade", "ConcreteGrade")
                        .WithMany("Applications")
                        .HasForeignKey("ConcreteGradeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Betonchel.Domain.DBModels.ConcretePump", "ConcretePump")
                        .WithMany("Applications")
                        .HasForeignKey("ConcretePumpId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Betonchel.Domain.DBModels.User", "User")
                        .WithMany("Application")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ConcreteGrade");

                    b.Navigation("ConcretePump");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.ConcreteGrade", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.ConcretePump", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("Betonchel.Domain.DBModels.User", b =>
                {
                    b.Navigation("Application");
                });
#pragma warning restore 612, 618
        }
    }
}
