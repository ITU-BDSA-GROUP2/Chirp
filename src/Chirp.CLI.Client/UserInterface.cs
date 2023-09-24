using System;
using System.Collections.Generic;

public static class UserInterface {

    public static string epoch2String(int epoch) {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToString(); 
    }

    public static void PrintCheeps(IEnumerable<Cheep> cheeps) 
    {
        foreach (var cheep in cheeps) 
        {
            string time = epoch2String(int.Parse(cheep.Timestamp.ToString()));
            Console.WriteLine($"{cheep.Author} @ " + time + ": " + $"{cheep.Message}");
        }
    }
}