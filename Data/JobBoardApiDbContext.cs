using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using JobBoardApi.Models;
using Microsoft.AspNetCore.Identity;

namespace JobBoardApi.Data;

public class JobBoardApiDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    public DbSet<Industry> Industries { get; set; }
    public DbSet<CompanyJob> CompanyJobs { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<JobApplicant> JobApplicants { get; set; }
    
    public JobBoardApiDbContext(DbContextOptions<JobBoardApiDbContext> context, IConfiguration config) : base(context)
    {
        _configuration = config;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // UserProfile - IdentityUser relationship (one-to-one)
        modelBuilder.Entity<UserProfile>()
            .HasOne(up => up.IdentityUser)
            .WithOne()
            .HasForeignKey<UserProfile>(up => up.IdentityUserId);
            
        // UserProfile - Industry relationship (many-to-one)
        modelBuilder.Entity<UserProfile>()
            .HasOne<Industry>()
            .WithMany()
            .HasForeignKey(up => up.IndustryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // CompanyJob - UserProfile relationship (many-to-one)
        modelBuilder.Entity<CompanyJob>()
            .HasOne<UserProfile>()
            .WithMany()
            .HasForeignKey(cj => cj.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // CompanyJob - Job relationship (many-to-one)
        modelBuilder.Entity<CompanyJob>()
            .HasOne<Job>()
            .WithMany()
            .HasForeignKey(cj => cj.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Applicant - IdentityUser relationship (one-to-one)
        modelBuilder.Entity<Applicant>()
            .HasOne(a => a.IdentityUser)
            .WithOne()
            .HasForeignKey<Applicant>(a => a.IdentityUserId);
        
        // JobApplicant - Job relationship (many-to-one)
        modelBuilder.Entity<JobApplicant>()
            .HasOne<Job>()
            .WithMany()
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // JobApplicant - Applicant relationship (many-to-one)
        modelBuilder.Entity<JobApplicant>()
            .HasOne<Applicant>()
            .WithMany()
            .HasForeignKey(ja => ja.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            Name = "Admin",
            NormalizedName = "admin"
        });
        
        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            UserName = "Administrator",
            Email = "admina@strator.comx",
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, _configuration["AdminPassword"])
        });
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f"
        });
   
        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            Id = 1,
            IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            Name = "mtg mirage",
            Location = "Hopkinsville, KY",
            IndustryId = 1 
        });
        
        modelBuilder.Entity<Industry>().HasData(
            new Industry { Id = 1, Name = "Technology" },
            new Industry { Id = 2, Name = "Healthcare" },
            new Industry { Id = 3, Name = "Finance" },
            new Industry { Id = 4, Name = "Education" },
            new Industry { Id = 5, Name = "Retail" }
        );
       
        modelBuilder.Entity<Job>().HasData(
            new Job
            {
                Id = 1,
                Title = "Senior Software Developer",
                Description = "Experienced developer for complex web applications",
                PostedDate = DateTime.Now.AddDays(-10),
                ClosesDate = DateTime.Now.AddDays(20)
            },
            new Job
            {
                Id = 2,
                Title = "Registered Nurse",
                Description = "Skilled nurse for patient care and support",
                PostedDate = DateTime.Now.AddDays(-15),
                ClosesDate = DateTime.Now.AddDays(15)
            },
            new Job
            {
                Id = 3,
                Title = "Financial Analyst",
                Description = "Analyze financial data and prepare reports",
                PostedDate = DateTime.Now.AddDays(-7),
                ClosesDate = DateTime.Now.AddDays(23)
            },
            new Job
            {
                Id = 4,
                Title = "Junior Web Developer",
                Description = "Entry-level developer for website maintenance",
                PostedDate = DateTime.Now.AddDays(-5),
                ClosesDate = DateTime.Now.AddDays(25)
            }
        );
       
        modelBuilder.Entity<CompanyJob>().HasData(
            new CompanyJob { Id = 1, CompanyId = 1, JobId = 1 },
            new CompanyJob { Id = 2, CompanyId = 1, JobId = 2 },
            new CompanyJob { Id = 3, CompanyId = 1, JobId = 3 },
            new CompanyJob { Id = 4, CompanyId = 1, JobId = 4 }
        );
    }
}