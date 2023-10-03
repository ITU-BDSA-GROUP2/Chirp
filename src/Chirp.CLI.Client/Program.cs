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
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

var baseURL = "https://bdsagroup02chirpremotedb.azurewebsites.net";
//var baseURL = "http://localhost:5002";
System.Net.Http.HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

const string usage = @"Chirp


Usage:
  Chirp.exe read [<number>]
  Chirp.exe cheep <message>
  Chirp.exe (-h | --help)

Options:
  -h --help     Show this screen.
";


var arguments = new Docopt().Apply(usage, args, exit: true)!;

if (arguments["read"].Value is bool read)
{ 
  var temp = await client.GetStringAsync("/cheeps");
}
if(arguments["cheep"].Value is bool cheepT)
{
  if(cheepT) {
      
      var author = System.Environment.MachineName;
      var message =  arguments["<message>"];
      var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
      var cheep = new Cheep(author,message.ToString(),timestamp);
      var response = await client.PostAsJsonAsync("/cheep", cheep);
      Console.WriteLine(response);
  }
}
