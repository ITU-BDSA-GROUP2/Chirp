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

    public Task CreateNewAuthor(AuthorDto newAuthor) 
    {   
        db.Author.Add(newAuthor);
        db.SaveChanges();
    }

    public async Task GetAuthorByName(string authorName)
    {
        return await db.Author
        .Where(u => u.Name == authorName)
        .Select(a => new AuthorDto(a.AuthorId, a.Name, a.Email));
    }
    
    public Task GetAuthorByEmail(string authorEmail)
    {
        return await db.Author
        .Where(u => u.Email == authorEmail)
        .Select(a => new AuthorDto(a.AuthorId, a.Name, a.Email));
    }
}