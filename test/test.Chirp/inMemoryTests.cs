// using System;
// using System.Data.Common;
// using System.Linq;
// //using EF.Testing.BloggingWebApi.Controllers;
// //using EF.Testing.BusinessLogic;
// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Xunit;
// using Chirp.Razor;
// using Infrastructure;
// namespace EF.Testing.UnitTests;

// //http://localhost:5273

// public class SqliteInMemoryChirpingControllerTest : IDisposable
// {
//     private readonly DbConnection _connection;
//     private readonly DbContextOptions<ChirpDBContext> _contextOptions;

//     #region ConstructorAndDispose
//     public SqliteInMemoryChirpingControllerTest()
//     {
//         // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
//         // at the end of the test (see Dispose below).
//         _connection = new SqliteConnection("Filename=:memory:");
//         _connection.Open();

//         // These options will be used by the context instances in this test suite, including the connection opened above.
//         _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>()
//             .UseSqlite(_connection)
//             .Options;

//         // Create the schema and seed some data
//         using var context = new ChirpDBContext();

//         if (context.Database.EnsureCreated())
//         {
//             using var viewCommand = context.Database.GetDbConnection().CreateCommand();
//             viewCommand.CommandText = @"
//                 CREATE VIEW AllAuthors AS
//                 SELECT Email
//                 FROM Author;";
//             viewCommand.ExecuteNonQuery();
//         }
//         var author1 = new Author
//         {
//             Name = "Helge",
//             Email = "ropf@itu.dk",
//             Cheeps = new List<Cheep> { new Cheep { Text = "Hello, BDSA students!" } }
//         };

//         var author2 = new Author
//         {
//             Name = "Rasmus",
//             Email = "rnie@itu.dk",
//             Cheeps = new List<Cheep> { new Cheep { Text = "Hej, velkommen til kurset." } }
//         };
//         var cheep1 = new Cheep {Text = "Hej med jer alle!", Author = author1, TimeStamp = DateTime.Now};
//         var cheep2 = new Cheep {Text = "wow det bliver vildt!", Author = author2, TimeStamp = DateTime.Now};
//         context.AddRange(author1, author2, cheep1, cheep2);
//         context.SaveChanges();
//     }

//     ChirpDBContext CreateContext() => new ChirpDBContext();

//     public void Dispose() => _connection.Dispose();
//     #endregion

//     #region GetAuthorByName
//     [Fact]
//     public async Task getAuthorByName()
//     {

//         //Arrange
//         using var context = CreateContext();
//         var controller = new AuthorRepository(context);

//         //Act
//         var authorDto = await controller.GetAuthorByName("Helge");

//         //Assert
//         Assert.NotNull(authorDto);
//         Assert.Equal("Helge", authorDto.Name);
//     }
// #endregion

//     [Fact]
//     public async Task getAuthorByEmail()
//     {
//         //Arrange
//         using var context = CreateContext();
//         var controller = new AuthorRepository(context);

//         //Act
//         var authorDto1 = await controller.GetAuthorByName("Rasmus");

//         //Assert
//         Assert.NotNull(authorDto1);
//         Assert.Equal("rnie@itu.dk", authorDto1.Email);
//     }

//     // [Fact]
//     //  public void GetAllBlogs()
//     //  {
//     //      using var context = CreateContext();
//     //      var controller = new AuthorRepository(context);

//     //      var blogs = controller.GetAllBlogs().Value;

//     //      Assert.Collection(
//     //          blogs,
//     //          b => Assert.Equal("Blog1", b.Name),
//     //          b => Assert.Equal("Blog2", b.Name));
//     //  }

//      [Fact]
//      public void CreateNewAuthor()
//      {
//         //Arrange
//          using var context = CreateContext();
//          var controller = new AuthorRepository(context);
//          var authorDto1 = new AuthorDto("Helge", "ropf@itu.dk");

//         //Act
//          controller.CreateNewAuthor(authorDto1);
//          var author = context.Authors.FirstOrDefault(a => a.Name == "Helge");

//         //Assert
//          Assert.NotNull(author);
//          Assert.Equal(authorDto1.Name, author.Name);
//      }

//      [Fact]
//      public async void GetCheepsFromAuthor()
//      {

//         //Arrange
//          using var context = CreateContext();
//          var controller = new CheepRepository(context);

//         //Act
//          var cheeps = await controller.GetCheepsFromAuthor("Helge", 1);
//          var firstCheepName = cheeps.First().Author;
        
//         //Assert
//          Assert.NotNull(cheeps);
//          Assert.Equal("Helge", firstCheepName);
//      }

//      [Fact]
//      public void createCheep() 
//      {

//         //Arrange
//         using var context = CreateContext();
//         var controller = new CheepRepository(context);
//         var cheep1 = new CheepDto("Hej med jer alle!", "Helge", DateTime.Now);

//         //Act
//         controller.CreateCheep(cheep1);
//         var cheep = context.Cheeps.FirstOrDefault(a => a.Text == "Hej med jer alle!");

//         //Assert
//         Assert.NotNull(cheep);
//         Assert.Equal(cheep1.Text, cheep.Text);
//      }
    
//     [Fact]
//     public async void getAllCheeps() 
//     {

//         //Arrange
//         using var context = CreateContext();
//         var controller = new CheepRepository(context);

//         //Act
//         var cheeps1 = await controller.GetCheeps(1);
        
//         //Assert
//         Assert.NotNull(cheeps1);
//         Assert.Equal("Hej med jer alle!", cheeps1.ElementAt(0).Text);
//         Assert.Equal("wow det bliver vildt!", cheeps1.ElementAt(1).Text);

//     }

//     // [Fact]
//     // public void UpdateBlogUrl()
//     // {
//     //     using var context = CreateContext();
//     //     var controller = new BloggingController(context);

//     //     controller.UpdateBlogUrl("Blog2", "http://blog2_updated.com");

//     //     var blog = context.Blogs.Single(b => b.Name == "Blog2");
//     //     Assert.Equal("http://blog2_updated.com", blog.Url);
//     // }
// }