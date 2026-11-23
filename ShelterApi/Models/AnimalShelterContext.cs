using System;
using Microsoft.EntityFrameworkCore;

namespace ShelterApi.Models;

public partial class AnimalshelterContext : DbContext
{
    public AnimalshelterContext()
    {
    }

    public AnimalshelterContext(DbContextOptions<AnimalshelterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Shelter> Shelters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("animals");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasMaxLength(40).HasColumnName("Name");
            entity.Property(e => e.Species).HasMaxLength(30).HasColumnName("Species");
            entity.Property(e => e.Age).HasColumnName("Age");
            entity.Property(e => e.Weight).HasColumnName("Weight");
            entity.Property(e => e.RegTime).HasColumnName("RegTime");
            entity.Property(e => e.ShelterId).HasColumnName("ShelterId");

            entity.HasOne(d => d.Shelter)
                .WithMany(p => p.Animals)
                .HasForeignKey(d => d.ShelterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Shelter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("shelters");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("Name");
            entity.Property(e => e.City).HasMaxLength(40).HasColumnName("City");
            entity.Property(e => e.Capacity).HasColumnName("Capacity");
            entity.Property(e => e.RegTime).HasColumnName("RegTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
