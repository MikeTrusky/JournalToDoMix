namespace JournalToDoMix.ViewModels
{
    public class StatisticsViewModel
    {
        public string? Label { get; set; }
        public double Count { get; set; }
        public ChartType Type { get; set; }
    }    

    public enum ChartType
    {
        ActivityCount,
        ActivityTime
    }
}
