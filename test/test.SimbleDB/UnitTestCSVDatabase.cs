namespace test.SimbleDB;

using SimpleDB;

public class UnitTestCSVDatabase
{
    private const int maxChrip = 10;
    private const string filePathToDB = "testingDatabase.csv";

    public CSVDatabase<Cheep> setup() {
        CSVDatabase<Cheep> temp = CSVDatabase<Cheep>.DBInstance(filePathToDB);
        using (StreamWriter w = File.AppendText(filePathToDB)) {
            w.Write("Author,Message,Timestamp");
        }
            
        for (int i = 0; i < maxChrip; i++) {
            var author = System.Environment.MachineName;
            var message =  "Hello message number: " + i;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
            var cheep = new Cheep(author,message.ToString(),timestamp); 
            temp.Store(cheep);
        }
       return temp; 
    }

    private void tearDown() {
        File.Delete(filePathToDB);
    }

    [Fact]
    public void Test_If_CSVDatabases_getFilePath_Returns_Coorect_Path()
    {
        //Arrange
        var database = setup();
        var correctFilePath = "testingDatabase.csv";

        //Assert
        Assert.True(database.getFilePathToDB() == correctFilePath);
        tearDown();
    }

    [Fact]
    public void Test_If_CSVDatabase_Is_Singleton()
    {
        //Arrange
        var database_0 = setup();
        var database_1 = setup();

        //Assert
        Assert.True(database_0.GetHashCode() == database_1.GetHashCode());
        tearDown();
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
        tearDown();
    }

    [Fact]
    public void Test_If_Read_Function_Works_With_Zero_Limit_And_Returns_All_Chirps()
    {
        //Arrange
        var database = setup();
        var limit = 0;

        //Act
        var result = database.Read(limit).ToList();
    
        //Assert
        Assert.True(result.Count() == maxChrip);
        tearDown();
    }

    [Fact]
    public void Test_If_Read_Function_Works_With_Positiv_Limit_Less_Than_MaxChirp()
    {
        //Arrange
        var database = setup();
        var limit = 5;

        //Act
        var result = database.Read(limit).ToList();
        //Assert
        Assert.True(result.Count() == limit);
        tearDown();
    }

    [Fact]
    public void Test_If_Read_Function_Works_With_Positiv_Limit_Greater_Than_MaxChrip()
    {
        //Arrange
        var database = setup();
        var newlimit = 200;
        
        //Act
        var result = database.Read(newlimit).ToList();

        //Assert
        Assert.True(result.Count() == maxChrip);
        tearDown();
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
        tearDown();
    }

    public record Cheep(string Author, string Message, long Timestamp);
}