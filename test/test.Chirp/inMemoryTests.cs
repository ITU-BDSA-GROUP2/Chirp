using System;
using System.Data.Common;
using System.Linq;
//using EF.Testing.BloggingWebApi.Controllers;
//using EF.Testing.BusinessLogic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Chirp.Razor;
using Infrastructure;
namespace EF.Testing.UnitTests;

//http://localhost:5273

public class SqliteInMemoryChirpingControllerTest : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ChirpDBContext> _contextOptions;

    #region ConstructorAndDispose
    public SqliteInMemoryChirpingControllerTest()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new ChirpDBContext();

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW AllAuthors AS
                SELECT Email
                FROM Author;";
            viewCommand.ExecuteNonQuery();
        }
        var author1 = new Author
        {
            Name = "Helge",
            Email = "ropf@itu.dk",
            Cheeps = new List<Cheep> { new Cheep { Text = "Hello, BDSA students!" } }
        };

        var author2 = new Author
        {
            Name = "Rasmus",
            Email = "ropf@itu.dk",
            Cheeps = new List<Cheep> { new Cheep { Text = "Hej, velkommen til kurset." } }
        };
        context.AddRange(author1, author2);
        context.SaveChanges();
    }

    ChirpDBContext CreateContext() => new ChirpDBContext();

    public void Dispose() => _connection.Dispose();
    #endregion

    #region GetAuthor
    [Fact]
    public async Task GetAuthor()
    {
    using var context = CreateContext();

    var controller = new AuthorRepository(context);

    var authorDto = await controller.GetAuthorByName("Helge");

    Assert.NotNull(authorDto);
    Assert.Equal("ropf@itu.dk", authorDto.Email);
    }
#endregion

    // [Fact]
    //  public void GetAllBlogs()
    //  {
    //      using var context = CreateContext();
    //      var controller = new AuthorRepository(context);

    //      var blogs = controller.GetAllBlogs().Value;

    //      Assert.Collection(
    //          blogs,
    //          b => Assert.Equal("Blog1", b.Name),
    //          b => Assert.Equal("Blog2", b.Name));
    //  }

     [Fact]
     public void CreateNewAuthor()
     {
         using var context = CreateContext();
         var controller = new AuthorRepository(context);

         var authorDto1 = new AuthorDto("Helge", "ropf@itu.dk");

         controller.CreateNewAuthor(authorDto1);

         var author = context.Authors.FirstOrDefault(a => a.Name == "Helge");

         Assert.NotNull(author);
         Assert.Equal("Helge", author.Name);
     }

    // [Fact]
    // public void UpdateBlogUrl()
    // {
    //     using var context = CreateContext();
    //     var controller = new BloggingController(context);

    //     controller.UpdateBlogUrl("Blog2", "http://blog2_updated.com");

    //     var blog = context.Blogs.Single(b => b.Name == "Blog2");
    //     Assert.Equal("http://blog2_updated.com", blog.Url);
    // }
}