using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class EventPlannerContext : DbContext
{
    public EventPlannerContext()
    {
    }

    public EventPlannerContext(DbContextOptions<EventPlannerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendEvent> AttendEvents { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(@"Server=.;Database=EventPlanner;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendEvent>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EventId }).HasName("PK__AttendEv__001C802B9DD5C702");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.AttendedDate).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Event).WithMany(p => p.AttendEvents)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AttendEve__Event__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.AttendEvents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AttendEve__UserI__5629CD9C");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C87085495635");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Picture)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Events)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__UserID__52593CB8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8745720A");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E443CC8A4E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534821C218E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('guest')");
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
