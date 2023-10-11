using System;
using System.Linq;



public class CheepRepository
{

    private static readonly List<CheepViewModel> _cheeps = new List<CheepViewModel>();

    CheepContext db = new CheepContext();

    public void add(Cheep cheep)  
    {

        db.Add(cheep);
        db.SaveChanges();
    }
    public List<CheepViewModel> get(int limit)
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
}

public record CheepViewModel(int CheepId, string Text, DateTime TimeStamp, int AuthorId);
