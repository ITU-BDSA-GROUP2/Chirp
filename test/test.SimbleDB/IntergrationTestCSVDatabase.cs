namespace test.SimbleDB;

using SimpleDB;
using System;
using System.IO;

public class IntergrationTestCSVDatabase
{
    private const int maxChrip = 10;
    private const string filePathToDB = "../../../../../data/testingDatabase.csv";

    public CSVDatabase<Cheep> setup() {
        CSVDatabase<Cheep> temp = CSVDatabase<Cheep>.DBInstance(filePathToDB); 
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
    public void Test_Store_And_Read_Methods() {
        var database = setup();
        var i = maxChrip;
    
        var author = System.Environment.MachineName;
        var message =  "This is a test Chirp";
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        var cheepToSend = new Cheep(author, message, timestamp); 
        database.Store(cheepToSend);

        var databseCheeps = database.Read();
        
        foreach (var cheep in databseCheeps) {
            if(i == 0) {
                 Assert.True(cheep == cheepToSend);
            }
            i--;
        }
       tearDown();
    }

    public record Cheep(string Author, string Message, long Timestamp);
}