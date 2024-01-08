using CsvHelper;
using System.Globalization;
using VoteSummary.Console.Model;

namespace VoteSummary.Console
{
    public static class CsvUtils
    {
        public static void SaveToCsv<T>(IEnumerable<T> results, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(results);
        }

        // Read async cause there can exist a very huge number of VoteResults
        public static async IAsyncEnumerable<VoteResult> ReadVoteResults()
        {
            using var reader = new StreamReader("vote_results.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await csv.ReadAsync();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<VoteResult>();
                if (record != null)
                    yield return record;
            }
        }

        // Read synch because Legislators are a fixed small number": all american legislators
        // in all states and congress, just a few thousand rows.
        // Use IDictionary to make faster lookup for Legislator's Name
        public static async Task<IDictionary<int, string>> ReadLegislatorsAsDictionary()
        {
            using var reader = new StreamReader("legislators.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await csv.ReadAsync();
            csv.ReadHeader();
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
