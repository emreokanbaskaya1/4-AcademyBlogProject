using Blogy.Business.DTOs.ToxicityDtos;

namespace Blogy.Business.Services.ToxicityServices
{
    /// <summary>
    /// Yorum toxicity analiz servisi
    /// </summary>
    public interface IToxicityService
    {
        /// <summary>
        /// Yorumun toksiklik seviyesini analiz eder (Hugging Face Toxic-BERT ile)
        /// </summary>
        /// <param name="commentText">Analiz edilecek yorum metni</param>
        /// <returns>Toxicity analiz sonucu</returns>
        Task<ToxicityResultDto> AnalyzeCommentAsync(string commentText);
    }
}
