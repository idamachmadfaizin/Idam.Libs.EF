﻿// <auto-generated />
using System;
using Idam.Libs.EF.Sample.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Idam.Libs.EF.Sample.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20230606170553_CreateBoosTable")]
    partial class CreateBoosTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Idam.Libs.EF.Sample.Models.Entity.Boo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(191)
                        .HasColumnType("nvarchar(191)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(191)
                        .HasColumnType("nvarchar(191)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Boos");
                });

            modelBuilder.Entity("Idam.Libs.EF.Sample.Models.Entity.Foo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<long?>("DeletedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(191)
                        .HasColumnType("nvarchar(191)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(191)
                        .HasColumnType("nvarchar(191)");

                    b.Property<long>("UpdatedAt")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Foos");
                });
#pragma warning restore 612, 618
        }
    }
}
