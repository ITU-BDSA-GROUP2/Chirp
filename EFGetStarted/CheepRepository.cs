using System;
using System.Linq;

using var db = new CheepContext();
public class CheepRepository
{
    public Cheep add(Cheep cheep)  
    {
        db.Add(cheep);
        db.SaveChanges();
        return cheep
    }
    public List<Cheep> get(int limit)
    {
        var Cheep = from c in db
        orderby c.DateTime descending;
        select new {Cheep = c.Cheep}
       
    }
}

