﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Store.Data;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20230830074811_newMig")]
    partial class newMig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Store.Models.Entities.Category", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Store.Models.Entities.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("jsonb")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<decimal?>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<string>("Sku")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("sku");

                    b.Property<int?>("catId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.HasKey("Id");

                    b.HasIndex("catId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("Store.Models.Entities.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<short?>("catId")
                        .HasColumnType("smallint")
                        .HasColumnName("category_id");

                    b.HasKey("Id");

                    b.HasIndex("catId");

                    b.ToTable("sub_categories");
                });

            modelBuilder.Entity("Store.Models.Entities.Product", b =>
                {
                    b.HasOne("Store.Models.Entities.SubCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("catId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Store.Models.Entities.SubCategory", b =>
                {
                    b.HasOne("Store.Models.Entities.Category", "Category")
                        .WithMany("Subcats")
                        .HasForeignKey("catId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Store.Models.Entities.Category", b =>
                {
                    b.Navigation("Subcats");
                });

            modelBuilder.Entity("Store.Models.Entities.SubCategory", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
