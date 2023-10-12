using System;
using System.Linq;

public class CheepRepository : ICheepRepository
{
    CheepContext db = new CheepContext();

    public async Task CreateCheep(CheepDto cheep)  
    {

        db.Add(cheep);
        db.SaveChanges();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepFromPage(int page)
    {
        // var Cheep = from c in db
        // orderby c.DateTime descending;
        // select new {Cheep = c.Cheep}
       
        var cheeps = await db.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .ToAsyncList();

        return cheeps;
        
    }

    public async Task<IEnumerable<CheepDto>> GetCheepFromAuthor(string author, int? page)
    {
        // var Cheep = from c in db
        // orderby c.DateTime descending;
        // select new {Cheep = c.Cheep}
       
        var cheeps = await db.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .ToAsyncList();

        return cheeps;
    }
}