using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext db;
    private const int pageSize = 32;
    public CheepRepository(ChirpDBContext context)
    {
        db = context;
        DbInitializer.SeedDatabase(context);
    }

    public async Task CreateCheep(CheepDto cheep)  
    {
        db.Cheeps.Add(cheep);
        db.SaveChanges();
    }

    public async Task<IEnumerable<CheepDto>> GetCheeps(int page)
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip(page)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepFromAuthor(string authorName, int page)
    {
        return await db.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.Name == authorName)
        .Skip(page)
        .Take(pageSize)
        .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
        .ToListAsync();
    }
}