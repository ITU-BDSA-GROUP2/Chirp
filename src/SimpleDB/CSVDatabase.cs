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
        private static CSVDatabase<T> instance;
        private string filePath;

        private CSVDatabase(string filePath) {
            this.filePath = filePath;

            if (!File.Exists(filePath))
            {
                using(StreamWriter w = File.CreateText(filePath))
                using(var csv = new CsvWriter(w, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<T>();
                }
            }
        }

        public static CSVDatabase<T> DBInstance(string filePathToDB)
        {
            if (instance == null) {
                instance = new CSVDatabase<T>(filePathToDB);
                return instance;
            }
            return instance;
        }

        public IEnumerable<T> Read(int? limit = null)
        {
            try
            {
<<<<<<< HEAD
               Assembly assembly = Assembly.GetExecutingAssembly(); // You can use another assembly if needed
                
                
                using (Stream stream = assembly.GetManifestResourceStream(filePath))
=======
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
>>>>>>> Retired-Chirp.CLI.Client
                {
                    var allRecords = csv.GetRecords<T>().ToList();

                    if (limit.HasValue && limit > 0)
                    {
                        return allRecords.Take(limit.Value);
                    }
                    return allRecords;
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

            
            try
            {
                var newList = new List<T>(Read());
                newList.Add(record);
                Assembly assembly = Assembly.GetExecutingAssembly(); // You can use another assembly if needed                
                using (Stream stream = assembly.GetManifestResourceStream(filePath))
                {
                    using (StreamWriter w = new StreamWriter(stream))
                    using (var csv = new CsvWriter(w, CultureInfo.InvariantCulture)) {
                        csv.WriteRecords(newList);
                        w.WriteLine();
                        csv.Flush();
                    }

                }
            } catch (IOException e) {
                Console.WriteLine(e.Message);
                return;
            }

            

           
        }

        public string getFilePathToDB() 
        {
            return filePath;
        }
        
}
