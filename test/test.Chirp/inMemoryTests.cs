using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Chirp.Razor;
using Infrastructure;
namespace test.Chirp;

public class InMemoryTests : IDisposable {
    private readonly DbContextOptions<ChirpDBContext> builder;
    private readonly SqliteConnection connection;
    private readonly ChirpDBContext context;
    private readonly AuthorRepository aController;
    private readonly CheepRepository cController;

    public InMemoryTests()
    {


        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        builder = new DbContextOptionsBuilder<ChirpDBContext>()
        .UseSqlite(connection)
        .Options;

        context = new ChirpDBContext(builder);
        
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        //Add data to database
        SeedDatabase();

        aController = new AuthorRepository(context);

        cController = new CheepRepository(context);

    }

    public void Dispose()
    {
        context.Database.EnsureDeletedAsync();
        context.Dispose();
    }


    [Fact]
    public async void GetAuthorByName()
    {

        //Arrange
        var authorName = "Voldemort";

        //Act
        var author = await aController.GetAuthorByName(authorName);

        //Assert
        Assert.NotNull(author);
        Assert.Equal(authorName, author.Name);
    }

    [Fact]
    public async void GetAuthorByNameExpectedNull()
    {
        //Arrange
        var authorName = "Harry Potter";

        //Act
        var author = await aController.GetAuthorByName(authorName);

        //Assert
        Assert.Null(author);
    }

    [Fact]
    public async void GetAuthorByEmail()
    {
        //Arrange
        var authorEmail = "Svanhildur.Jørgensen@yahoo.com";

        //Act
        var author = await aController.GetAuthorByEmail(authorEmail);

        //Assert
        Assert.NotNull(author);
        Assert.Equal(authorEmail, author.Email);
    }

    [Fact]
    public async void GetAuthorByEmailExpectedNull()
    {
        //Arrange
        var authorEmail = "VoldARMAN@jubiiiiii.com";

        //Act
        var author = await aController.GetAuthorByEmail(authorEmail);

        //Assert
        Assert.Null(author);
    }
        


     [Fact]
     public async void CreateNewAuthor()
     {
        //Arrange
        var authorDto = new AuthorDto("Asger", "Asger@asd.com");

        //Act
         aController.CreateNewAuthor(authorDto);
         var author = await aController.GetAuthorByName(authorDto.Name);

        //Assert
         Assert.NotNull(author);
         Assert.Equal(authorDto.Name, author.Name);
     }


    //  [Fact]
    //  public async void GetAllCheeps()
    //  {
    //     //Arrange
    //     var listOfCheeps = new List<CheepDto>();

    //     //Act
    //     listOfCheeps.Add(await cController.GetAllCheeps());
        

    //     //Assert
    //     Assert.Equal(3, listOfCheeps.Count);
    //  }

     
     
     public void SeedDatabase() {
        var a1 = new Author() { AuthorId = 1, Name = "Voldemort", Email = "Voldemanden@gmail.com", Cheeps = new List<Cheep>() };
        var a2 = new Author() { AuthorId = 2, Name = "Svanhildur", Email = "Svanhildur.Jørgensen@yahoo.com", Cheeps = new List<Cheep>() };
        
        var authors = new List<Author> { a1, a2 };

        var c1 = new Cheep() { CheepId = 1, AuthorId = a1.AuthorId, Author = a1, Text = "Harry Potter suckz!!", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 2, AuthorId = a1.AuthorId, Author = a1, Text = "HAHAAHHAHA I KILLED HARRY POTTER!!!", TimeStamp = DateTime.Parse("2023-08-01 13:14:45") };

        var c3 = new Cheep() { CheepId = 3, AuthorId = a2.AuthorId, Author = a2, Text = "I hate my name. How do I change this?", TimeStamp = DateTime.Parse("2023-06-01 09:10:45") };


        var cheeps = new List<Cheep> { c1, c2, c3 };

        a1.Cheeps = new List<Cheep>() { c1, c2 };
        a2.Cheeps = new List<Cheep>() { c3 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        context.SaveChanges();
     }


}