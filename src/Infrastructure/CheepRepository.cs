using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;


// <summary>
//   This class represents a repository for 'Cheep'
//   It implements the ICheepRepository interface
//   This repository contains methods for creating and deleting cheeps 
//   and returning cheeps based on different search criteria like 
//   all cheeps from a certain author or all cheeps by a followed author etc. 
// </summary>
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext db;
    private const int pageSize = 32;
    public CheepRepository(ChirpDBContext context)
    {
        db = context;
    }


    public async Task CreateCheep(string text, string authorName, DateTime timestamp)
    {
        var author = await db.Authors
        .Where(a => a.Name == authorName)
        .FirstOrDefaultAsync();

        if (author == null) 
        {
            return;
        }

        var newCheep = new Cheep {
            Text = text,
            TimeStamp = timestamp,
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
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId, c.Likes))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName, int page)
    {
        return await db.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Where(u => u.Author.Name == authorName)
            .Skip(page*pageSize)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId, c.Likes))
            .ToListAsync();
    }

    public async Task<int> GetAllCheeps() 
    {
        return await db.Cheeps
            .CountAsync();
    }

    public async Task<int> GetAllCheepsFromAuthor(string authorName) 
    {
        return await db.Cheeps
            .Where(u => u.Author.Name == authorName)
            .CountAsync();
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
            .Where(u => u.Author.Name == user || followedList.Contains(u.AuthorId))
            .Skip(page*pageSize)
            .Take(pageSize)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId, c.Likes))
            .ToListAsync();
    }

    public async Task<int> GetAllCheepsFromFollowedCount(string user) 
    {

        var userId = await db.Authors
            .Where(a => a.Name == user)
            .Select(a => a.AuthorId).FirstOrDefaultAsync();

        var followedList = await db.Following
            .Where(a => a.UserId == userId)
            .Select(a => a.FollowedAuthorId)
            .ToListAsync();
        return await db.Cheeps
            .Where(u => u.Author.Name == user || followedList.Contains(u.AuthorId))
            .CountAsync();
    }

}