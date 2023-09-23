namespace test.SimbleDB;

using SimpleDB;

public class UnitTestCSVDatabase
{
    
    // Startup method to instantiate database singleton
    public CSVDatabase<Cheep> setup() {
       return CSVDatabase<Cheep>.DBInstance("../../../../../data/chirp_cli_db.csv");
    }

    [Fact]
    public void Test_If_CSVDatabases_getFilePath_Returns_Coorect_Path()
    {
        //Arrange
        var database = setup();
        var correctFilePath = "../../../../../data/chirp_cli_db.csv";

        //Assert
        Assert.True(database.getFilePathToDB() == correctFilePath);
    }

    [Fact]
    public void Test_If_CSVDatabase_Is_Singleton()
    {
        //Arrange
        var database_0 = setup();
        var database_1 = setup();

        //Assert
        Assert.True(database_0.GetHashCode() == database_1.GetHashCode());
    }

    [Fact]
    public void Test_If_Read_Function_Returns_Result()
    {
        //Arrange
        var database = setup();

        //Act
        var result = database.Read().ToList();

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Count() > 0);
    }

    /*[Fact]
    public void Test_If_Read_Function_Works_With_Negativ_Limit()
    {
        //Arrange
        var database = setup();
        var limit = -1;

        //Act
        var result = database.Read(limit).ToList();

        //Assert
        Assert.True(result.Count() != limit);
    }*/

    [Fact]
    public void Test_If_Read_Function_Works_With_Zero_Limit()
    {
        //Arrange
        var database = setup();
        var limit = 0;

        //Act
        var result = database.Read(limit).ToList();

        //Assert
        Assert.True(result.Count() == limit);
    }

    [Fact]
    public void Test_If_Read_Function_Works_With_Positiv_Limit()
    {
        //Arrange
        var database = setup();
        var limit = 5;

        //Act
        var result = database.Read(limit).ToList();

        //Assert
        Assert.True(result.Count() == limit);
    }

    [Fact]
    public void Test_If_Read_Function_Works_With_Positiv_Limit_1()
    {
        //Arrange
        var database = setup();
        var limit = 200;
        var numberOfCheeps = 21;

        //Act
        var result = database.Read(limit).ToList();

        //Assert
        Assert.True(result.Count() == numberOfCheeps);
    }

    [Fact]
    public void Test_When_Read_Function_Reads_From_Wrong_Filepath()
    {
        //Arrange
        //var database = CSVDatabase<Cheep>.DBInstance("chirp_cli_db.csv");
        
        //Act
        //var result = database.Read();
        //ArgumentException exception = Assert.Throws<ArgumentException>(database.Read().ToList());
    
        //The thrown exception can be used for even more detailed assertions.
         //Assert.Equal("expected error message here", exception.Message);

        //Assert
       // Assert.True(database.getFilePathToDB() == correctFilePath);
    }

    [Fact]
    public void Test_If_Store_Stores_A_Record()
    {
        //Arrange
        var database = setup();
        var currentNumberOfRecords = database.Read().Count();

        var author = System.Environment.MachineName;
        var message =  "This is for testing purposes only";
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        var cheep = new Cheep(author,message,timestamp); 
        
        //Act
        database.Store(cheep);
        var newNumberOfRecords = database.Read().Count();

        //Assert
        Assert.True(currentNumberOfRecords+1 == newNumberOfRecords);
    }

    [Fact]
    public void Test_If_Store_Function_Stored_The_Record_Correctly()
    {
        //Arrange
        var database = setup();
        var currentNumberOfRecords = database.Read().Count();
        
        //Act
        database.Store(cheep);
        var newNumberOfRecords = database.Read().Count();

        //Assert
        Assert.True(currentNumberOfRecords+1 == newNumberOfRecords);
    }



    public record Cheep(string Author, string Message, long Timestamp);
}