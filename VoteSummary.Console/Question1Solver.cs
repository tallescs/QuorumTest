using VoteSummary.Console.Model;

namespace VoteSummary.Console
{
    public class Question1Solver
    {
        private static readonly int YEA_VOTE = 1;

        /// <summary>
        ///  Solvs the following questions: 
        ///  For every legislatorin the dataset, how many bills did the legislator support 
        ///  (voted for the bill)? How many bills did the legislator oppose?
        /// </summary>
        /// <param name="legislators">Dictionary containing the Legislators from legislators.csv</param>
        /// <param name="voteResults">List containing the VoteResults from vote_results.csv </param>
        /// <returns> IEnumerable<LegislatorSupportResult></returns>
        public static IEnumerable<LegislatorSupportResult> Solve(
            IDictionary<int, string> legislators,
            IEnumerable<VoteResult> voteResults)
        {
            var legDict = new Dictionary<int, LegislatorSupportResult>();

            foreach (var voteResult in voteResults)
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
