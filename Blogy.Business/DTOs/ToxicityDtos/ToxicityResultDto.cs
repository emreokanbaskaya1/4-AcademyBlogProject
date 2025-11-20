namespace Blogy.Business.DTOs.ToxicityDtos
{
    /// <summary>
    /// Yorum toxicity analiz sonucu
    /// </summary>
    public class ToxicityResultDto
    {
        /// <summary>
        /// Yorum toksik mi? (toxicity score > 0.5 ise true)
        /// </summary>
        public bool IsToxic { get; set; }

        /// <summary>
        /// Genel toxicity skoru (0.0 - 1.0 arasý)
        /// 0.0 = Hiç toksik deðil
        /// 1.0 = Çok toksik
        /// </summary>
        public double ToxicityScore { get; set; }

        /// <summary>
        /// Tespit edilen toksik kategoriler ve skorlarý
        /// Örnek: { "toxic": 0.85, "insult": 0.62, "threat": 0.12 }
        /// </summary>
        public Dictionary<string, double> Categories { get; set; } = new();

        /// <summary>
        /// Kullanýcýya gösterilecek mesaj
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
