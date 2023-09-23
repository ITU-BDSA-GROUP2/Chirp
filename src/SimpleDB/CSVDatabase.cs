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

        Regex rx = new Regex(@"(?'author'.+),""(?'message'.+)"",(?'timestamp'\d+)""");

        public IEnumerable<T> Read(int? limit = null)
        {
            try
            {
                // Inversion of Control
                using (var sr = new StreamReader(filePath))
                using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                {
                    var allRecords = csv.GetRecords<T>().ToList();

                    var records = new List<T>();

                    if (limit >= 0) {
                        foreach (var record in allRecords) 
                        {
                            if (limit == 0) break;
                            records.Add(record);
                            limit--;
                        }
                        return records;
                    } else {
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
            using (StreamWriter w = File.AppendText(filePath))
            {
                var csv = new CsvWriter(w, CultureInfo.InvariantCulture);
                w.WriteLine();
                csv.WriteRecord(record);
                csv.Flush();
            }
        }

        public string getFilePathToDB() 
        {
            return filePath;
        }
        
    }
