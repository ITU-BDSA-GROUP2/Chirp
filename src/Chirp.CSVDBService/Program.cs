using SimpleDB;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var newCheep = new Cheep("Mads","Dette er en test", 1684229348);
IDatabaseRepository<Cheep> db = CSVDatabase<Cheep>.DBInstance("../../data/chirp_cli_db.csv");


app.MapPost("/cheep/{cheep}", (string cheep) => db.Store(JsonSerializer.Deserialize<Cheep>(cheep)));
app.MapGet("/cheeps/{id}", (int id) => db.Read(id));


app.Run();

public record Cheep(string Author, string Message, long Timestamp);
