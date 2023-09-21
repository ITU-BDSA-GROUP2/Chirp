using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Expressions;
using SimpleDB;
using DocoptNet;
using System.Collections.Immutable;

const string usage = @"Chirp


Usage:
  
  Chirp.exe read [<number>]
  Chirp.exe cheep <message>
  Chirp.exe (-h | --help)

Options:
  -h --help     Show this screen.
";

var arguments = new Docopt().Apply(usage, args, exit: true)!;
var db = CSVDatabase<Cheep>.DBInstance;

if (arguments["read"].Value is bool read)
{
  var limit = Convert.ToInt32(arguments["<number>"].Value);
  if (read) {
    UserInterface.PrintCheeps(db.Read(limit));
  }
}
if(arguments["cheep"].Value is bool cheepT)
{
  if(cheepT) {
      
      var author = System.Environment.MachineName;
      var message =  arguments["<message>"];
      var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
      var cheep = new Cheep(author,message.ToString(),timestamp); 
      db.Store(cheep);
  }
}



