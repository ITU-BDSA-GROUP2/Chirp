using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;



public class FollowerListRepository : IFollowerListRepository
{
    private readonly ChirpDBContext db;

    public FollowerListRepository(ChirpDBContext context)
    {
        db = context;
    }


    public async Task Follow(AuthorDto authorFollows, AuthorDto authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows.Name)
        .FirstOrDefaultAsync();

        if (user == null) {
            throw new ArgumentNullException("No user exists");
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed.Name)
        .FirstOrDefaultAsync();

        if (followedUser == null) {
            throw new ArgumentNullException("No user exists");
        }

        var followList = new FollowerList {
            UserId = user.AuthorId,
            FollowedAuthorId = followedUser.AuthorId,
        };

        db.Following.Add(followList);
        await db.SaveChangesAsync();
    }

    public async Task UnFollow(AuthorDto authorFollows, AuthorDto authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows.Name)
        .FirstOrDefaultAsync();

        if (user == null) {
            throw new ArgumentNullException("No user exists");
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed.Name)
        .FirstOrDefaultAsync();

        if (followedUser == null) {
            throw new ArgumentNullException("No user exists");
        }

        var unfollow = await db.Following
        .Where(a =>
        (a.UserId == user.AuthorId) &&
        (a.FollowedAuthorId == followedUser.AuthorId))
        .FirstOrDefaultAsync();

         if (unfollow != null) {
            db.Following.Remove(unfollow);
        } else {
            throw new ArgumentNullException("No cheep to be removed");
        }
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<FollowDto>> GetFollowers(string authorName)
    {
        int id = await db.Authors.
        Where(c => c.Name == authorName)
        .Select(c => c.AuthorId)
        .FirstOrDefaultAsync();

        return await db.Following
            .Where(u => u.UserId == id)
            .Select(c => new FollowDto(c.UserId, c.FollowedAuthorId))
            .ToListAsync();
    }

}