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
    }

    public void CreateCheep(CheepDto cheep)
    {
        db.Add(cheep);
        db.SaveChanges();
    }

    public async Task<IEnumerable<CheepDto>> GetCheeps(int page)
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip(page*pageSize)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName, int page)
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == authorName)
            .Skip(page*pageSize)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetAllCheeps() {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetAllCheepsFromAuthor(string authorName) {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == authorName)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }
}