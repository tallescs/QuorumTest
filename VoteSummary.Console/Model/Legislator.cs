namespace VoteSummary.Console.Model
{
    // props names equals to csv columns to simplify code and
    // avoid use of ClassMap<T> for each class
    public class Legislator
    {
        public int id { get; set; }
        public required string name { get; set; }
    }
}
