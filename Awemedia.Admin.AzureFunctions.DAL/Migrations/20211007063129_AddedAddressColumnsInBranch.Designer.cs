﻿// <auto-generated />
using System;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    [DbContext(typeof(AwemediaContext))]
    [Migration("20211007063129_AddedAddressColumnsInBranch")]
    partial class AddedAddressColumnsInBranch
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Geolocation")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChargeDuration")
                        .HasColumnType("int");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("ChargeOptions");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BatteryLevel")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("ChargeControllerId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DeviceToken")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("Geolocation")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<string>("LastBatteryLevel")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("LastBatteryLevelAvailablityTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("LastPingTimeStamp")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("ChargeStation");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("EventType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("ChargeStationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DateTime")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ServerDateTime")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.IndustryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("IndustryType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ChargeStationsOrdered")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ContactName")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Dba")
                        .HasColumnName("DBA")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("DepositMoneyPaid")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("IndustryTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LicenseNum")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("NumOfActiveLocations")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ProfitSharePercentage")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SecondaryContact")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SecondaryPhone")
                        .HasColumnType("varchar(50)")
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
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeviceId")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("DeviceToken")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<DateTime?>("LoggedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("NotificationResult")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("Payload")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserSessionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserSessionId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("PromotionDesc")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<int?>("PromotionType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("Promotion");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("SessionStatus");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("SessionType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppKey")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("ApplicationId")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("ChargeParams")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<decimal?>("ChargeRentalRevnue")
                        .HasColumnType("money");

                    b.Property<Guid?>("ChargeStationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeviceId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("SessionEndTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("SessionStartTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("SessionStatus")
                        .HasColumnType("int");

                    b.Property<int?>("SessionType")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("TransactionTypeId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("UserAccountId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("ChargeStationId");

                    b.HasIndex("SessionStatus");

                    b.HasIndex("SessionType");

                    b.ToTable("UserSession");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", "Merchant")
                        .WithMany("Branch")
                        .HasForeignKey("MerchantId")
                        .HasConstraintName("FK_Branch_Merchant")
                        .IsRequired();
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeOptions", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Country", "Country")
                        .WithMany("ChargeOptions")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_ChargeOptions_Country");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", "Branch")
                        .WithMany("ChargeStation")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_ChargeStation_Branch");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Events", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.EventType", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeId")
                        .HasConstraintName("FK_EventID_EventTypeId")
                        .IsRequired();
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Merchant", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.IndustryType", "IndustryType")
                        .WithMany("Merchant")
                        .HasForeignKey("IndustryTypeId")
                        .HasConstraintName("FK_Merchant_IndustryType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Notification", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession", "UserSession")
                        .WithMany("Notification")
                        .HasForeignKey("UserSessionId")
                        .HasConstraintName("FK_UserSession_Notifications");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Promotion", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.Branch", "Branch")
                        .WithMany("Promotion")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_Promotion_Branch");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession", b =>
                {
                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation", "ChargeStation")
                        .WithMany("UserSession")
                        .HasForeignKey("ChargeStationId")
                        .HasConstraintName("FK_UserSession_ChargeStation");

                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionStatus", "SessionStatusNavigation")
                        .WithMany("UserSession")
                        .HasForeignKey("SessionStatus")
                        .HasConstraintName("FK_SessionStatus_UserSession");

                    b.HasOne("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionType", "SessionTypeNavigation")
                        .WithMany("UserSession")
                        .HasForeignKey("SessionType")
                        .HasConstraintName("FK_SessionType_UserSession");
                });
#pragma warning restore 612, 618
        }
    }
}