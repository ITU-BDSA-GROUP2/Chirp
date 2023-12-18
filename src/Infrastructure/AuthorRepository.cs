using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

// <summary>
//   This class represents a repository for 'Author'.
//   It implements the IAuthorRepository interface.
//   This repository contains methods for creating and deleting authors. 
//   Furthermore it have multiple methods for returning an AuthorDTO object with different parameters
//   And lastly some methods for updating the author information and image.
// </summary>

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
        var imageURL = await db.Authors
        .Where(u => u.Name == authorName)
        .Select(a => a.ImageUrl)
        .FirstOrDefaultAsync();

        if (imageURL == null) 
        {
            imageURL = "images/bird1.webp";
        }

        return imageURL;
    }

    public async Task SetAuthorImageUrl(string authorName, string imageUrl)
    {
         var author = await db.Authors
        .Where(u => u.Name == authorName)
        .FirstOrDefaultAsync();

        //This should not be possible
        if (author == null) {
            return;
        }
        //This should not be possible
        if (!ValidateImageUrl(imageUrl)) 
        {
            return;
        }
        author.ImageUrl = imageUrl;
        await db.SaveChangesAsync();
    }

    public bool ValidateImageUrl(string imageUrl) 
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