using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniGradeWebApp;

public partial class DbUniGradeSystemContext : DbContext
{
    public DbUniGradeSystemContext()
    {
    }

    public DbUniGradeSystemContext(DbContextOptions<DbUniGradeSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cathedra> Cathedras { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-LFAOE14\\SQLEXPRESS;\nDatabase=DB_Uni_Grade_System; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cathedra>(entity =>
        {
            entity.HasKey(e => e.CathId);

            entity.ToTable("Cathedra");

            entity.Property(e => e.CathId).HasColumnName("CathID");
            entity.Property(e => e.CathName)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.CathFacNavigation).WithMany(p => p.Cathedras)
                .HasForeignKey(d => d.CathFac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cathedras_Faculty");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacId);

            entity.ToTable("Faculty");

            entity.Property(e => e.FacId).HasColumnName("FacID");
            entity.Property(e => e.FacName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GrdId);

            entity.ToTable("Grade");

            entity.Property(e => e.GrdId).HasColumnName("GrdID");

            entity.HasOne(d => d.GrdSbjNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.GrdSbj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Subject");

            entity.HasOne(d => d.GrdStnNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.GrdStn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Student");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GrpId);

            entity.ToTable("Group");

            entity.Property(e => e.GrpId).HasColumnName("GrpID");
            entity.Property(e => e.GrpName)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.HasOne(d => d.GrpCathNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.GrpCath)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Groups_Cathedra");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StnId);

            entity.ToTable("Student");

            entity.Property(e => e.StnId).HasColumnName("StnID");
            entity.Property(e => e.StnFullName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.StnGrpNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.StnGrp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Group");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SbjId);

            entity.ToTable("Subject");

            entity.Property(e => e.SbjId).HasColumnName("SbjID");
            entity.Property(e => e.SbjTeach)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.SbjCathNavigation).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.SbjCath)
                .HasConstraintName("FK_Subjects_Cathedra");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
