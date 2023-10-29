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

    //AuthorRepository test methods
    [Fact]
     public async void CreateNewAuthor()
     {
        //Arrange
        var authorDto = new AuthorDto("Asger", "Asger@asd.com");

        //Act
         aController.CreateNewAuthor(authorDto);
         var author = await context.Authors
         .Where(a => a.Name == authorDto.Name)
         .FirstOrDefaultAsync();

        //Assert
         Assert.NotNull(author);
         Assert.Equal(authorDto.Name, author.Name);
     }


    [Fact]
    public async void GetAuthorByNameThatExist()
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
    public async void GetAuthorByNameThatDoesNotExist()
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
    public async void GetAuthorByEmailThatDoesNotExist()
    {
        //Arrange
        var authorEmail = "VoldARMAN@jubiiiiii.com";

        //Act
        var author = await aController.GetAuthorByEmail(authorEmail);

        //Assert
        Assert.Null(author);
    }
        


     
    //CheepRepository test methods
     [Fact]
     public async void CreateCheep() {
        //Arrange
        var cheepDto = new CheepDto("Hello my friends! :)", "Voldemort", DateTime.Now);

        //Act
        cController.CreateCheep(cheepDto);
        var cheep = await context.Cheeps
        .Where(c => c.Text == cheepDto.Text)
        .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(cheep);
        Assert.Equal(cheepDto.Text, cheep.Text);
        Assert.Equal(cheepDto.Author, cheep.Author.Name);
        Assert.Equal(cheepDto.Timestamp, cheep.TimeStamp);
     }



    [Fact]
    public async void GetCheepsFromAuthor() 
    {
        //Arrange
        var authorName = "Voldemort";
        int cheepPage = 0;

        //Act
        var allCheeps = await cController.GetCheepsFromAuthor(authorName,cheepPage);

        //Assert
        Assert.NotNull(allCheeps);
        Assert.Equal(2, allCheeps.Count());
    }

    [Fact]
    public async void GetCheepsFromAuthorThatDoesNotExist() 
    {
        //Arrange
        var authorName = "MonsterDrinker9000";
        int cheepPage = 0;

        //Act
        var allCheeps = await cController.GetCheepsFromAuthor(authorName,cheepPage);

        //Assert
        Assert.NotNull(allCheeps);
        Assert.Equal(0, allCheeps.Count());
    }



    [Fact]
    public async void GetCheepsFromPage()
    {
        //Arrange 
        var author = new AuthorDto("Hans", "Hansemanden@asd.com");
        aController.CreateNewAuthor(author);
        for (int i = 0; i < 50; i++) {
            var cheep = new CheepDto(i.ToString(), author.Name, DateTime.Now);
            cController.CreateCheep(cheep);
        }
        var pageSize = 32;
        var expectedCheepCount = 53;
        int cheepPage1 = 0;
        int cheepPage2 = 1;

        //Act
        var cheepsPage1 = await cController.GetCheeps(cheepPage1);
        var cheepsPage2 = await cController.GetCheeps(cheepPage2);


        //Assert
        Assert.NotNull(cheepsPage1);
        Assert.Equal(pageSize, cheepsPage1.Count());
        Assert.Equal(expectedCheepCount-pageSize, cheepsPage2.Count());


    }

    [Fact]
    public async void GetCheepsFromSpecificAuthorPage() {
        //Arrange
        var author = new AuthorDto("Hans", "Hansemanden@asd.com");
        aController.CreateNewAuthor(author);
        for (int i = 0; i < 50; i++) {
            var cheep = new CheepDto(i.ToString(), author.Name, DateTime.Now);
            cController.CreateCheep(cheep);
        }
        var pageSize = 32;
        var expectedCheepCount = 50;
        var cheepPage1 = 0;
        var cheepPage2 = 1;
        var cheepPage3 = 2;

        //Act
        var cheepsPage1 = await cController.GetCheepsFromAuthor(author.Name, cheepPage1);
        var cheepsPage2 = await cController.GetCheepsFromAuthor(author.Name, cheepPage2);
        var cheepsPage3 = await cController.GetCheepsFromAuthor(author.Name, cheepPage3);


        //Assert
        Assert.NotNull(cheepsPage1);
        Assert.NotNull(cheepsPage2);
        Assert.NotNull(cheepsPage3);

        Assert.Equal(pageSize, cheepsPage1.Count());
        Assert.Equal(expectedCheepCount-pageSize, cheepsPage2.Count());
        Assert.Equal(0, cheepsPage3.Count());
    }

    [Fact]
    public async void GetAllCheeps() {
        //Arrange
        var author = new AuthorDto("Hans", "Hansemanden@asd.com");
        aController.CreateNewAuthor(author);
        for (int i = 0; i < 50; i++) {
            var cheep = new CheepDto(i.ToString(), author.Name, DateTime.Now);
            cController.CreateCheep(cheep);
        }
        var expectedCheepCount = 53;
        
        //Act
        var allCheeps = await cController.GetAllCheeps();
        
        //Assert
        Assert.NotNull(allCheeps);
        
        Assert.Equal(expectedCheepCount, allCheeps.Count());
    }

    [Fact]
    public async void GetAllCheepsFromAuthor() {
        //Arrange
        var author = new AuthorDto("Hans", "Hansemanden@asd.com");
        aController.CreateNewAuthor(author);
        for (int i = 0; i < 50; i++) {
            var cheep = new CheepDto(i.ToString(), author.Name, DateTime.Now);
            cController.CreateCheep(cheep);
        }
        var expectedCheepCount = 50;
        
        //Act
        var allCheepsFromHans = await cController.GetAllCheepsFromAuthor(author.Name);
        
        //Assert
        Assert.NotNull(allCheepsFromHans);
        
        Assert.Equal(expectedCheepCount, allCheepsFromHans.Count());
    }


     private void SeedDatabase() {
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