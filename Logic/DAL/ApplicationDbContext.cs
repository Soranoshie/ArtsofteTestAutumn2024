﻿using Logic.DAL.Entities;
using Logic.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Logic.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    private readonly Config config;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, Config config) : base(options)
    {
        this.config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql(config.DatabaseConnectionString,
                builder => { builder.
                    EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); });
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // прописать id но сначала затестить уникальность выданного ID вручную
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Phone)
            .IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}