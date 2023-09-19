namespace test.Chirp;

using Program.cs
using UserInterface.cs

public class UnitTest1
{
    [Theory]
    [InlineData("Mads Orfelt", "hej med her alle",181882832)]
    [InlineData("Oliver","Gustav er min bedste ven", 99999999)]
    [InlineData("Casper", "elon er cool", 123444321)]
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
    [InlineData()]
    public void epoch2String() {

    }
}