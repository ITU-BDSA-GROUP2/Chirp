using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Expressions;
using SimpleDB;

string command = args[0];

var db = new CSVDatabase<Cheep>();

string epoch2String(int epoch) {
return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToString(); }
    

if (command == "read") {
    var records = db.Read();
    foreach (var record in records) {
            string time = epoch2String(int.Parse(record.Timestamp.ToString()));
            Console.WriteLine($"{record.Author} @ " + time + ": " + $"{record.Message}");
    }
} else if(command == "cheep") {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
    
    var author = System.Environment.MachineName;
    var message =  args[1] ;
    var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
    var cheep = new Cheep(author,message,timestamp); 
    db.Store(cheep);
}



