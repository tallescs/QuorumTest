using VoteSummary.Console.Model;

namespace VoteSummary.Console
{
    internal class Program
    {
        private const int YEA_VOTE = 1;
        private const string QUESTION1_RESULT_FILE = "legislators-support-oppose-count.csv";
        static async Task Main()
        {
            // Question 1:
            var legislators = await CsvUtils.ReadLegislatorsAsDictionary();
            var voteResults = CsvUtils.ReadVoteResults();

            IEnumerable<LegislatorSupportResult> question1Result = await GetLegislatorSupportsAsync(legislators, voteResults);
            CsvUtils.SaveToCsv(question1Result, QUESTION1_RESULT_FILE);
            // End Question 1.
        }


        // Computes the summary for Question 1
        // given a Dictionary<int,string> where key is LegislatorId and value is LegislatorName 
        // and an IAsyncEnumerable<VoteResults>
        private static async Task<IEnumerable<LegislatorSupportResult>> GetLegislatorSupportsAsync(IDictionary<int, string> legislators, IAsyncEnumerable<VoteResult> voteResults)
        {
            var legDict = new Dictionary<int, LegislatorSupportResult>();

            await foreach (var voteResult in voteResults)
            {
                int legislatorId = voteResult.legislator_id;
                if (legDict.TryGetValue(legislatorId, out LegislatorSupportResult? value))
                {
                    if (voteResult.vote_type == YEA_VOTE)
                        value.num_supported_bills++;
                    else
                        value.num_opposed_bills++;
                }
                else
                {
                    var result = new LegislatorSupportResult
                    {
                        id = legislatorId,
                        name = legislators[legislatorId],
                        num_opposed_bills = voteResult.vote_type == YEA_VOTE ? 0 : 1,
                        num_supported_bills = voteResult.vote_type == YEA_VOTE ? 1 : 0,
                    };
                    legDict.Add(legislatorId, result);
                }
            }
            return legDict.Values;
        }
    }
}
