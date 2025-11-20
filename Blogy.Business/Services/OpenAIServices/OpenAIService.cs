using Blogy.Business.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.ClientModel;

namespace Blogy.Business.Services.OpenAIServices
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAISettings _settings;
        private readonly ChatClient _chatClient;

        public OpenAIService(IOptions<OpenAISettings> settings)
        {
            _settings = settings.Value;
            
            // OpenAI ChatClient'ý baþlatýyoruz (doðrudan OpenAI API ile)
            var openAiClient = new OpenAI.OpenAIClient(_settings.ApiKey);
            _chatClient = openAiClient.GetChatClient(_settings.Model);
        }

        public async Task<string> GenerateArticleAsync(string keywords, string shortDescription)
        {
            try
            {
                // Prompt (AI'ya gönderilecek talimat) - ÝNGÝLÝZCE
                var prompt = $@"Please write a blog article based on the following information:

Keywords: {keywords}
Short Description: {shortDescription}

Requirements:
- The article must be approximately 1000 characters long
- Write in proper English grammar
- Make it SEO-friendly and informative
- Do NOT include a title, only write the article content
- Structure the content in paragraphs
- Use a professional and fluent writing style
- Make it engaging and readable

Write the article now:";

                // Chat mesajlarý oluþturuyoruz
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("You are a professional blog writer. You write articles in English."),
                    new UserChatMessage(prompt)
                };

                // API'ye istek gönderiyoruz
                var chatCompletion = await _chatClient.CompleteChatAsync(
                    messages,
                    new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = _settings.MaxTokens, // 500 token
                        Temperature = 0.7f, // Creativity level
                        FrequencyPenalty = 0.3f, // Reduce repetition
                        PresencePenalty = 0.3f // Topic diversity
                    }
                );

                // AI'dan gelen cevabý alýyoruz
                var generatedContent = chatCompletion.Value.Content[0].Text;

                // Ýçeriði döndürüyoruz
                return generatedContent?.Trim() ?? "Article could not be generated. Please try again.";
            }
            catch (Exception ex)
            {
                // Hata durumunda detaylý mesaj döndürüyoruz
                throw new Exception($"AI article generation error: {ex.Message}", ex);
            }
        }
    }
}
