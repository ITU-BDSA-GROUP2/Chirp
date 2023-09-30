namespace test.Chirp;

public class UnitTestChirp
{
    [Theory]
    [InlineData(1690891760, "01/08/2023 12:09:20")]
    [InlineData(967329900, "26/08/2000 22:45:00")]
    [InlineData(937925665, "21/09/1999 14:54:25")]
    [InlineData(29851932, "12/12/1970 12:12:12")]
    [InlineData(1049328015, "03/04/2003 00:00:15")]
    public void epoch2String_Using_Different_times_Correct_Times(int epoch, string time) {
        //Act
        var value = UserInterface.epoch2String(epoch);

        //Assert
        Assert.Equal(value, time);
        
    }

    [Theory]
    [InlineData(29851932, "01/08/2023 12:09:20")]
    [InlineData(965329900, "08/27/2000 12:45:00")]
    [InlineData(930920660, "12/09/1999 14:54:25")]
    [InlineData(1690891760, "12/12/1970 12:12:12")]
    [InlineData(1000000000, "03/04/2003 00:00:15")]
    public void epoch2String_Using_Different_times_Wrong_Times(int epoch, string time) {
        // Act
        var value = UserInterface.epoch2String(epoch);

        // Assert
        Assert.NotEqual(time,value);
    }

    [Theory]
    [InlineData("Mads Orfelt", "det en cool tweet", 1000000000)]
    [InlineData("Oliver Prip", "godt vejr til at spille fodbold", 545355450)]
    [InlineData("Casper Pilgaard", "skal vi tage i Føtex?", 1215432234)]
    [InlineData("Mads Orfelt", "waouw det bliver vildt", 971543352)]
    [InlineData("Mads Orfelt", "waouw det bliver vildt", 860432241)]
    public void PrintCheeps_Should_Print_Cheep_Correctly(string Author, string Message, long Timestamp)
    {
        // Arrange
        var cheep = new Cheep(Author, Message, Timestamp);
        IEnumerable<Cheep> cheeps = new List<Cheep> { cheep };

    
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            UserInterface.PrintCheeps(cheeps);
            string capturedOutput = sw.ToString();

            // Assert
            string expected = $"{Author} @ {UserInterface.epoch2String(Convert.ToInt32(Timestamp))}: {Message}";
            Assert.Equal(expected, capturedOutput.Trim());
        }
    }
}