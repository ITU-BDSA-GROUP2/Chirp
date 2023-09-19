namespace test.Chirp;

using Program.cs

public class UnitTest1
{
    [Theory]
    [InlineData("Mads Orfelt", "hej med her alle",)]
    [InlineData("")]
    [InlineData("Mads Orfelt")]
    public void Test1()
    {
        //Arrange 
        string name = "Mads Orfelt"
        string cheep = "hej med jer alle sammen"
        string timestamp = ""
        Program program = new Program()

        //Act
        program.cheep()
    }
}