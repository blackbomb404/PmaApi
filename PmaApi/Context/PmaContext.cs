using Microsoft.EntityFrameworkCore;
using PmaApi.Models.Domain;
using Task = PmaApi.Models.Domain.Task;

namespace Pma.Context;

public class PmaContext(DbContextOptions<PmaContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<JobRole> JobRoles { get; set; }
    public DbSet<AccessRole> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure the inheritance hierarchy for AttachableEntity
        modelBuilder.Entity<AttachableEntity>()
            .UseTphMappingStrategy();
        
        modelBuilder.Entity<AccessRole>()
            .ToTable("access_roles");
        modelBuilder.Entity<JobRole>()
            .ToTable("job_roles");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Projects)
            .WithMany(p => p.Members)
            .UsingEntity(
                "user_projects",
                r => r.HasOne(typeof(Project)).WithMany().HasForeignKey("project_id"),
                l => l.HasOne(typeof(User)).WithMany().HasForeignKey("user_id")
            );
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithMany(t => t.Members)
            .UsingEntity(
                "user_tasks",
                r => r.HasOne(typeof(Task)).WithMany().HasForeignKey("task_id"),
                l => l.HasOne(typeof(User)).WithMany().HasForeignKey("user_id")
                );
        modelBuilder.Entity<User>()
            .HasOne(u => u.JobRole)
            .WithMany()
            .HasForeignKey(u => u.JobRoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>()
            .HasOne(u => u.AccessRole)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<AccessRole>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.AcessRoles)
            .UsingEntity(
                "role_permissions",
                r => r.HasOne(typeof(Permission)).WithMany().HasForeignKey("permission_id"),
                l => l.HasOne(typeof(AccessRole)).WithMany().HasForeignKey("role_id")
                );
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.AttachableEntity)
            .WithMany(ae => ae.Attachments)
            .HasForeignKey(a => a.AttachableEntityId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Add unique contraints and others (if needed)
        modelBuilder.Entity<AccessRole>()
            .HasIndex(r => r.Name)
            .IsUnique();
        modelBuilder.Entity<JobRole>()
            .HasIndex(r => r.Name)
            .IsUnique();
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}