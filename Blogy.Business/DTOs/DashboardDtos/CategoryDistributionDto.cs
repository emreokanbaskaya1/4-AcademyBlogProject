namespace Blogy.Business.DTOs.DashboardDtos
{
    /// <summary>
    /// Kategori bazlý blog daðýlýmý (Pie Chart için)
    /// </summary>
    public class CategoryDistributionDto
    {
        public string CategoryName { get; set; }
        public int BlogCount { get; set; }
        public double Percentage { get; set; }
    }
}
