﻿// <auto-generated />
using System;
using ElectronicObserverDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KancolleSimulator.Migrations
{
    [DbContext(typeof(MasterDataContext))]
    [Migration("20191129052600_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0-preview3.19554.8");

            modelBuilder.Entity("KancolleSimulator.Models.Ships", b =>
                {
                    b.Property<long>("ShipId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AaMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AaMin")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Aircraft1")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Aircraft2")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Aircraft3")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Aircraft4")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Aircraft5")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ArmorMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ArmorMin")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AswMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AswMinLowerBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AswMinUpperBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Equipment1")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Equipment2")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Equipment3")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Equipment4")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Equipment5")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EvasionMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EvasionMinLowerBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EvasionMinUpperBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("FirepowerMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("FirepowerMin")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("HpMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("HpMin")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LosMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LosMinLowerBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LosMinUpperBound")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LuckMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LuckMin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MessageAlbum")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageGet")
                        .HasColumnType("TEXT");

                    b.Property<long?>("OriginalCostumeShipId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Range")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResourceGraphicVersion")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResourceName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResourcePortVoiceVersion")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResourceVoiceVersion")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShipName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TorpedoMax")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TorpedoMin")
                        .HasColumnType("INTEGER");

                    b.HasKey("ShipId");

                    b.HasIndex("ShipId")
                        .IsUnique();

                    b.ToTable("Ships");
                });
#pragma warning restore 612, 618
        }
    }
}
