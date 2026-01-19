using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DrivingLicenseExam.Models;

public partial class DrivingLicenseExamDbContext : DbContext
{
    public DrivingLicenseExamDbContext()
    {
    }

    public DrivingLicenseExamDbContext(DbContextOptions<DrivingLicenseExamDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<ExamResult> ExamResults { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=DrivingLicenseExamDB;User Id=sa;Password=123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC0784F6F1C7");

            entity.ToTable("Exam");

            entity.Property(e => e.Name)
                .HasMaxLength(100);

            entity.Property(e => e.ExamCode)
                .HasMaxLength(50)      // hoặc đúng size DB
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasColumnType("bit");
        });


        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamQues__3214EC074C85CDA2");

            entity.ToTable("ExamQuestion");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamQuestion_Exam");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamQuestion_Question");
        });

        modelBuilder.Entity<ExamResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamResu__3214EC077D08C6E5");

            entity.ToTable("ExamResult");

            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamResults)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamResult_Exam");

            entity.HasOne(d => d.User).WithMany(p => p.ExamResults)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamResult_User");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07C0D65860");

            entity.ToTable("Question");

            entity.Property(e => e.AnswerA).HasMaxLength(255);
            entity.Property(e => e.AnswerB).HasMaxLength(255);
            entity.Property(e => e.AnswerC).HasMaxLength(255);
            entity.Property(e => e.AnswerD).HasMaxLength(255);
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0758741C0B");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4AD4B9629").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
