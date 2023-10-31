using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext<ApplicationUser>
{
    [Required]
    public DbSet<Cheep> Cheeps { get; set; }

    [Required]
    public DbSet<Author> Authors { get; set; }

    public string DbPath { get; }


    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
           .HasIndex(c => c.AuthorId)
           .IsUnique();
    }
}