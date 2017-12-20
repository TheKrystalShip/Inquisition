﻿// <auto-generated />
using Inquisition.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Inquisition.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inquisition.Data.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("TargetUserId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TargetUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("Inquisition.Data.Game", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("Exe");

                    b.Property<bool>("IsOnline");

                    b.Property<string>("LaunchArgs");

                    b.Property<string>("Port")
                        .HasMaxLength(10);

                    b.Property<string>("Version")
                        .HasMaxLength(10);

                    b.HasKey("Name");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Inquisition.Data.Joke", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Jokes");
                });

            modelBuilder.Entity("Inquisition.Data.Meme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("Url");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Memes");
                });

            modelBuilder.Entity("Inquisition.Data.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("Name");

                    b.Property<int?>("SongId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Inquisition.Data.Reminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreateDate");

                    b.Property<DateTimeOffset>("DueDate");

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("Message");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Inquisition.Data.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Duration");

                    b.Property<string>("Name");

                    b.Property<int?>("PlaylistId");

                    b.Property<string>("Url");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("UserId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Inquisition.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("Discriminator");

                    b.Property<DateTimeOffset?>("JoinedAt");

                    b.Property<DateTimeOffset>("LastSeenOnline");

                    b.Property<string>("Nickname");

                    b.Property<int?>("TimezoneOffset");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inquisition.Data.Alert", b =>
                {
                    b.HasOne("Inquisition.Data.User", "TargetUser")
                        .WithMany("TargetAlerts")
                        .HasForeignKey("TargetUserId");

                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Alerts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inquisition.Data.Joke", b =>
                {
                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Jokes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inquisition.Data.Meme", b =>
                {
                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Memes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inquisition.Data.Playlist", b =>
                {
                    b.HasOne("Inquisition.Data.Song")
                        .WithMany("Playlists")
                        .HasForeignKey("SongId");

                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Inquisition.Data.Reminder", b =>
                {
                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Reminders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inquisition.Data.Song", b =>
                {
                    b.HasOne("Inquisition.Data.Playlist")
                        .WithMany("Songs")
                        .HasForeignKey("PlaylistId");

                    b.HasOne("Inquisition.Data.User", "User")
                        .WithMany("Songs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
