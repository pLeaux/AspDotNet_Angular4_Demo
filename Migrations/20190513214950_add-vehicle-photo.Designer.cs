﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VEGA.Models;

namespace VEGA.Migrations
{
    [DbContext(typeof(VegaDbContext))]
    [Migration("20190513214950_add-vehicle-photo")]
    partial class addvehiclephoto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VEGA.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("VEGA.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("VEGA.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrandId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("VEGA.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<int>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("VEGA.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(64);

                    b.Property<string>("ContactName")
                        .HasMaxLength(64);

                    b.Property<string>("ContactPhone")
                        .HasMaxLength(64);

                    b.Property<bool>("IsRegistered");

                    b.Property<int>("ModelId");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("VEGA.Models.VehicleFeature", b =>
                {
                    b.Property<int>("VehicleId");

                    b.Property<int>("FeatureId");

                    b.HasKey("VehicleId", "FeatureId");

                    b.HasAlternateKey("FeatureId", "VehicleId");

                    b.ToTable("VehicleFeatures");
                });

            modelBuilder.Entity("VEGA.Models.Model", b =>
                {
                    b.HasOne("VEGA.Models.Brand", "Brand")
                        .WithMany("Models")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VEGA.Models.Photo", b =>
                {
                    b.HasOne("VEGA.Models.Vehicle")
                        .WithMany("Photos")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VEGA.Models.Vehicle", b =>
                {
                    b.HasOne("VEGA.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VEGA.Models.VehicleFeature", b =>
                {
                    b.HasOne("VEGA.Models.Feature", "Feature")
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VEGA.Models.Vehicle", "Vehicle")
                        .WithMany("Features")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
