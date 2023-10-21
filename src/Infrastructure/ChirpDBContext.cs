using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public string DbPath { get; }

    
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }

}

