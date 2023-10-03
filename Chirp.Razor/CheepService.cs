using System.Data;
using Microsoft.Data.Sqlite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new List<CheepViewModel>();

    public static void makeList()
    {
        string sqlDBFilePath = "data/chirp.db";
        var sqlQuery = @"SELECT text FROM message";
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read()){
                string message = reader[0].ToString();
               CheepViewModel someCheep = new CheepViewModel("marius",message,UnixTimeStampToDateTimeString(1690892208));
                _cheeps.Add(someCheep);
                
            }
        }
    }

           // new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
           // new CheepViewModel("Rasmus", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        

    public List<CheepViewModel> GetCheeps()
    {
        makeList();
        return _cheeps;
    }
    
    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
