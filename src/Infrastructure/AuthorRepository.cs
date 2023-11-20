using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext db;
    public AuthorRepository(ChirpDBContext context)
    {
        db = context;
    }

    public async Task CreateNewAuthor(AuthorDto newAuthor)
    {
        var author = new Author {
            Name = newAuthor.Name,
            Email = newAuthor.Email,
            Cheeps = new List<Cheep>(),
            FollowerList = new List<Author>(),
        };
        
        db.Authors.Add(author);
        await db.SaveChangesAsync();
    }

    public async Task<AuthorDto> GetAuthorByName(string authorName)
    {
        var author = await db.Authors
        .Where(u => u.Name == authorName)
        .Select(a => new AuthorDto(a.Name, a.Email))
        .FirstOrDefaultAsync();

        return author;
    }

    public async Task<AuthorDto> GetAuthorByEmail(string authorEmail)
    {
        var author = await db.Authors
        .Where(u => u.Email == authorEmail)
        .Select(a => new AuthorDto(a.Name, a.Email))
        .FirstOrDefaultAsync();
        return author;
    }
}