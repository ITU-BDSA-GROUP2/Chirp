namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Expressions;
using System.IO;
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        try {
            using (var sr = new StreamReader("chirp_cli_db.csv"))
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Cheep>();
                return (IEnumerable<T>)records;
                /*foreach (var record in records) {
                    string time = epoch2String(int.Parse(record.Timestamp.ToString()));
                    Console.WriteLine($"{record.Author} @ " + time + ": " + $"{record.Message}");
                }*/
            }
            } catch (IOException e) {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
                return null;
            }
        

    
    }

    public void Store(T record, string message)
    {
        using(StreamWriter w = File.AppendText("chirp_cli_db.csv")) 
        {       
            w.WriteLine();
            var csv = new CsvWriter(w, CultureInfo.InvariantCulture);
            var author = System.Environment.MachineName;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
            var line = string.Format("{0},{1},{2}", author, message,timestamp);
            var cheep = new Cheep(author,message,timestamp); 
            csv.WriteRecord(cheep);
            csv.Flush();
        }
    }

    public void Store(T record)
    {
        throw new NotImplementedException();
    }
}
