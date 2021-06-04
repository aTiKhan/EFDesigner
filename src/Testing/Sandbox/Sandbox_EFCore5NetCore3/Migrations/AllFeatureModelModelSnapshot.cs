﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Testing;

namespace Testing.Migrations
{
    [DbContext(typeof(AllFeatureModel))]
    partial class AllFeatureModelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0-rc.1.20451.13");

            modelBuilder.Entity("Entity1Entity2", b =>
                {
                    b.Property<long>("Entity1Id")
                        .HasColumnType("bigint");

                    b.Property<long>("Entity2_1Id")
                        .HasColumnType("bigint");

                    b.HasKey("Entity1Id", "Entity2_1Id");

                    b.HasIndex("Entity2_1Id");

                    b.ToTable("Entity1Entity2");
                });

            modelBuilder.Entity("Testing.Base", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Entity11_Id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Entity11_Id");

                    b.ToTable("Bases");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Base");
                });

            modelBuilder.Entity("Testing.Entity1", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long?>("Entity2Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Property1")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Property2")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Property3")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("Entity2Id");

                    b.ToTable("Entity1");
                });

            modelBuilder.Entity("Testing.Entity11", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Property1")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Property2")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Property3")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Entity11");
                });

            modelBuilder.Entity("Testing.Entity2", b =>
                {
                    b.HasBaseType("Testing.Base");

                    b.Property<string>("Property1")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Property2")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasDiscriminator().HasValue("Entity2");
                });

            modelBuilder.Entity("Entity1Entity2", b =>
                {
                    b.HasOne("Testing.Entity1", null)
                        .WithMany()
                        .HasForeignKey("Entity1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Testing.Entity2", null)
                        .WithMany()
                        .HasForeignKey("Entity2_1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Testing.Base", b =>
                {
                    b.HasOne("Testing.Entity11", "Entity11")
                        .WithMany("Bases")
                        .HasForeignKey("Entity11_Id");

                    b.Navigation("Entity11");
                });

            modelBuilder.Entity("Testing.Entity1", b =>
                {
                    b.HasOne("Testing.Entity2", "Entity2")
                        .WithMany()
                        .HasForeignKey("Entity2Id");

                    b.Navigation("Entity2");
                });

            modelBuilder.Entity("Testing.Entity11", b =>
                {
                    b.Navigation("Bases");
                });
#pragma warning restore 612, 618
        }
    }
}
