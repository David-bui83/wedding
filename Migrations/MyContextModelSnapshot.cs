﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wedding.Models;

namespace wedding.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("wedding.Models.RSVP", b =>
                {
                    b.Property<int>("RSVPId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Rsvp");

                    b.Property<int>("UserId");

                    b.Property<int>("WeddingId");

                    b.HasKey("RSVPId");

                    b.HasIndex("UserId");

                    b.HasIndex("WeddingId");

                    b.ToTable("RSVPs");
                });

            modelBuilder.Entity("wedding.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("wedding.Models.Wedding", b =>
                {
                    b.Property<int>("WeddingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.Property<string>("WedderOne")
                        .IsRequired();

                    b.Property<string>("WedderTwo")
                        .IsRequired();

                    b.Property<DateTime>("WeddingDate");

                    b.HasKey("WeddingId");

                    b.HasIndex("UserId");

                    b.ToTable("Weddings");
                });

            modelBuilder.Entity("wedding.Models.RSVP", b =>
                {
                    b.HasOne("wedding.Models.User", "User")
                        .WithMany("WeddingRsvpList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("wedding.Models.Wedding", "Wedding")
                        .WithMany("UserRsvp")
                        .HasForeignKey("WeddingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("wedding.Models.Wedding", b =>
                {
                    b.HasOne("wedding.Models.User", "creator")
                        .WithMany("WeddingList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
