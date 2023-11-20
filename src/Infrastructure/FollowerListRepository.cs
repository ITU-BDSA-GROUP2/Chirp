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


    public Task Follow(AuthorDto authorFollows, AuthorDto authorFollowed)
    {
        
        var listOfFollowers = db.FollowerList.Author
        .Where()

        var followList = new FollowerList()
        {

        }

        db.FollowerList.Add(followList);
        await db.SaveChangesAsync();

    }

}