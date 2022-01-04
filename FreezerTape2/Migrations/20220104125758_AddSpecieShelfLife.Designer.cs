﻿// <auto-generated />
using System;
using FreezerTape2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FreezerTape2.Migrations
{
    [DbContext(typeof(FreezerContext))]
    [Migration("20220104125758_AddSpecieShelfLife")]
    partial class AddSpecieShelfLife
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FreezerTape2.Models.Carcass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double?>("Age")
                        .HasColumnType("float");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("DressedWeight")
                        .HasColumnType("float");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("LiveWeight")
                        .HasColumnType("float");

                    b.Property<string>("PositionOfBulkhead")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShotBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ShotDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShotPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpecieId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpecieId");

                    b.ToTable("Carcass");
                });

            modelBuilder.Entity("FreezerTape2.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CarcassId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PackingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PrimalCutId")
                        .HasColumnType("int");

                    b.Property<int?>("StoragePlaceId")
                        .HasColumnType("int");

                    b.Property<double?>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CarcassId");

                    b.HasIndex("PrimalCutId");

                    b.HasIndex("StoragePlaceId");

                    b.ToTable("Package");
                });

            modelBuilder.Entity("FreezerTape2.Models.PrimalCut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PrimalCut");
                });

            modelBuilder.Entity("FreezerTape2.Models.Specie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ShelfLife")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Specie");
                });

            modelBuilder.Entity("FreezerTape2.Models.StoragePlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StoragePlace");
                });

            modelBuilder.Entity("PrimalCutSpecie", b =>
                {
                    b.Property<int>("PrimalCutsId")
                        .HasColumnType("int");

                    b.Property<int>("SpeciesId")
                        .HasColumnType("int");

                    b.HasKey("PrimalCutsId", "SpeciesId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("PrimalCutSpecie");
                });

            modelBuilder.Entity("FreezerTape2.Models.Carcass", b =>
                {
                    b.HasOne("FreezerTape2.Models.Specie", "Specie")
                        .WithMany("Carcasses")
                        .HasForeignKey("SpecieId");

                    b.Navigation("Specie");
                });

            modelBuilder.Entity("FreezerTape2.Models.Package", b =>
                {
                    b.HasOne("FreezerTape2.Models.Carcass", "Carcass")
                        .WithMany("Packages")
                        .HasForeignKey("CarcassId");

                    b.HasOne("FreezerTape2.Models.PrimalCut", "PrimalCut")
                        .WithMany("Packages")
                        .HasForeignKey("PrimalCutId");

                    b.HasOne("FreezerTape2.Models.StoragePlace", "StoragePlace")
                        .WithMany("Packages")
                        .HasForeignKey("StoragePlaceId");

                    b.Navigation("Carcass");

                    b.Navigation("PrimalCut");

                    b.Navigation("StoragePlace");
                });

            modelBuilder.Entity("PrimalCutSpecie", b =>
                {
                    b.HasOne("FreezerTape2.Models.PrimalCut", null)
                        .WithMany()
                        .HasForeignKey("PrimalCutsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FreezerTape2.Models.Specie", null)
                        .WithMany()
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FreezerTape2.Models.Carcass", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("FreezerTape2.Models.PrimalCut", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("FreezerTape2.Models.Specie", b =>
                {
                    b.Navigation("Carcasses");
                });

            modelBuilder.Entity("FreezerTape2.Models.StoragePlace", b =>
                {
                    b.Navigation("Packages");
                });
#pragma warning restore 612, 618
        }
    }
}
