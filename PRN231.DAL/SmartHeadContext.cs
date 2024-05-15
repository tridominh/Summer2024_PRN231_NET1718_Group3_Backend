using System;
using System.Collections.Generic;
using PRN231.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PRN231.DAL;

public partial class SmartHeadContext :IdentityDbContext<User, Role, int>
{
    public SmartHeadContext()
    {
    }

    public SmartHeadContext(DbContextOptions<SmartHeadContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        optionsBuilder.UseSqlServer("Data Source=(local);Database=SmartHead;User ID=sa;Password=1234567890;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Student)
            .WithMany(x => x.StudentFeedbacks)
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // Use the appropriate delete behavior for your scenario

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Tutor)
            .WithMany(x => x.TutorFeedbacks)
            .HasForeignKey(f => f.TutorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Service>()
            .HasOne(f => f.Subject)
            .WithMany(x => x.Services)
            .HasForeignKey(f => f.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Order>()
            .HasOne(f => f.Service)
            .WithMany(x => x.Orders)
            .HasForeignKey(f => f.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);

        /*modelBuilder.Entity<User>()
            .HasMany(f => f.StudentFeedbacks)
            .WithOne()
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // Use the appropriate delete behavior for your scenario

        modelBuilder.Entity<User>()
            .HasMany(f => f.TutorFeedbacks)
            .WithOne()
            .HasForeignKey(f => f.TutorId)
            .OnDelete(DeleteBehavior.NoAction);*/
        
       
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
