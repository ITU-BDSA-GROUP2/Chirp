using System.Data;
using Microsoft.Data.Sqlite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new List<CheepViewModel>();

    public List<CheepViewModel> GetCheeps(int page)
    {
        int numberOfCheeps = (page == 1 ? 0 : 32 * (page-1));
        var sqlQuery =
        $@"SELECT M.text, U.username, M.pub_date FROM message M
        JOIN user U ON M.author_id = U.user_id
        ORDER BY M.pub_date DESC
        LIMIT 32
        OFFSET {numberOfCheeps};";
        
        return Query(sqlQuery);
    }
    
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        author = $"\"{author}\"";
        // filter by the provided author name
        int numberOfCheeps = (page == 1 ? 0 : 32 * (page-1));
        var sqlQuery =
        $@"SELECT M.text, U.username, M.pub_date FROM message M
        JOIN user U ON M.author_id = U.user_id
        WHERE U.username = {author}
        ORDER BY M.pub_date DESC
        LIMIT 32
        OFFSET {numberOfCheeps};";

        return Query(sqlQuery);

    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


    public List<CheepViewModel> Query(string query) {

        _cheeps.Clear();
        string value = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        string tempPath;
        string chirpPath = "./data/chirp.db";

        if (value == null)
        {
            tempPath = Path.GetTempPath();
            string tmp = tempPath + "chirp.db";
            Environment.SetEnvironmentVariable("CHIRPDBPATH", tmp);
            value = Environment.GetEnvironmentVariable("CHIRPDBPATH");
            File.Copy(chirpPath, tmp, true);
        }

        using (var connection = new SqliteConnection($"Data Source={value}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();
            while (reader.Read()){
                string message = reader[0].ToString();
                string author = reader[1].ToString();
                long timestamp = Convert.ToInt64(reader[2]);
               CheepViewModel someCheep = new CheepViewModel(author,message,UnixTimeStampToDateTimeString(timestamp));
                _cheeps.Add(someCheep); 
            }
        }
        return _cheeps;
    }

}
