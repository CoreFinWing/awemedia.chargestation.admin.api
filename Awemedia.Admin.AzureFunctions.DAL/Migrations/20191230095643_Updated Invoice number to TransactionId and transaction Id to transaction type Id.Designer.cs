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
    [Migration("20191230095643_Updated Invoice number to TransactionId and transaction Id to transaction type Id")]
    partial class UpdatedInvoicenumbertoTransactionIdandtransactionIdtotransactiontypeId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("IsActive");

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

                    b.Property<int>("ChargeDuration");

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

                    b.ToTable("ChargeOptions");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.ChargeStation", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("AppVersion");

                    b.Property<string>("BatteryLevel")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("BranchId");

                    b.Property<string>("ChargeControllerId")
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

                    b.Property<bool>("IsActive");

                    b.Property<bool?>("IsOnline");

                    b.Property<DateTime?>("LastPingTimeStamp")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

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

                    b.HasIndex("EventTypeId");

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

                    b.Property<bool>("IsActive");

                    b.Property<string>("LicenseNum")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("NumOfActiveLocations");

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

                    b.Property<string>("Status");

                    b.Property<Guid?>("UserSessionId");

                    b.HasKey("Id");

                    b.HasIndex("UserSessionId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("SessionStatus");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.SessionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("SessionType");
                });

            modelBuilder.Entity("Awemedia.Admin.AzureFunctions.DAL.DataContracts.UserSession", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("AppKey")
                        .IsUnicode(false);

                    b.Property<string>("ApplicationId")
                        .IsUnicode(false);

                    b.Property<string>("ChargeParams")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<decimal?>("ChargeRentalRevnue")
                        .HasColumnType("money");

                    b.Property<Guid?>("ChargeStationId");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeviceId")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Mobile")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("SessionEndTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("SessionStartTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("SessionStatus");

                    b.Property<int?>("SessionType");

                    b.Property<string>("TransactionId")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("TransactionTypeId")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("UserAccountId")
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
                        .HasConstraintName("FK_Branch_Merchant");
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
                        .HasConstraintName("FK_EventID_EventTypeId");
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
