namespace VoteSummary.Console.Model
{
    public class LegislatorSupportResult
    {
        public int id { get; set; }
        public required string name { get; set; }
        public int num_supported_bills { get; set; }
        public int num_opposed_bills { get; set; }
    }
}
