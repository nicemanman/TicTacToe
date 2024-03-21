﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(UnitOfWork))]
    [Migration("20240321150020_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Server.DataModel.Game", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("UUID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Server.DataModel.Game", b =>
                {
                    b.OwnsOne("Server.DataModel.GameMap", "GameMap", b1 =>
                        {
                            b1.Property<Guid>("GameUUID")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("UUID")
                                .HasColumnType("uuid");

                            b1.HasKey("GameUUID");

                            b1.ToTable("Games");

                            b1.WithOwner()
                                .HasForeignKey("GameUUID");

                            b1.OwnsMany("Server.DataModel.GameMapField", "Map", b2 =>
                                {
                                    b2.Property<Guid>("GameMapGameUUID")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b2.Property<int>("Id"));

                                    b2.Property<Guid>("UUID")
                                        .HasColumnType("uuid");

                                    b2.HasKey("GameMapGameUUID", "Id");

                                    b2.ToTable("GameMapField");

                                    b2.WithOwner()
                                        .HasForeignKey("GameMapGameUUID");
                                });

                            b1.Navigation("Map");
                        });

                    b.Navigation("GameMap")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
