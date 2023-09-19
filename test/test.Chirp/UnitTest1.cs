namespace test.Chirp;

using Program.cs
using UserInterface.cs

public class UnitTest1
{
    [Theory]
    [InlineData("Mads Orfelt", "hej med her alle",181882832)]
    [InlineData("Oliver","Gustav er min bedste ven", 99999999)]
    [InlineData("Casper", "Elon Musk er cool", 123444321)]
    public void Test1(string name, string message, int timestamp)
    {
        //Arrange 
        Program program = new Program()
        var program.cheep = new cheep(name,message,timestamp)

        //Act
        program.cheep()

        Assert.Equal()
    }
    [Theory]
    [InlineData(1690891760)]
    /*[InlineData(1690978778)]
    [InlineData(1690979858)]
    [InlineData(1690981487)]
    [InlineData(1694517710)]
    [InlineData(1694517854)]
    [InlineData(1694521023)]*/
    public void epoch2String(int epoch) {
        //Arrange
        UserInterface user = new UserInterface()
        
        //Act
        var value = user.epoch2String(epoch)
        //Assert
        AssertEqual(value,"01-08-2023 12:09:20")
    }
}