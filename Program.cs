using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Expressions;

string command = args[0];



var pattern = """(?'author'.+),"(?'message'.+)",(?'timestamp'\d+)""";
Regex rx = new Regex(pattern);

string epoch2String(int epoch) {
return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToString(); }
    

if (command == "read") {
try {
        using (var sr = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
{
            var records = csv.GetRecords<Cheep>();

            foreach (var record in records) {
                string time = epoch2String(int.Parse(record.Timestamp.ToString()));
                Console.WriteLine($"{record.Author} @ " + time + ": " + $"{record.Message}");
            }
}


} catch (IOException e) {
    Console.WriteLine("The file could not be read");
    Console.WriteLine(e.Message);
}
} else if(command == "cheep") {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp

using(StreamWriter w = File.AppendText("chirp_cli_db.csv")) 
{       w.WriteLine();
        var csv = new CsvWriter(w, CultureInfo.InvariantCulture);
        var author = System.Environment.MachineName;
        var message =  args[1] ;
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        var line = string.Format("{0},{1},{2}", author, message,timestamp);
        var cheep = new Cheep(author,message,timestamp); 
        csv.WriteRecord(cheep);
        csv.Flush();
}
}
public record Cheep(string Author, string Message, long Timestamp);


