namespace Autopark.API.Entities
{
    public abstract class ReportBase<T>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public ReportPeriod Period { get; set; }
        public required string Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Dictionary<DateTime, T> Result { get; set; }
    }
}