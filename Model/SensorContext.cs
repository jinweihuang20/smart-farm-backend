using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Smart_farm.Model
{
    public partial class SensorContext : DbContext
    {
        public SensorContext()
        {
        }

        public SensorContext(DbContextOptions<SensorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<D1wifi> D1wifis { get; set; } = null!;
        public virtual DbSet<Mega2560> Mega2560s { get; set; } = null!;
        public virtual DbSet<RealTime> RealTimes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("server=ec2-34-235-198-25.compute-1.amazonaws.com;Port=5432;Database=dfhgdavbo9j8v3; User Id=qhipqkqnqdpkil;Password=835916d79baeaf71b930a7f9851ae0775359dd5707fe0903de9e7c1ee1fecea9;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<D1wifi>(entity =>
            {
                entity.HasKey(e => e.Datetime)
                    .HasName("D1WIFI_pkey");

                entity.ToTable("D1WIFI");

                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");

                entity.Property(e => e.Humidity).HasColumnName("humidity");

                entity.Property(e => e.Raw).HasColumnName("raw");

                entity.Property(e => e.Relayon).HasColumnName("relayon");
            });

            modelBuilder.Entity<Mega2560>(entity =>
            {
                entity.HasKey(e => e.Datetime)
                    .HasName("Mega2560_pkey");

                entity.ToTable("Mega2560");

                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");

                entity.Property(e => e.Humidity).HasColumnName("humidity");

                entity.Property(e => e.Raw).HasColumnName("raw");

                entity.Property(e => e.Relayon).HasColumnName("relayon");
            });

            modelBuilder.Entity<RealTime>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("RealTime_pkey");

                entity.ToTable("RealTime");

                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");

                entity.Property(e => e.Humidity).HasColumnName("humidity");

                entity.Property(e => e.Raw).HasColumnName("raw");

                entity.Property(e => e.Relayon).HasColumnName("relayon");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
