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

    public async Task<AuthorDto?> GetAuthorByID(int id)
    {
        var author = await db.Authors
        .Where(u => u.AuthorId == id)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        if (author == null) 
        {
            throw new ArgumentNullException("Author does not exist");
        }

        return author;
    }

    
    public async Task<AuthorDto?> GetAuthorByName(string authorName)
    {
        var author = await db.Authors
        .Where(u => u.Name == authorName)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        return author;
    }

    public async Task<AuthorDto?> GetAuthorByEmail(string authorEmail)
    {
        var author = await db.Authors
        .Where(u => u.Email == authorEmail)
        .Select(a => new AuthorDto(a.Name, a.Email, a.AuthorId))
        .FirstOrDefaultAsync();

        return author;
    }

    public async Task<string> GetAuthorImageUrl(string authorName)
    {
         return await db.Authors
        .Where(u => u.Name == authorName)
        .Select(a => a.ImageUrl)
        .FirstOrDefaultAsync();
    }

    public async Task SetAuthorImageUrl(string authorName, string imageUrl)
    {
         var author = await db.Authors
        .Where(u => u.Name == authorName)
        .FirstOrDefaultAsync();

        if (!await ValidateImageUrl(imageUrl)) 
        {
            return;
        }
        author.ImageUrl = imageUrl;
        await db.SaveChangesAsync();
    }

    public async Task<bool> ValidateImageUrl(string imageUrl) 
    {
        var list = new List<string>();
        list.Add("images/bird1.webp");
        list.Add("images/Bird2.png");
        list.Add("images/Bird3.png");
        list.Add("images/Image4.png");

        return list.Contains(imageUrl);
    }

    public async Task UpdateAuthor(string oldName, string newName, string email) {
        var author = await db.Authors
        .Where(a => a.Name == oldName)
        .FirstOrDefaultAsync();

        if (author == null) 
        {
            return;
        }

        
        author.Name = newName;
        author.Email = email;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAuthor(string name)
    {
        var author = await db.Authors
        .Where(a => a.Name == name)
        .FirstOrDefaultAsync();

        if (author == null) 
        {
            return;
        }

        db.Remove(author);

        await db.SaveChangesAsync();
    }

}