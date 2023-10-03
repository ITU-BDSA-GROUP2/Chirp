using System.Data;
using Microsoft.Data.Sqlite;
public class DBFacade 
{
    DBFacade()
    {
        CheepService someCheepService = new CheepService();
        someCheepService.GetCheeps();
    }
}