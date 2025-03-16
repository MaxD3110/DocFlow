﻿// <auto-generated />
using System;
using FileManagementService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FileManagementService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250316103722_extensions2")]
    partial class extensions2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExtensionConversion", b =>
                {
                    b.Property<int>("SourceExtensionId")
                        .HasColumnType("integer");

                    b.Property<int>("TargetExtensionId")
                        .HasColumnType("integer");

                    b.HasKey("SourceExtensionId", "TargetExtensionId");

                    b.HasIndex("TargetExtensionId");

                    b.ToTable("ExtensionConversion");
                });

            modelBuilder.Entity("FileManagementService.Models.Extension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FilenameExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MediaType")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Extensions");
                });

            modelBuilder.Entity("FileManagementService.Models.FileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ExtensionId")
                        .HasColumnType("integer");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("StoragePath")
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ExtensionId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ExtensionConversion", b =>
                {
                    b.HasOne("FileManagementService.Models.Extension", null)
                        .WithMany()
                        .HasForeignKey("SourceExtensionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FileManagementService.Models.Extension", null)
                        .WithMany()
                        .HasForeignKey("TargetExtensionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FileManagementService.Models.FileModel", b =>
                {
                    b.HasOne("FileManagementService.Models.Extension", "Extension")
                        .WithMany()
                        .HasForeignKey("ExtensionId");

                    b.Navigation("Extension");
                });
#pragma warning restore 612, 618
        }
    }
}
