using CsvHelper;
using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static DbConnectionClassLib.Data.DatabaseInstance;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<T> ParseCSV<T>(string csvString, string delimiter)
        {
            List<T> response = new List<T>();
            try
            {
                using (TextReader sr = new StringReader(csvString))
                {
                    CsvHelper.Configuration.Configuration config = new CsvHelper.Configuration.Configuration() { CultureInfo = new CultureInfo("en-US") };
                    config.MissingFieldFound = null;
                    config.Delimiter = delimiter;
                    var csv = new CsvReader(sr, config);
                    csv.Read();
                    csv.ReadHeader();
                    response = csv.GetRecords<T>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ParseCSV: csvString:" + csvString, ex);
            }
            return response;
        }
    }
}
