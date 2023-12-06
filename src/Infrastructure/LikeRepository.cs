using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;



public class LikeRepository : ILikeRepository
{
    private readonly ChirpDBContext db;

    public LikeRepository(ChirpDBContext context)
    {
        db = context;
    }


    public async Task Like(int id, string userName)
    {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        if (user == null) {
            return;
        }
        
        var checkIfUserLiked = await db.Likes
        .Where(l => l.UserId == user.AuthorId &&
        l.CheepId == id)
        .FirstOrDefaultAsync();

        var cheep = await db.Cheeps
        .Where(c => c.CheepId == id)
        .FirstOrDefaultAsync();

    
        if (cheep == null) {
            return;
        }

        if (await IsLiked(id, userName)) {     
            await Dislike(id, userName);       
            return;
        }

        var like = new Like {
            CheepId = id,
            UserId = user.AuthorId,
        };
        cheep.Likes += 1;
        db.Likes.Add(like);

        await db.SaveChangesAsync();
    }

    public async Task Dislike(int id, string userName)
    {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        if (user == null) {
            return;
        }

        var cheep = await db.Cheeps
        .Where(c => c.CheepId == id)
        .FirstOrDefaultAsync();

    
        if (cheep == null) {
            return;
        }

        if (!await IsLiked(id, userName)) {
            await Like(id, userName);
            return;
        }

        var like = await db.Likes
        .Where(l => l.UserId == user.AuthorId &&
        l.CheepId == id)
        .FirstOrDefaultAsync();

        cheep.Likes -= 1;
        db.Likes.Remove(like);

        await db.SaveChangesAsync();
    }

    public async Task<bool> IsLiked(int id, string userName) {
        var user = await db.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        var checkIfUserLiked = await db.Likes
        .Where(l => l.UserId == user.AuthorId &&
        l.CheepId == id)
        .FirstOrDefaultAsync();

        return checkIfUserLiked != null;
    }


}