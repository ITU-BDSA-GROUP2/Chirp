namespace SimpleDB;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Expressions;



sealed public class CSVDatabase<T> : IDatabaseRepository<T>
{
Regex rx = new Regex("""(?'author'.+),"(?'message'.+)",(?'timestamp'\d+)""");



public IEnumerable<T> Read(int? limit = null) {
    try {
        using (var sr = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
{
            var records = csv.GetRecords<T>().ToList();
            return records;
}
} catch (IOException e) {
    Console.WriteLine("The file could not be read");
    Console.WriteLine(e.Message);
    return null;
}


}

public void Store(T record) {
    
    using(StreamWriter w = File.AppendText("chirp_cli_db.csv")) 
{       
    var csv = new CsvWriter(w, CultureInfo.InvariantCulture);
    w.WriteLine();
    csv.WriteRecord(record);
    csv.Flush();
}
}

}


