﻿// <auto-generated />
using System;
using Forum.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Forum.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Forum.Domain.Entities.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeOfCreation")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                            Level = 0,
                            QuestionId = new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            Text = "Text 1",
                            TimeOfCreation = new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5502)
                        },
                        new
                        {
                            Id = new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                            Level = 1,
                            ParentId = new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                            QuestionId = new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            Text = "Text 1-1",
                            TimeOfCreation = new DateTime(2024, 10, 14, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5518)
                        },
                        new
                        {
                            Id = new Guid("31ed4f16-6f88-42f4-bc9e-ede50898b39a"),
                            Level = 2,
                            ParentId = new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                            QuestionId = new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            Text = "Text 1-1-1",
                            TimeOfCreation = new DateTime(2024, 10, 15, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5521)
                        },
                        new
                        {
                            Id = new Guid("2a621bcb-01ae-4803-8584-fe0542fdee5c"),
                            Level = 1,
                            ParentId = new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                            QuestionId = new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            Text = "Text 1-2",
                            TimeOfCreation = new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5524)
                        },
                        new
                        {
                            Id = new Guid("126719aa-4937-484f-a1ce-0151730e4457"),
                            Level = 0,
                            QuestionId = new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            Text = "Text 2",
                            TimeOfCreation = new DateTime(2024, 10, 12, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5527)
                        });
                });

            modelBuilder.Entity("Forum.Domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Questions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            AuthorId = new Guid("24f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            DateOfCreation = new DateTime(2024, 10, 6, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6815),
                            Description = "Description 1",
                            IsClosed = false,
                            Title = "Title 1"
                        },
                        new
                        {
                            Id = new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            AuthorId = new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            DateOfCreation = new DateTime(2024, 9, 16, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6819),
                            Description = "Description 2",
                            IsClosed = false,
                            Title = "Title 2"
                        },
                        new
                        {
                            Id = new Guid("35f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            AuthorId = new Guid("26f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            DateOfCreation = new DateTime(2024, 10, 15, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6832),
                            Description = "Description 3",
                            IsClosed = false,
                            Title = "Title 3"
                        },
                        new
                        {
                            Id = new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            AuthorId = new Guid("26f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                            DateOfCreation = new DateTime(2024, 10, 11, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6835),
                            Description = "Description 4",
                            IsClosed = false,
                            Title = "Title 4"
                        });
                });

            modelBuilder.Entity("Forum.Domain.Entities.Answer", b =>
                {
                    b.HasOne("Forum.Domain.Entities.Answer", "Parent")
                        .WithMany("Childrens")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Forum.Domain.Entities.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Forum.Domain.Entities.Answer", b =>
                {
                    b.Navigation("Childrens");
                });

            modelBuilder.Entity("Forum.Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
