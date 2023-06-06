﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersistentLayer.Configurations;

#nullable disable

namespace PersistentLayer.Migrations
{
    [DbContext(typeof(ConcordiaDbContext))]
    partial class ConcordiaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExperimentScientist", b =>
                {
                    b.Property<int>("ExperimentsId")
                        .HasColumnType("int");

                    b.Property<int>("ScientistsId")
                        .HasColumnType("int");

                    b.HasKey("ExperimentsId", "ScientistsId");

                    b.HasIndex("ScientistsId");

                    b.ToTable("ExperimentScientist");
                });

            modelBuilder.Entity("PersistentLayer.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExperimentId")
                        .HasColumnType("int");

                    b.Property<string>("TrelloId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PersistentLayer.Models.Experiment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeadLine")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LabelId")
                        .HasColumnType("int");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrelloId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.HasIndex("ListId");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("PersistentLayer.Models.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrelloId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("PersistentLayer.Models.ListEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrelloId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("PersistentLayer.Models.Scientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrelloMemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrelloToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Scientist");
                });

            modelBuilder.Entity("ExperimentScientist", b =>
                {
                    b.HasOne("PersistentLayer.Models.Experiment", null)
                        .WithMany()
                        .HasForeignKey("ExperimentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersistentLayer.Models.Scientist", null)
                        .WithMany()
                        .HasForeignKey("ScientistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersistentLayer.Models.Comment", b =>
                {
                    b.HasOne("PersistentLayer.Models.Experiment", "Experiment")
                        .WithMany("Comments")
                        .HasForeignKey("ExperimentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Experiment");
                });

            modelBuilder.Entity("PersistentLayer.Models.Experiment", b =>
                {
                    b.HasOne("PersistentLayer.Models.Label", "Label")
                        .WithMany()
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersistentLayer.Models.ListEntity", "List")
                        .WithMany("Experiments")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");

                    b.Navigation("List");
                });

            modelBuilder.Entity("PersistentLayer.Models.Experiment", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("PersistentLayer.Models.ListEntity", b =>
                {
                    b.Navigation("Experiments");
                });
#pragma warning restore 612, 618
        }
    }
}
