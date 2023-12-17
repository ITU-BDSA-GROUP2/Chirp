using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;

// <summary>
//   This class represents a repository for 'Like'
//   It implements the ILikeRepository interface
//   This repository contains methods for liking and disliking cheeps
// </summary>

public class LikeRepository : ILikeRepository
{
    private readonly ChirpDBContext db;

    public LikeRepository(ChirpDBContext context)
    {
        db = context;
    }

    // <summary>
    //   This method checks if both the CheepID and userName points to a cheep and author respectively
    //   If both exist it either calls 'Dislike()', if the user have liked the cheep before 
    //   or adds a like if the author haven't liked the cheep before.
    // </summary>
    public async Task Like(int CheepId, string userName)
    {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
            return;
        }

        var cheep = await db.Cheeps
        .Where(c => c.CheepId == CheepId)
        .FirstOrDefaultAsync();

        if (cheep == null) 
        {
            return;
        }

        if (await IsLiked(CheepId, userName)) 
        {     
            await Dislike(CheepId, userName);       
            return;
        }

        var like = new Like 
        {
            CheepId = CheepId,
            UserId = user.AuthorId,
        };

        cheep.Likes += 1;
        db.Likes.Add(like);

        await db.SaveChangesAsync();
    }

    // <summary>
    //   This method checks if both the CheepID and userName points to a cheep and author respectively
    //   If both exist it either calls 'Like()', if the user haven't liked the cheep before 
    //   or removes a like if the author have already liked the cheep.
    // </summary>
    public async Task Dislike(int CheepId, string userName)
    {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
            return;
        }

        var cheep = await db.Cheeps
        .Where(c => c.CheepId == CheepId)
        .FirstOrDefaultAsync();

    
        if (cheep == null) 
        {
            return;
        }

        if (!await IsLiked(CheepId, userName)) 
        {
            await Like(CheepId, userName);
            return;
        }

        var like = await db.Likes
        .Where(l => l.UserId == user.AuthorId &&
        l.CheepId == CheepId)
        .FirstOrDefaultAsync();

        cheep.Likes -= 1;
        db.Likes.Remove(like);

        await db.SaveChangesAsync();
    }

    // <summary>
    //   This method checks if the user already likes a cheep
    // </summary>
    public async Task<bool> IsLiked(int CheepId, string userName) 
    {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        var checkIfUserLiked = await db.Likes
        .Where(l => l.UserId == user.AuthorId &&
        l.CheepId == CheepId)
        .FirstOrDefaultAsync();

        return checkIfUserLiked != null;
    }


}