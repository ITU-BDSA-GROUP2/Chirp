using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Chirp.Razor;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
namespace test.Chirp;

public class InMemoryTests : IDisposable {
    private readonly DbContextOptions<ChirpDBContext> builder;
    private readonly SqliteConnection connection;
    private readonly ChirpDBContext context;
    private readonly AuthorRepository aController;
    private readonly CheepRepository cController;
    private readonly FollowerListRepository fController;



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

        fController = new FollowerListRepository(context);

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
        var authorName = "Asger";
        var email = "Asger@asd.com";

        //Act
         await aController.CreateNewAuthor(authorName, email);
         var author = await context.Authors
         .Where(a => a.Name == authorName)
         .FirstOrDefaultAsync();

        //Assert
         Assert.NotNull(author);
         Assert.Equal(authorName, authorName);
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

    //Found from here: https://stackoverflow.com/a/45017575
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


    //Inspiration from here: https://stackoverflow.com/a/45017575
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
        var cheepMessage = "Hello my friends! :)";
        var cheepAuthor = "Voldemort";
        var time = DateTime.Now;

        //Act
        await cController.CreateCheep(cheepMessage, cheepAuthor, time);
        var cheep = await context.Cheeps
        .Where(c => c.Text == cheepMessage)
        .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(cheep);
        Assert.Equal(cheepMessage, cheep.Text);
        Assert.Equal(cheepAuthor, cheep.Author.Name);
        Assert.Equal(time, cheep.TimeStamp);
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
        Assert.Empty(allCheeps);
    }



    [Fact]
    public async void GetCheepsFromPage()
    {
        //Arrange 
        var authorName = "Hans";
        var email = "Hansemanden@asd.com";
        await aController.CreateNewAuthor(authorName, email);
        for (int i = 0; i < 50; i++) {
            await cController.CreateCheep(i.ToString(), authorName, DateTime.Now);
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
        var authorName = "Hans";
        var email = "Hansemanden@asd.com";
        await aController.CreateNewAuthor(authorName, email);
        for (int i = 0; i < 50; i++) {
            await cController.CreateCheep(i.ToString(), authorName, DateTime.Now);
        }
        var pageSize = 32;
        var expectedCheepCount = 50;
        var cheepPage1 = 0;
        var cheepPage2 = 1;
        var cheepPage3 = 2;

        //Act
        var cheepsPage1 = await cController.GetCheepsFromAuthor(authorName, cheepPage1);
        var cheepsPage2 = await cController.GetCheepsFromAuthor(authorName, cheepPage2);
        var cheepsPage3 = await cController.GetCheepsFromAuthor(authorName, cheepPage3);


        //Assert
        Assert.NotNull(cheepsPage1);
        Assert.NotNull(cheepsPage2);
        Assert.NotNull(cheepsPage3);

        Assert.Equal(pageSize, cheepsPage1.Count());
        Assert.Equal(expectedCheepCount-pageSize, cheepsPage2.Count());
        Assert.Empty(cheepsPage3);
    }

    [Fact]
    public async void GetAllCheeps() {
        //Arrange
        var authorName = "Hans";
        var email = "Hansemanden@asd.com";
        await aController.CreateNewAuthor(authorName, email);
        for (int i = 0; i < 50; i++) {
            await cController.CreateCheep(i.ToString(), authorName, DateTime.Now);
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
        var authorName = "Hans";
        var email = "Hansemanden@asd.com";
        await aController.CreateNewAuthor(authorName, email);
        for (int i = 0; i < 50; i++) {
            await cController.CreateCheep(i.ToString(), authorName, DateTime.Now);
        }
        var expectedCheepCount = 50;
        
        //Act
        var allCheepsFromHans = await cController.GetAllCheepsFromAuthor(authorName);
        
        //Assert
        Assert.NotNull(allCheepsFromHans);
        
        Assert.Equal(expectedCheepCount, allCheepsFromHans.Count());
    }

    [Fact]
    public async void FollowAuthor() {

        //Arrange
        var loggedInUser = "Voldemort";
        var loggedInUserId = await context.Authors.Where(a => a.Name == loggedInUser).Select(a => a.AuthorId).FirstOrDefaultAsync();

        var userToFollow = "Svanhildur";
        var userToFollowId = await context.Authors.Where(a => a.Name == userToFollow).Select(a => a.AuthorId).FirstOrDefaultAsync();


        
        //Act
        await fController.Follow(loggedInUser, userToFollow);
        bool isUserFollowed = await context.Following.Where(a => a.UserId == loggedInUserId && a.FollowedAuthorId == userToFollowId).FirstOrDefaultAsync() != null;


        //Assert
        Assert.Equal(isUserFollowed, true);
    }

    [Fact]
    public async void UnfollowAuthor() {

        //Arrange
        var loggedInUser = "Voldemort";
        var loggedInUserId = await context.Authors.Where(a => a.Name == loggedInUser).Select(a => a.AuthorId).FirstOrDefaultAsync();

        var userToFollow = "Svanhildur";
        var userToFollowId = await context.Authors.Where(a => a.Name == userToFollow).Select(a => a.AuthorId).FirstOrDefaultAsync();
        await fController.Follow(loggedInUser, userToFollow);


        
        //Act
        await fController.UnFollow(loggedInUser, userToFollow);
        bool isUserFollowed = await context.Following.Where(a => a.UserId == loggedInUserId && a.FollowedAuthorId == userToFollowId).FirstOrDefaultAsync() != null;

        //Assert
        Assert.Equal(isUserFollowed, false);
    }

    [Fact]
    public async void GetAllCheepsFromFollowed() {

        //Arrange
        var loggedInUser = "Voldemort";
        var loggedInUserId = await context.Authors.Where(a => a.Name == loggedInUser).Select(a => a.AuthorId).FirstOrDefaultAsync();

        var userToFollow = "Svanhildur";
        var userToFollowId = await context.Authors.Where(a => a.Name == userToFollow).Select(a => a.AuthorId).FirstOrDefaultAsync();
        await fController.Follow(loggedInUser, userToFollow);

        var expectedCheepCount = 3;


        
        //Act
        var cheepsFromUserAndFollowed = await cController.GetAllCheepsFromFollowed(loggedInUser, 0);
        //Assert
        Assert.NotNull(cheepsFromUserAndFollowed);
        Assert.Equal(expectedCheepCount, cheepsFromUserAndFollowed.Count());
    }

    [Fact]
    public async void UpdateAuthor() {

        //Arrange
        var loggedInUser = "Voldemort";
        var desiredName = "Voldemor";
        var desiredEmail = "Voldemor@gmail.com";

        //Act
        await aController.UpdateAuthor(loggedInUser, desiredName, desiredEmail);

        var updatedAuthorCheck = await context.Authors
        .Where(a => a.Name == "Voldemor")
        .FirstOrDefaultAsync();
        var oldAuthor = await context.Authors
        .Where(a => a.Name == "Voldemort")
        .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(updatedAuthorCheck);
        Assert.Null(oldAuthor);

        Assert.Equal(updatedAuthorCheck.Name, "Voldemor");
        Assert.Equal(updatedAuthorCheck.Email, "Voldemor@gmail.com");

    }

    [Fact]
    public async void DeleteAuthor() {

        //Arrange
        var userName = "Voldemort";

        var user = await context.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        //Act
        await aController.DeleteAuthor(userName);
        var deletedAuthorCheck = await context.Authors
        .Where(a => a.Name == userName)
        .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(user);
        Assert.Null(deletedAuthorCheck);
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