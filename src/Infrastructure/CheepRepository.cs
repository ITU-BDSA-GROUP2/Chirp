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

    public async Task CreateCheep(CheepDto cheep)
    {
        var author = await db.Authors
        .Where(a => a.Name == cheep.Author)
        .FirstOrDefaultAsync();

        if (author == null) 
        {
            throw new ArgumentNullException("Author doesn't exist");
        }
        var newCheep = new Cheep {
            Text = cheep.Text,
            TimeStamp = cheep.Timestamp,
            AuthorId = author.AuthorId,
            Author = author,
        };
        db.Cheeps.Add(newCheep);
        await db.SaveChangesAsync();
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

    public async Task<IEnumerable<CheepDto>> GetAllCheeps() 
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetAllCheepsFromAuthor(string authorName) 
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == authorName)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetAllCheepsFromFollowed(string user, int page) 
    {

        var userId = await db.Authors
            .Where(a => a.Name == user)
            .Select(a => a.AuthorId).FirstOrDefaultAsync();

        var followedList = await db.Following
            .Where(a => a.UserId == userId)
            .Select(a => a.FollowedAuthorId)
            .ToListAsync();
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == user || followedList.Contains(u.AuthorId))
            .Skip(page*pageSize)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetAllCheepsFromFollowedCount(string user) 
    {

        var userId = await db.Authors
            .Where(a => a.Name == user)
            .Select(a => a.AuthorId).FirstOrDefaultAsync();

        var followedList = await db.Following
            .Where(a => a.UserId == userId)
            .Select(a => a.FollowedAuthorId)
            .ToListAsync();
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == user || followedList.Contains(u.AuthorId))
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

}