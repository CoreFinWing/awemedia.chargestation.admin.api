﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class AwemediaContext : DbContext
    {
        public AwemediaContext()
        {
        }

        public AwemediaContext(DbContextOptions<AwemediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<ChargeOptions> ChargeOptions { get; set; }
        public virtual DbSet<ChargeStation> ChargeStation { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<IndustryType> IndustryType { get; set; }
        public virtual DbSet<Merchant> Merchant { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<SessionStatus> SessionStatus { get; set; }
        public virtual DbSet<SessionType> SessionType { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:awemedia-chargestation-sqlsvr-test.database.windows.net,1433;Initial Catalog=awemedia-chargestation-db-test;Persist Security Info=False;User ID=it-admin;Password=sdf@23Yusda;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Geolocation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_Merchant");
            });

            modelBuilder.Entity<ChargeOptions>(entity =>
            {
                entity.Property(e => e.ChargeDuration)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<ChargeStation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ChargeControllerId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceToken).IsUnicode(false);

                entity.Property(e => e.Geolocation).IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.ChargeStation)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_ChargeStation_Branch");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.DateTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.EventData)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventID_EventTypeId");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IndustryType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeStationsOrdered)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Dba)
                    .HasColumnName("DBA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepositMoneyPaid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseNum)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProfitSharePercentage)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecondaryContact)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecondaryPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IndustryType)
                    .WithMany(p => p.Merchant)
                    .HasForeignKey(d => d.IndustryTypeId)
                    .HasConstraintName("FK_Merchant_IndustryType");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DeviceToken)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LoggedDateTime).HasColumnType("datetime");

                entity.Property(e => e.NotificationResult)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Payload)
                    .IsRequired()
                    .IsUnicode(false);
                entity.HasOne(d => d.UserSession)
                     .WithMany(p => p.Notifications)
                     .HasForeignKey(d => d.UserSessionId)
                     .HasConstraintName("FK_UserSession_Notifications");
            });

            modelBuilder.Entity<SessionStatus>(entity =>
            {
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SessionType>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AppKey).IsUnicode(false);

                entity.Property(e => e.ApplicationId).IsUnicode(false);

                entity.Property(e => e.ChargeParams)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeRentalRevnue).HasColumnType("money");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SessionEndTime).HasColumnType("datetime");

                entity.Property(e => e.SessionStartTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ChargeStation)
                    .WithMany(p => p.UserSession)
                    .HasForeignKey(d => d.ChargeStationId)
                    .HasConstraintName("FK_UserSession_ChargeStation");

                entity.HasOne(d => d.SessionStatusNavigation)
                    .WithMany(p => p.UserSession)
                    .HasForeignKey(d => d.SessionStatus)
                    .HasConstraintName("FK_SessionStatus_UserSession");

                entity.HasOne(d => d.SessionTypeNavigation)
                    .WithMany(p => p.UserSession)
                    .HasForeignKey(d => d.SessionType)
                    .HasConstraintName("FK_SessionType_UserSession");


            });
        }
    }
}
