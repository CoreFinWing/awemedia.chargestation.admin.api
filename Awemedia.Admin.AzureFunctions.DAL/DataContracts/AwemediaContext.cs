using System;
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

        public virtual DbSet<ChargeOptions> ChargeOptions { get; set; }
        public virtual DbSet<ChargeStation> ChargeStation { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=Awemedia;user=sa;password=login@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Geolocation).IsUnicode(false);

                entity.Property(e => e.MerchantId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasIndex(e => e.DeviceId)
                    .HasName("fkIdx_197");

                entity.HasIndex(e => e.EventTypeId)
                    .HasName("fkIdx_186");

                entity.Property(e => e.DateTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventData)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceId_ChargestationId");

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
        }
    }
}
