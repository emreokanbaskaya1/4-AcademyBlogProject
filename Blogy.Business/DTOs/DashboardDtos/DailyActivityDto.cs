namespace Blogy.Business.DTOs.DashboardDtos
{
    /// <summary>
    /// Günlük aktivite verisi (Line Chart için)
    /// </summary>
    public class DailyActivityDto
    {
        public DateTime Date { get; set; }
        public int BlogCount { get; set; }
        public int CommentCount { get; set; }
    }
}
