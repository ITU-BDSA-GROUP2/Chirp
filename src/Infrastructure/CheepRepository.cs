using System;
using System.Linq;



public class CheepRepository : ICheepRepository
{

    private static readonly List<CheepDto> _cheeps = new List<CheepViewModel>();

    CheepContext db = new CheepContext();

    public void Create(Cheep cheep)  
    {

        db.Add(cheep);
        db.SaveChanges();
    }

    public async Task<IEnumerable<CheepDto>> GetCheeps(int page)
    {
        // var Cheep = from c in db
        // orderby c.DateTime descending;
        // select new {Cheep = c.Cheep}
       
        var cheeps = db.Cheeps
        .OrderBy(c => c.Text)
        .ToList();

        foreach (var cheep in cheeps) {
            _cheeps.Add(new CheepViewModel(cheep.CheepId, cheep.Text, cheep.TimeStamp, cheep.AuthorId));
        }
        return _cheeps;
        
    }

    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(int page)
    {
        // var Cheep = from c in db
        // orderby c.DateTime descending;
        // select new {Cheep = c.Cheep}
       
        var cheeps = db.Cheeps
        .OrderByDescending(c => c.TimeStamp)
        .ToAsyncList();

        foreach (var cheep in cheeps) {
            _cheeps.Add(new CheepDto(cheep.Text, cheep.Author, cheep.TimeStamp));
        }
        return _cheeps;
    }
}

public record CheepViewModel(int CheepId, string Text, DateTime TimeStamp, int AuthorId);
