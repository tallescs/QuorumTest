using VoteSummary.Console.Model;

namespace VoteSummary.Console
{
    public class Question2Solver
    {
        /// <summary>
        /// Solves the following question: 
        /// For every bill in the dataset, how many legislators supported the bill? 
        /// How many legislators opposed the bill? Who was the primary sponsor of the bill?
        /// </summary>
        /// <param name="legislators">Dictionary from legislators.csv, key is id and value is name</param>
        /// <param name="voteResults">List from vote_results.csv</param>
        /// <param name="bills">List from bills.csv</param>
        /// <param name="votes">List from votes.csv</param>
        /// <returns>IEnumerable<BillResult</returns>
        public static IEnumerable<BillResult> Solve(
            IDictionary<int, string> legislators,
            IEnumerable<VoteResult> voteResults,
            IEnumerable<Bill> bills,
            IEnumerable<Vote> votes)
        {
            IDictionary<int, int> voteDictionary = votes
                .Select(v => new KeyValuePair<int, int>(v.id, v.bill_id))
                .ToDictionary();

            // create a HashMap(Dictionary in C#) to count votes for every bill
            // faster than search list to count everytime for opposer_count and supporter_count
            var billVoteCount = GetVoteSummaryByBill(voteResults, voteDictionary);

            foreach (var bill in bills)
            {
                var foundLegislator = legislators.TryGetValue(bill.sponsor_id, out var name);
                yield return new BillResult
                {
                    id = bill.id,
                    title = bill.title,
                    primary_sponsor = name ?? string.Empty,
                    opposer_count = billVoteCount[bill.id].OpposerCount,
                    supporter_count = billVoteCount[bill.id].SupporterCount
                };
            }
        }

        private static IDictionary<int, VoteCounter>
            GetVoteSummaryByBill(IEnumerable<VoteResult> voteResults,
            IDictionary<int, int> voteDictionary)
        {
            var voteSummaryByBill = new Dictionary<int, VoteCounter>();

            foreach (var voteResult in voteResults)
            {

                int voteType = voteResult.vote_type;
                int billId = voteDictionary[voteResult.vote_id];

                if (!voteSummaryByBill.ContainsKey(billId))
                {
                    voteSummaryByBill[billId] = new VoteCounter();
                }

                if (voteType == 1) // Supporter
                {
                    voteSummaryByBill[billId].SupporterCount++;
                }
                else if (voteType == 2) // Opposer
                {
                    voteSummaryByBill[billId].OpposerCount++;
                }
            }

            return voteSummaryByBill;
        }
    }
}
