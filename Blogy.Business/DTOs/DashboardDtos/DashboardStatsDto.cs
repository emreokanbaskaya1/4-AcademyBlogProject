namespace Blogy.Business.DTOs.DashboardDtos
{
    /// <summary>
    /// Dashboard istatistik verileri
    /// </summary>
    public class DashboardStatsDto
    {
        /// <summary>
        /// Toplam blog sayýsý
        /// </summary>
        public int TotalBlogs { get; set; }

        /// <summary>
        /// Toplam kullanýcý sayýsý
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// Toplam yorum sayýsý
        /// </summary>
        public int TotalComments { get; set; }

        /// <summary>
        /// Toplam mesaj sayýsý
        /// </summary>
        public int TotalMessages { get; set; }

        /// <summary>
        /// Toplam kategori sayýsý
        /// </summary>
        public int TotalCategories { get; set; }

        /// <summary>
        /// Son 30 günde blog yazan aktif yazar sayýsý
        /// </summary>
        public int TotalActiveWriters { get; set; }
    }
}
