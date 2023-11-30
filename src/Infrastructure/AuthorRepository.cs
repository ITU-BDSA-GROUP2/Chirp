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

    public async Task CreateNewAuthor(string name, string email)
    {
        var author = new Author {
            Name =  name,
            Email = email,
            Cheeps = new List<Cheep>(),
        };
        
        db.Authors.Add(author);
        await db.SaveChangesAsync();
    }

    public async Task<AuthorDto> GetAuthorByID(int id)
    {
        var author = await db.Authors
        .Where(u => u.AuthorId == id)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        if (author == null) {
            throw new ArgumentNullException("Author does not exist");
        }

        return author;
    }

    public async Task<AuthorDto> GetAuthorByName(string authorName)
    {
        var author = await db.Authors
        .Where(u => u.Name == authorName)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        if (author == null) {
            throw new ArgumentNullException("Author does not exist");
        }

        return author;
    }

    public async Task<AuthorDto> GetAuthorByEmail(string authorEmail)
    {
        var author = await db.Authors
        .Where(u => u.Email == authorEmail)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        if (author == null) {
            throw new ArgumentNullException("Author does not exist");
        }

        return author;
    }

    public async Task UpdateAuthor(string oldName, string newName, string email) {
        var author = await db.Authors
        .Where(a => a.Name == oldName)
        .FirstOrDefaultAsync();

        if (author == null) {
            throw new ArgumentNullException("Error! Could not find user");
        }
        author.Name = newName;
        author.Email = email;
        await db.SaveChangesAsync();
    }

    public async Task DeleteUser(string name)
    {

    }

}