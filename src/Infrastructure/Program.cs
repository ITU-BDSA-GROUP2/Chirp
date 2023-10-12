// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;
using System.Globalization;

public partial class Program
{

    private static readonly List<CheepViewModel> _cheeps = new List<CheepViewModel>();

    public static void Main(string[] args)
    {
        var cr = new CheepRepository();

        DateTime time = DateTime.Now;

        var a10 = new Author() { AuthorId = 10, Name = "Jacqualine Gilcoine", Email = "Jacqualine.Gilcoine@gmail.com" };

        var cheep = new Cheep() {
            CheepId = 1,
            AuthorId = 1,
            Author = a10,
            TimeStamp = DateTime.Now,
            Text = "Hej"
        };

        

        cr.add(cheep);
        var alist = cr.get(0);

        Console.WriteLine(alist.Count);
        
    }
}
