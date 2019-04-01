using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WpfResumeBrowsingSystem.Domain.Models
{
    public partial class ResumeBrowingSystemV00Context : DbContext
    {
        public ResumeBrowingSystemV00Context()
        {
        }

        public ResumeBrowingSystemV00Context(DbContextOptions<ResumeBrowingSystemV00Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Experiences> Experiences { get; set; }
        public virtual DbSet<Staffs> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL(@"server=47.94.162.230;user id=root;password=123456;database=ResumeBrowingSystemV00; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Experiences>(entity =>
            {
                entity.HasKey(e => e.Eid);

                entity.ToTable("Experiences", "ResumeBrowingSystemV00");

                entity.HasIndex(e => e.StaffSid)
                    .HasName("IX_Staff_Sid");

                entity.Property(e => e.Eid).HasColumnType("int(11)");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.End).HasColumnType("date");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StaffSid)
                    .HasColumnName("Staff_Sid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Start).HasColumnType("date");

                entity.HasOne(d => d.StaffS)
                    .WithMany(p => p.Experiences)
                    .HasForeignKey(d => d.StaffSid);
            });

            modelBuilder.Entity<Staffs>(entity =>
            {
                entity.HasKey(e => e.Sid);

                entity.ToTable("Staffs", "ResumeBrowingSystemV00");

                entity.Property(e => e.Sid).HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Age).HasColumnType("int(11)");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Education)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image).HasColumnType("bit(1)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Post)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.School)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sex).HasColumnType("char(1)");

                entity.Property(e => e.Specialty)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
