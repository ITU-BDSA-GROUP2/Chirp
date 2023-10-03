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
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
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
            using (StreamWriter w = File.AppendText(filePath))
            using (var csv = new CsvWriter(w, CultureInfo.InvariantCulture)) 
            {
                    csv.WriteRecord(record);
                    w.WriteLine();
                    csv.Flush();
            }
        }
        public string getFilePathToDB() 
        {
            return filePath;
        }

        
}
