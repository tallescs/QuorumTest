
namespace VoteSummary.Console.Model
{
    public class BillResult
    {
        public int id {  get; set; }
        public required string title { get; set; }
        public int supporter_count { get; set; } //count of yes votes for the bill
        public int opposer_count { get; set; } // count of no votes for the bill
        public required string primary_sponsor { get; set; } // legislator name
    }
}
