using SimpleDB;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//var newCheep = new Cheep("Mads","Dette er en test", 1684229348);
IDatabaseRepository<Cheep> db = CSVDatabase<Cheep>.DBInstance("./chirp_cli_db.csv");


app.MapPost("/cheep", (Cheep cheep) => db.Store(cheep));
app.MapGet("/cheeps", () => db.Read());


app.Run();

public record Cheep(string Author, string Message, long Timestamp);
