using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure;



public class FollowerList : IFollowerListRepository
{
    private readonly ChirpDBContext db;

    public FollowerList(ChirpDBContext context)
    {
        db = context;
    }


    public async Task Follow(AuthorDto authorFollows, AuthorDto authorFollowed)
    {
        var user = await db.Author
        .Where(a => a.Name == authorFollows.Name)
        .FirstOrDefaultAsync();

        var followedUser = await db.Author
        .Where(a => a.Name == authorFollowed.Name)
        .FirstOrDefaultAsync();

        var followList = new FollowerList()
        {
            AuthorID = user.AuthorID,
            Author = user,
            FollowedAuthorID = followedUser.AuthorID,
            FollowedAuthor = followedUser,   
        }

        db.FollowerList.Add(followList);
        await db.SaveChangesAsync();
    }

    public async Task UnFollow(AuthorDto authorFollows, AuthorDto authorFollowed)
    {
        var user = await db.Author
        .Where(a => a.Name == authorFollows.Name)
        .FirstOrDefaultAsync();

        var followedUser = await db.Author
        .Where(a => a.Name == authorFollowed.Name)
        .FirstOrDefaultAsync();

        var unfollow = await db.FollowerList
        .Where(a =>
        (a.AuthorID == user.AuthorID) &&
        (a.FollowedAuthorID == followedUser.AuthorID)
        )

         if (unfollow != null) {
            db.FollowerList.Remove(unfollow);
        } else {
            throw new ArgumentNullException("No cheep to be removed");
        }
        await db.SaveChangesAsync();
    }

}