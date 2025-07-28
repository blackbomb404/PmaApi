using Microsoft.EntityFrameworkCore;
using PmaApi.Models.Domain;
using Task = PmaApi.Models.Domain.Task;

namespace Pma.Context;

public class PmaContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure the inheritance hierarchy for AttachableEntity
        modelBuilder.Entity<AttachableEntity<long>>()
            .UseTphMappingStrategy();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Projects)
            .WithMany(p => p.Members)
            .UsingEntity<UserProject>(
                j => j
                    .HasOne(up => up.Project)
                    .WithMany(p => p.UserProjects)
                    .HasForeignKey(up => up.ProjectId),

                j => j
                    .HasOne(up => up.User)
                    .WithMany(u => u.UserProjects)
                    .HasForeignKey(up => up.UserId),
                j =>
                {
                    j.HasKey(up => new { up.ProjectId, up.UserId });
                    j.ToTable("user_project");
                }
            );
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithMany(t => t.Members)
            .UsingEntity(
                "user_task",
                l => l.HasOne(typeof(User)).WithMany().HasForeignKey("user_id"),
                r => r.HasOne(typeof(Task)).WithMany().HasForeignKey("task_id")
                );
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(
                "role_permission",
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("role_id"),
                r => r.HasOne(typeof(Permission)).WithMany().HasForeignKey("permission_id")
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
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Add the unique contraints and others (if needed)
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}