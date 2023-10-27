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

    public void CreateNewAuthor(AuthorDto newAuthor)
    {
        var author = new Author {
            Name = newAuthor.Name,
            Email = newAuthor.Email,
        };
        
        
        db.Authors.Add(author);
        db.SaveChanges();
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