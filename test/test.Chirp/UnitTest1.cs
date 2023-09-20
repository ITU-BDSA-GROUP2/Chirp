namespace test.Chirp;

//using Program;

public class UnitTest1
{
    [Theory]
    [InlineData(1690891760)]
    [InlineData(1690978778)]
    [InlineData(1690979858)]
    [InlineData(1690981487)]
    [InlineData(1694517710)]
    [InlineData(1694517854)]
    [InlineData(1694521023)]
    public void test(int epoch) {
        //Arrange
        
        
        //Act
        var value = UserInterface.epoch2String(epoch);
        //Assert
        Assert.Equal(value,"01-08-2023 12:09:20");
    }
}