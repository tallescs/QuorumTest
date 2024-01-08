using CsvHelper;
using System.Globalization;
using VoteSummary.Console.Model;

namespace VoteSummary.Console
{
    public static class CsvUtils
    {
        public static void SaveToCsv<T>(IEnumerable<T> list, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(list);
        }

        public static async Task<IEnumerable<T>> ReadCsv<T>(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var list = new List<T>();

            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<T>();
                if (record != null)
                    list.Add(record);
            }
            return list;
        }

        // Read synch because Legislators are a fixed small number": all american legislators
        // in all states and congress, just a few thousand rows.
        // Use IDictionary to make faster lookup for Legislator's Name
        public static async Task<IDictionary<int, string>> ReadLegislatorsAsDictionary()
        {
            using var reader = new StreamReader("legislators.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            //csv.ReadHeader();
            var dict = new Dictionary<int, string>();

            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<Legislator>();
                if (record != null)
                    dict.Add(record.id, record.name);
            }
            return dict;
        }

    }
}
