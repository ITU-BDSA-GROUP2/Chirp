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


    public async Task Follow(string authorFollows, string authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
            throw new ArgumentNullException("No user logged in");
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed)
        .FirstOrDefaultAsync();

        if (followedUser == null) 
        {
            throw new ArgumentNullException("User tried to follow does not exist");
        }

        var followList = new FollowerList {
            UserId = user.AuthorId,
            FollowedAuthorId = followedUser.AuthorId,
        };

        if (await db.Following
        .Where(f => (f.UserId == followList.UserId) && (f.FollowedAuthorId == followList.FollowedAuthorId))
        .FirstOrDefaultAsync() != null) return;

        db.Following.Add(followList);
        await db.SaveChangesAsync();
    }

    public async Task UnFollow(string authorFollows, string authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
            throw new ArgumentNullException("No user logged in");
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed)
        .FirstOrDefaultAsync();

        if (followedUser == null) 
        {
            throw new ArgumentNullException("User tried to follow does not exist");
        }

        var unfollow = await db.Following
        .Where(a =>
        (a.UserId == user.AuthorId) &&
        (a.FollowedAuthorId == followedUser.AuthorId))
        .FirstOrDefaultAsync();

        if (unfollow != null) 
        {
            db.Following.Remove(unfollow);
        }
        else 
        {
            return;
        }
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<FollowDto>> GetFollowers(string authorName)
    {
        var user = await db.Authors
        .Where(c => c.Name == authorName)
        .FirstOrDefaultAsync();

        if (user == null)
        {
            return Enumerable.Empty<FollowDto>();
        }

        return await db.Following
            .Where(u => u.UserId == user.AuthorId)
            .Select(c => new FollowDto(c.UserId, c.FollowedAuthorId))
            .ToListAsync();
    }

}