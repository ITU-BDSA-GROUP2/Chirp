using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext
{
    public DbSet<Cheep> Cheeps { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<FollowerList> Following { get; set; }



    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Author>()
           .HasIndex(c => c.AuthorId)
           .IsUnique();
        modelBuilder.Entity<FollowerList>()
            .HasKey(c => new {c.UserId, c.FollowedAuthorId});
    }
}