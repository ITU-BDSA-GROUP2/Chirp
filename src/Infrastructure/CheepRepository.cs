using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext db;

    public CheepRepository(ChirpDBContext context)
    {
        db = context;
        DbInitializer.SeedDatabase(context);
    }


    public async Task<IEnumerable<CheepDto>> GetCheeps(int offset)
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip(offset)
            .Take(32)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }


    /*public async Task CreateCheep(CheepDto cheep)  
    {
        db.Add(cheep);
        db.SaveChanges();
    }*/

    public async Task<IEnumerable<CheepDto>> GetCheepFromAuthor(string user, int offset)
    {
        return await db.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .Where(u => u.Author.Name == user)
        .Skip(offset)
        .Take(32)
        .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
        .ToListAsync();
    }
}