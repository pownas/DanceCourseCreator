using Microsoft.EntityFrameworkCore;
using DanceCourseCreator.API.Models;

namespace DanceCourseCreator.API.Data;

public class DanceCourseDbContext : DbContext
{
    public DanceCourseDbContext(DbContextOptions<DanceCourseDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<PatternOrExercise> Patterns { get; set; }
    public DbSet<LessonSection> LessonSections { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<ShareLink> ShareLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Role).HasConversion<string>();
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.Members)
                  .HasForeignKey(e => e.TeamId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Team configuration
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // PatternOrExercise configuration
        modelBuilder.Entity<PatternOrExercise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Level).HasConversion<string>();
            entity.HasOne(e => e.Creator)
                  .WithMany(u => u.CreatedPatterns)
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // LessonSection configuration
        modelBuilder.Entity<LessonSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
        });

        // Lesson configuration
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Creator)
                  .WithMany(u => u.CreatedLessons)
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Course)
                  .WithMany(c => c.Lessons)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Course configuration
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Level).HasConversion<string>();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.HasOne(e => e.Creator)
                  .WithMany(u => u.CreatedCourses)
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Template configuration
        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Scope).HasConversion<string>();
            entity.HasOne(e => e.OwnerUser)
                  .WithMany()
                  .HasForeignKey(e => e.Owner)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.TeamEntity)
                  .WithMany()
                  .HasForeignKey(e => e.Team)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ShareLink configuration
        modelBuilder.Entity<ShareLink>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Visibility).HasConversion<string>();
            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}