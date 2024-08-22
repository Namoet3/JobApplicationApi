using Microsoft.EntityFrameworkCore;
using JobApplicationApi.Models;

namespace JobApplicationApi.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<JobExperience> JobExperiences { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<SkillCertificate> SkillCertificates { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Language> Languages { get; set; }
        // public DbSet<ApplicationStage> ApplicationStages { get; set; }
        public DbSet<DeletedUser> DeletedUsers { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<PageVisitLog> PageVisitLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserDetails>().ToTable("UserDetails");
            modelBuilder.Entity<JobExperience>().ToTable("JobExperiences");
            modelBuilder.Entity<Reference>().ToTable("References");
            modelBuilder.Entity<SkillCertificate>().ToTable("SkillCertificates");
            modelBuilder.Entity<Membership>().ToTable("Memberships");
            modelBuilder.Entity<Language>().ToTable("Languages");
            // modelBuilder.Entity<ApplicationStage>().ToTable("ApplicationStages");

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDetails)
                .WithOne()
                .HasForeignKey<User>(u => u.UserDetailsId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.JobExperiences)
                .WithOne()
                .HasForeignKey(j => j.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.References)
                .WithOne()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SkillCertificates)
                .WithOne()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Memberships)
                .WithOne()
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Languages)
                .WithOne()
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<JobExperience>()
                .Property(e => e.StartDate)
                .HasConversion(
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                    v => v
                );

            modelBuilder.Entity<JobExperience>()
                .Property(e => e.EndDate)
                .HasConversion(
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                    v => v
                );

            modelBuilder.Entity<User>()
                .Property(u => u.GraduateDate)
                .HasConversion(
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                    v => v
                );

            modelBuilder.Entity<User>()
                .Property(u => u.MilitaryDate)
                .HasConversion(
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                    v => v
                );

            modelBuilder.Entity<User>()
                .Property(u => u.CriminalDate)
                .HasConversion(
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                    v => v
                );
        }
    }
}
