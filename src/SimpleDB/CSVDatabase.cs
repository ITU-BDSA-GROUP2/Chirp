namespace SimpleDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using CsvHelper;
using CsvHelper.Expressions;
using System.Reflection;



    public sealed class CSVDatabase<T> : IDatabaseRepository<T>
    {
        // Singleton pattern
        private static CSVDatabase<T> instance;
        private string filePath;

        private CSVDatabase(string filePath) {
            this.filePath = filePath;
        }

        public static CSVDatabase<T> DBInstance(string filePathToDB)
        {
            if (instance == null) {
                instance = new CSVDatabase<T>(filePathToDB);
                return instance;
            }
            return instance;
        }

        //Regex rx = new Regex(@"(?'author'.+),""(?'message'.+)"",(?'timestamp'\d+)""");

        public IEnumerable<T> Read(int? limit = null)
        {
            try
            {
               Assembly assembly = Assembly.GetExecutingAssembly(); // You can use another assembly if needed
                
                
                using (Stream stream = assembly.GetManifestResourceStream(filePath))
                {
                   if (stream == null)
                    {
                        Console.WriteLine("Embedded resource not found.");
                        return null;
                    }

                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var allRecords = csv.GetRecords<T>().ToList();

                        if (limit.HasValue && limit > 0)
                        {
                            return allRecords.Take(limit.Value);
                        }

                        return allRecords;
                    }
                }
            } catch (IOException e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Store(T record)
        {

            Assembly assembly = Assembly.GetExecutingAssembly(); // You can use another assembly if needed                
                using (Stream stream = assembly.GetManifestResourceStream(filePath))
                {
                    using (StreamWriter w = new StreamWriter(stream))
                    using (var csv = new CsvWriter(w, CultureInfo.InvariantCulture)) {
                        csv.WriteRecords(Read());
                        csv.WriteRecord(record);
                        w.WriteLine();
                        csv.Flush();
                    }

                }

            

           
        }

        public string getFilePathToDB() 
        {
            return filePath;
        }
        
}
