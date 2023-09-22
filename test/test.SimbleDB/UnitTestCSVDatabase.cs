namespace test.SimbleDB;

using SimpleDB;

public class UnitTestCSVDatabase
{
    public record Cheep(string Author, string Message, long Timestamp);

    [Fact]
    public void Test()
    {
        //Arrange
        var database = CSVDatabase<Cheep>.DBInstance;

        //Act
        var result = database.Read().ToList();

        //Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Count() > 0);
    }

    [Fact]
    public void Test1()
    {
        //Arrange
        var database = CSVDatabase<Cheep>.DBInstance;
        var limit = 5;

        //Act
        var result = database.Read(limit).ToList();

        Assert.True(result.Count() == 5);
    }
}