namespace test.Chirp;

//using Program;


public class UnitTest1
{
    [Theory]
    [InlineData(1690891760, "01-08-2023 12:09:20")]
    [InlineData(967329900, "08-27-2000  12:45:00")]
    [InlineData(1690978778, "02-08-2023 12:19:38")]
    [InlineData(1690979858, "02-08-2023 12:37:38")]
    [InlineData(1690981487, "02-08-2023 13:04:47")]
    [InlineData(1694517710, "12-09-2023 11:21:50")]
    [InlineData(1694517854, "12-09-2023 11:24:14")]
    [InlineData(1694521023, "12-09-2023 12:17:03")]
    public void test(int epoch, string time) {
        //Arrange
        
        //Act
        var value = UserInterface.epoch2String(epoch);

        //Assert
        Assert.Equal(value, time);
        
    }
    [Theory]
    [InlineData("19-09-2023 10:17:02", 695129684)]
    [InlineData("12-12-2012 12:12:12", 1456628758)]
    [InlineData("19-09-2023 10:17:09", 1695088079)]
    [InlineData("19-09-2023 10:19:01", 1694528758)]
    [InlineData("30-01-1999 11:00:00", 124528758)]
    public void test2(string time, int epoch) {

        var value = UserInterface.epoch2String(epoch);

        Assert.NotEqual(time,value);
    }

    /*public void test3() {
        List<CheepCheep> list = new List<CheepCheep>();
        list.Add("Mads Orfelt", "det en cool tweet",1690891760);
        //list.Add("Oliver Prip", "hej med jer alle sammen", 1690978778);
        

        IEnumerable<CheepCheep> en = list;
        

        Assert.Equal(UserInterface.PrintCheeps(en),"Mads Orfelt @ 01-08-2023 12:09:20: det en cool tweet");
    }*/
    
}