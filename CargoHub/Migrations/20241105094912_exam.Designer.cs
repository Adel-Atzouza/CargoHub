﻿// <auto-generated />
using System;
using CargoHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CargoHub.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241105094912_exam")]
    partial class exam
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("CargoHub.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("CargoHub.Models.Warehouse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "created_at");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Province")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "updated_at");

                    b.Property<string>("Zip")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContactId")
                        .IsUnique();

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("CargoHub.Models.Warehouse", b =>
                {
                    b.HasOne("CargoHub.Contact", "Contact")
                        .WithOne()
                        .HasForeignKey("CargoHub.Models.Warehouse", "ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });
#pragma warning restore 612, 618
        }
    }
}
