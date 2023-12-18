using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;

// <summary>
//   This class represents a repository for 'Followerlist'
//   It implements the IFollowerlistRepository interface
//   This repository contains methods for following and unfollowing other authors
// </summary>

public class FollowerListRepository : IFollowerListRepository
{
    private readonly ChirpDBContext db;

    public FollowerListRepository(ChirpDBContext context)
    {
        db = context;
    }

    // <summary>
    //   This method checks if both the authorFollows and authorFollowed points to two authors
    //   If they do, it then checks if 'authorFollows' already follows 'authorFollowed' 
    //   if the follow relationship dose not exist already the method creates it
    // </summary>
    public async Task Follow(string authorFollows, string authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
            return;
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed)
        .FirstOrDefaultAsync();

        if (followedUser == null) 
        {
            return;
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

    // <summary>
    //   This method checks if both the authorFollows and authorFollowed points to two authors
    //   If they do, it then checks if 'authorFollows' already follows 'authorFollowed' 
    //   if the follow relationship dose not exist it returns else it removes the relationship
    // </summary>
    public async Task UnFollow(string authorFollows, string authorFollowed)
    {
        var user = await db.Authors
        .Where(a => a.Name == authorFollows)
        .FirstOrDefaultAsync();

        if (user == null) 
        {
           return;
        }

        var followedUser = await db.Authors
        .Where(a => a.Name == authorFollowed)
        .FirstOrDefaultAsync();

        if (followedUser == null) 
        {
            return;
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

    // <summary>
    //   This method checks if authorName points to a author.
    //   If it does not it returns an empty enumerable object.
    //   If it does it returns a list with all the followers of the author
    // </summary>
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