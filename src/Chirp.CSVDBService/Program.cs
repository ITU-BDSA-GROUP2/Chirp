using SimpleDB;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> db = CSVDatabase<Cheep>.DBInstance("chirp_cli_db.csv");



app.MapPost("/cheep", (Cheep cheep) => db.Store(cheep));
app.MapGet("/cheeps", () => db.Read());
app.MapGet("/", () => "Hello world");


app.Run();

public record Cheep(string Author, string Message, long Timestamp);
