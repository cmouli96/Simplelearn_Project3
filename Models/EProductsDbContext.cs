using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace e_commerce.Models
{
    public partial class EProductsDbContext : DbContext
    {
        public EProductsDbContext()
        {
        }

        public EProductsDbContext(DbContextOptions<EProductsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=H5CG1220K22\\MSSQLSERVER01;integrated security=true;database=EProductsDb ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => new { e.Uname, e.Pid });

                entity.ToTable("tblOrders");

                entity.Property(e => e.Uname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UName");

                entity.Property(e => e.Pname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PName");

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SName");

                entity.HasOne(d => d.PidNavigation)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.Pid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrders_tblProducts");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.Pid);

                entity.ToTable("tblProducts");

                entity.Property(e => e.Pname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PName");

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
