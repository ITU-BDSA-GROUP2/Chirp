using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


public class ChirpDBContext : DbContext
{
    public required DbSet<Cheep> Cheeps { get; set; }
    public required DbSet<Author> Authors { get; set; }
    public string DbPath { get; }


    //public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }

    public ChirpDBContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data source={DbPath}");
}

