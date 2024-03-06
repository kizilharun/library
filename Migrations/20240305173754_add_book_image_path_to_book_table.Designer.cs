﻿// <auto-generated />
using System;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Library.Migrations
{
    [DbContext(typeof(SysDataContext))]
    [Migration("20240305173754_add_book_image_path_to_book_table")]
    partial class add_book_image_path_to_book_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Library.Data.book", b =>
                {
                    b.Property<int>("book_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("book_id"));

                    b.Property<bool>("at_library")
                        .HasColumnType("bit");

                    b.Property<string>("book_author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("book_image_path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("book_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("book_id");

                    b.ToTable("book");
                });

            modelBuilder.Entity("Library.Data.borrower", b =>
                {
                    b.Property<int>("borrower_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("borrower_id"));

                    b.Property<int>("borrowed_book_id")
                        .HasColumnType("int");

                    b.Property<string>("borrower_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("borrowing_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("return_date")
                        .HasColumnType("datetime2");

                    b.HasKey("borrower_id");

                    b.ToTable("borrower");
                });
#pragma warning restore 612, 618
        }
    }
}