using VoteSummary.Console.Model;
using static System.Console;

namespace VoteSummary.Console
{
    internal class Program
    {
        private const int YEA_VOTE = 1;
        private const string QUESTION1_RESULT_FILE = "legislators-support-oppose-count.csv";
        private const string QUESTION2_RESULT_FILE = "bills-support-oppose-count.csv";
        private const string VOTE_RESULTS_FILE = "vote_results.csv";
        private const string BILLS_FILE = "bills.csv";
        private const string VOTES_FILE = "votes.csv";
        private const string LEGISLATORS_FILE = "legislators.csv";

        static async Task Main()
        {

            WriteLine("Reading csv files from root folder....");
            var legislators = await CsvUtils.ReadCsv<Legislator>(LEGISLATORS_FILE);
            var voteResults = await CsvUtils.ReadCsv<VoteResult>(VOTE_RESULTS_FILE);
            var bills = await CsvUtils.ReadCsv<Bill>(BILLS_FILE);
            var votes = await CsvUtils.ReadCsv<Vote>(VOTES_FILE);

            WriteLine("Solving question 1....");
            var legislatorsDict = legislators
                .Select(l => new KeyValuePair<int, string>(l.id, l.name))
                .ToDictionary();

            var q1Result = Question1Solver.Solve(legislatorsDict, voteResults);
            CsvUtils.SaveToCsv(q1Result, QUESTION1_RESULT_FILE);
            WriteLine($"Question 1 solved. Output file {QUESTION1_RESULT_FILE}");


            WriteLine("Solving question 2....");
            var q2Result = Question2Solver.Solve(legislatorsDict, voteResults, bills, votes);
            CsvUtils.SaveToCsv(q2Result, QUESTION2_RESULT_FILE);
            WriteLine($"Question 2 solved. Output file {QUESTION2_RESULT_FILE}");
        }
    }
}
