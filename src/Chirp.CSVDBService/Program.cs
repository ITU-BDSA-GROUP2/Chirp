using SimpleDB;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var newCheep = new Cheep("Mads","Dette er en test", 1684229348);
IDatabaseRepository<Cheep> db = CSVDatabase<Cheep>.DBInstance("../../data/chirp_cli_db.csv");


app.MapGet("/cheep", () => db.Store(newCheep));
app.MapGet("/cheeps", () => db.Read());

app.Run();

public record Cheep(string Author, string Message, long Timestamp);
