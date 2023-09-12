
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using SimpleDB;



string command = args[0];
string message = args[1];



var pattern = """(?'author'.+),"(?'message'.+)",(?'timestamp'\d+)""";
Regex rx = new Regex(pattern);

string epoch2String(int epoch) {
return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToString(); }
    

if (command == "read") {
} else if(command == "cheep") {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
    

}
public record Cheep(string Author, string Message, long Timestamp);


