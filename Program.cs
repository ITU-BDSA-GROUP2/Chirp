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


if (command == "read") {
    UserInterface.PrintCheeps(db.Read());
} else if(command == "cheep") {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
    
    var author = System.Environment.MachineName;
    var message =  args[1] ;
    var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
    var cheep = new Cheep(author,message,timestamp); 
    db.Store(cheep);
}



