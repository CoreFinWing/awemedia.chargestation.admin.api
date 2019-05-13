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
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("AwemediaConnection_staging"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChargeOptions>(entity =>
            {
                entity.HasIndex(e => new { e.ChargeDuration, e.Price, e.Currency })
                    .HasName("IX_ChargeOptions")
                    .IsUnique();

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
                entity.HasIndex(e => e.DeviceId)
                    .HasName("IX_ChargeStations_DeviceId")
                    .IsUnique();

                entity.HasIndex(e => e.Uid)
                    .HasName("IX_ChargeStations_UID")
                    .IsUnique();

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

                entity.Property(e => e.MerchantId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasIndex(e => e.EventTypeId)
                    .HasName("fkIdx_Events_EventType");

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
        }
    }
}
