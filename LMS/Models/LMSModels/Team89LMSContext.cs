using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class Team89LMSContext : DbContext
    {
        public Team89LMSContext()
        {
        }

        public Team89LMSContext(DbContextOptions<Team89LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<AssignmentCategory> AssignmentCategory { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Enrolled> Enrolled { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Submissions> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=atr.eng.utah.edu;User Id=u1078722;Password=hg1758;Database=Team89LMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<AssignmentCategory>(entity =>
            {
                entity.HasKey(e => e.AcId)
                    .HasName("PRIMARY");

                entity.ToTable("Assignment_Category");

                entity.HasIndex(e => e.ClassId)
                    .HasName("classID");

                entity.HasIndex(e => new { e.Name, e.ClassId })
                    .HasName("Name")
                    .IsUnique();

                entity.Property(e => e.AcId)
                    .HasColumnName("acID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ClassId)
                    .HasColumnName("classID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategory)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Assignment_Category_ibfk_1");
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasKey(e => e.AssId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AcId)
                    .HasName("acID");

                entity.HasIndex(e => new { e.Name, e.AcId })
                    .HasName("Name")
                    .IsUnique();

                entity.Property(e => e.AssId)
                    .HasColumnName("assID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.AcId)
                    .HasColumnName("acID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Contents).HasColumnType("varchar(8192)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Ac)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.AcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProfId)
                    .HasName("profID");

                entity.HasIndex(e => new { e.CId, e.SemesterSeason, e.SemesterYear, e.ProfId })
                    .HasName("cID")
                    .IsUnique();

                entity.Property(e => e.ClassId)
                    .HasColumnName("classID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CId)
                    .HasColumnName("cID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Location).HasColumnType("varchar(100)");

                entity.Property(e => e.ProfId)
                    .IsRequired()
                    .HasColumnName("profID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.SemesterSeason)
                    .IsRequired()
                    .HasColumnType("varchar(6)");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.Prof)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ProfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_2");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DId)
                    .HasName("dID");

                entity.HasIndex(e => new { e.Number, e.DId })
                    .HasName("Number")
                    .IsUnique();

                entity.Property(e => e.CId)
                    .HasColumnName("cID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.DId)
                    .HasColumnName("dID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Number).HasColumnType("int(10)");

                entity.HasOne(d => d.D)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Subject)
                    .HasName("Subject")
                    .IsUnique();

                entity.Property(e => e.DId)
                    .HasColumnName("dID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnType("varchar(4)");
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.ClassId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClassId)
                    .HasName("classID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.ClassId)
                    .HasColumnName("classID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Grade).HasColumnType("varchar(2)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_1");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DId)
                    .HasName("dID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.DId)
                    .HasColumnName("dID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.D)
                    .WithMany(p => p.Professor)
                    .HasForeignKey(d => d.DId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Professor_ibfk_1");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DId)
                    .HasName("dID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.DId)
                    .HasColumnName("dID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.D)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.DId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Student_ibfk_1");
            });

            modelBuilder.Entity<Submissions>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.AssId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AssId)
                    .HasName("assID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.AssId)
                    .HasColumnName("assID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Contents).HasColumnType("varchar(8192)");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Ass)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.AssId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_1");
            });
        }
    }
}
