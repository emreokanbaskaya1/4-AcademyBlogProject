using Blogy.Business.Configuration;
using Blogy.Business.DTOs.ToxicityDtos;
using Microsoft.Extensions.Options;
using OpenAI.Moderations;

namespace Blogy.Business.Services.ToxicityServices
{
    public class ToxicityService : IToxicityService
    {
        private readonly OpenAISettings _settings;
        private readonly ModerationClient _moderationClient;
        private static readonly SemaphoreSlim _rateLimitSemaphore = new(1, 1); // Tek seferde 1 istek
        private static DateTime _lastRequestTime = DateTime.MinValue;

        public ToxicityService(IOptions<OpenAISettings> settings)
        {
            _settings = settings.Value;
            
            // OpenAI Moderation Client'ý baþlat
            var openAiClient = new OpenAI.OpenAIClient(_settings.ApiKey);
            _moderationClient = openAiClient.GetModerationClient("omni-moderation-latest");
        }

        /// <summary>
        /// Yorumun toksiklik seviyesini OpenAI Moderation API ile analiz eder
        /// Rate limit korumasý ve otomatik retry ile
        /// </summary>
        public async Task<ToxicityResultDto> AnalyzeCommentAsync(string commentText)
        {
            // 1. Boþ yorum kontrolü
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return new ToxicityResultDto
                {
                    IsToxic = false,
                    ToxicityScore = 0.0,
                    Message = "Comment is empty."
                };
            }

            // Retry parametreleri (daha agresif)
            const int maxRetries = 5;
            const int baseDelayMs = 2000; // 2 saniye baþlangýç

            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    // Rate limit korumasý (concurrent istekleri engelle)
                    await _rateLimitSemaphore.WaitAsync();
                    try
                    {
                        // Son istekten bu yana en az 1 saniye geçmesini bekle
                        var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                        if (timeSinceLastRequest.TotalMilliseconds < 1000)
                        {
                            var waitTime = 1000 - (int)timeSinceLastRequest.TotalMilliseconds;
                            Console.WriteLine($"[ToxicityService] Throttling: waiting {waitTime}ms");
                            await Task.Delay(waitTime);
                        }

                        Console.WriteLine($"[ToxicityService] Analyzing comment (Attempt {attempt + 1}/{maxRetries + 1}): {commentText.Substring(0, Math.Min(50, commentText.Length))}...");

                        // API çaðrýsý
                        var moderationResult = await _moderationClient.ClassifyTextAsync(commentText);
                        _lastRequestTime = DateTime.UtcNow;

                        // Baþarýlý - sonuçlarý iþle
                        var result = moderationResult.Value;
                        
                        var categories = new Dictionary<string, double>();
                        double maxScore = 0.0;

                        // Kategorileri topla
                        if (result.Hate.Flagged)
                        {
                            var score = result.Hate.Score;
                            categories["hate"] = Math.Round(score, 2);
                            if (score > maxScore) maxScore = score;
                        }

                        if (result.Harassment.Flagged)
                        {
                            var score = result.Harassment.Score;
                            categories["harassment"] = Math.Round(score, 2);
                            if (score > maxScore) maxScore = score;
                        }

                        if (result.SelfHarm.Flagged)
                        {
                            var score = result.SelfHarm.Score;
                            categories["self_harm"] = Math.Round(score, 2);
                            if (score > maxScore) maxScore = score;
                        }

                        if (result.Sexual.Flagged)
                        {
                            var score = result.Sexual.Score;
                            categories["sexual"] = Math.Round(score, 2);
                            if (score > maxScore) maxScore = score;
                        }

                        if (result.Violence.Flagged)
                        {
                            var score = result.Violence.Score;
                            categories["violence"] = Math.Round(score, 2);
                            if (score > maxScore) maxScore = score;
                        }

                        bool isToxic = result.Flagged;

                        if (!isToxic && maxScore > 0)
                        {
                            const double toxicityThreshold = 0.3;
                            isToxic = maxScore > toxicityThreshold;
                        }

                        Console.WriteLine($"[ToxicityService] ? Success - IsToxic: {isToxic}, Score: {maxScore:F2}");

                        return new ToxicityResultDto
                        {
                            IsToxic = isToxic,
                            ToxicityScore = Math.Round(maxScore, 2),
                            Categories = categories,
                            Message = isToxic 
                                ? $"Your comment contains inappropriate content (score: {maxScore:P0}). Please revise it."
                                : "Your comment is acceptable."
                        };
                    }
                    finally
                    {
                        _rateLimitSemaphore.Release();
                    }
                }
                catch (Exception ex) when (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests"))
                {
                    // Rate limit hatasý
                    _lastRequestTime = DateTime.UtcNow; // Son istek zamanýný güncelle
                    
                    if (attempt < maxRetries)
                    {
                        // Exponential backoff: 2s, 4s, 8s, 16s, 32s
                        int delayMs = baseDelayMs * (int)Math.Pow(2, attempt);
                        Console.WriteLine($"[ToxicityService] ?? Rate limit (429). Waiting {delayMs / 1000}s before retry {attempt + 2}/{maxRetries + 1}...");
                        await Task.Delay(delayMs);
                        continue; // Retry
                    }
                    else
                    {
                        // Tüm denemeler tükendi
                        Console.WriteLine($"[ToxicityService] ? Rate limit exceeded after {maxRetries + 1} attempts.");
                        throw new Exception(
                            "? OpenAI Moderation API rate limit exceeded. " +
                            "This feature is temporarily unavailable. " +
                            "The comment will be posted without toxicity check.", 
                            ex
                        );
                    }
                }
                catch (Exception ex)
                {
                    // Diðer hatalar (retry yapma)
                    Console.WriteLine($"[ToxicityService] ? Error: {ex.Message}");
                    throw new Exception($"Toxicity analysis error: {ex.Message}", ex);
                }
            }

            // Bu noktaya hiç ulaþmamalý
            throw new Exception("Unexpected error in toxicity analysis.");
        }
    }
}
