﻿namespace VoteSummary.Console.Model
{
    public class Bill
    {
        public int id { get; set; }
        public required string title { get; set; }
        public int sponsor_id { get; set; }
    }
}
