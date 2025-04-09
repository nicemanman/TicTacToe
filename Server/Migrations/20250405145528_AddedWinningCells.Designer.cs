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
    [Migration("20250405145528_AddedWinningCells")]
    partial class AddedWinningCells
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

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WinningCells")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("UUID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Server.DataModel.GameSession", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("GameState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GameUUID")
                        .HasColumnType("uuid");

                    b.Property<string>("JoinCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Player1Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Player2Id")
                        .HasColumnType("text");

                    b.Property<string>("PlayerIdTurn")
                        .HasColumnType("text");

                    b.Property<string>("PlayerIdWin")
                        .HasColumnType("text");

                    b.HasKey("UUID");

                    b.HasIndex("GameUUID");

                    b.HasIndex("Player1Id")
                        .IsUnique()
                        .HasFilter("'GameState'='InProgress'");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("Server.DataModel.Game", b =>
                {
                    b.OwnsOne("Server.DataModel.GameMap", "GameMap", b1 =>
                        {
                            b1.Property<Guid>("GameUUID")
                                .HasColumnType("uuid");

                            b1.HasKey("GameUUID");

                            b1.ToTable("Games");

                            b1.WithOwner()
                                .HasForeignKey("GameUUID");

                            b1.OwnsMany("Server.DataModel.GameMapField", "Fields", b2 =>
                                {
                                    b2.Property<Guid>("GameMapGameUUID")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b2.Property<int>("Id"));

                                    b2.Property<string>("Char")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<int>("Column")
                                        .HasColumnType("integer");

                                    b2.Property<int>("Row")
                                        .HasColumnType("integer");

                                    b2.HasKey("GameMapGameUUID", "Id");

                                    b2.ToTable("GameMapField");

                                    b2.WithOwner()
                                        .HasForeignKey("GameMapGameUUID");
                                });

                            b1.Navigation("Fields");
                        });

                    b.Navigation("GameMap")
                        .IsRequired();
                });

            modelBuilder.Entity("Server.DataModel.GameSession", b =>
                {
                    b.HasOne("Server.DataModel.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });
#pragma warning restore 612, 618
        }
    }
}
