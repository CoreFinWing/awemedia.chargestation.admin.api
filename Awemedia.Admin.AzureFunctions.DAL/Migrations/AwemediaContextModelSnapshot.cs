﻿// <auto-generated />
using System;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    [DbContext(typeof(AwemediaContext))]
    partial class AwemediaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Geolocation")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("MerchantId");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChargeDuration")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ChargeDuration", "Price", "Currency")
                        .IsUnique()
                        .HasName("IX_ChargeOptions");

                    b.ToTable("ChargeOptions");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("ChargeControllerId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DeviceToken")
                        .IsUnicode(false);

                    b.Property<string>("Geolocation")
                        .IsUnicode(false);

                    b.Property<string>("MerchantId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsUnique()
                        .HasName("IX_ChargeStations_DeviceId");

                    b.HasIndex("Uid")
                        .IsUnique()
                        .HasName("IX_ChargeStations_UID");

                    b.ToTable("ChargeStation");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("ChargeStationId");

                    b.Property<string>("DateTime")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("EventTypeId");

                    b.Property<bool>("IsActive");

                    b.HasKey("Id");

                    b.HasIndex("EventTypeId")
                        .HasName("fkIdx_Events_EventType");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("EventType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.IndustryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("IndustryType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ChargeStationsOrdered")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ContactName")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Dba")
                        .HasColumnName("DBA")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DepositMoneyPaid")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("IndustryTypeId");

                    b.Property<string>("LicenseNum")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ProfitSharePercentage")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SecondaryContact")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SecondaryPhone")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("IndustryTypeId");

                    b.ToTable("Merchant");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<string>("DeviceToken")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<DateTime>("LoggedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("NotificationResult")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<string>("Payload")
                        .IsRequired()
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", "Merchant")
                        .WithMany("Branch")
                        .HasForeignKey("MerchantId")
                        .HasConstraintName("FK_Branch_Merchant");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Events", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.EventType", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeId")
                        .HasConstraintName("FK_EventID_EventTypeId");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.IndustryType", "IndustryType")
                        .WithMany("Merchant")
                        .HasForeignKey("IndustryTypeId")
                        .HasConstraintName("FK_Merchant_IndustryType");
                });
#pragma warning restore 612, 618
        }
    }
}
