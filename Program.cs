using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

string command = args[0];
var pattern = """(?'author'.+),"(?'message'.+)",(?'timestamp'\d+)""";
Regex rx = new Regex(pattern);

string epoch2String(int epoch) {
return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToString(); }



if (command == "read") {
try {
    using (var sr = new StreamReader("chirp_cli_db.csv")) {
            sr.ReadLine();
                while (!sr.EndOfStream) {
                string input = sr.ReadLine();
                var match = Regex.Match(input, pattern);
                var author = match.Groups["author"];
                var message = match.Groups["message"];
                string timestamp = match.Groups["timestamp"].ToString();
                string t = epoch2String(int.Parse(timestamp));
                Console.WriteLine($"{author} @ " + t + ": " + $"{message}");
            }
        }
} catch (IOException e) {
    Console.WriteLine("The file could not be read");
    Console.WriteLine(e.Message);
}
} else if(command == "cheep") {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp

using(StreamWriter w = File.AppendText("chirp_cli_db.csv")) 
{
        var author = System.Environment.MachineName;
        var message = "\"" + args[1] + "\"";
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        var line = string.Format("{0},{1},{2}", author, message,timestamp);
        w.WriteLine(line);
        w.Flush();
}
}