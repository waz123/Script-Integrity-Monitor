﻿// <auto-generated />
using System;
using DBA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBA.Migrations
{
    [DbContext(typeof(DbaDbContext))]
    [Migration("20240917225408_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DBA.Tables.S_Scripts", b =>
                {
                    b.Property<int>("scriptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("scriptID"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("hash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isAllowed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("scanDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("subdomain")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("scriptID");

                    b.ToTable("S_Scripts");
                });
#pragma warning restore 612, 618
        }
    }
}
