using System;

namespace TestBM.Model
{
    public class Cost
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string CostInfo { get; set; }
        public double CostDetail { get; set; }
        public DateTime CostDateTime { get; set; }
    }
}
