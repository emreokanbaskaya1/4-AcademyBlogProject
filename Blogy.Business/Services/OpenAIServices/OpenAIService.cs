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

        /// <summary>
        /// AI ile Footer "About Us" text üretir
        /// </summary>
        public async Task<string> GenerateAboutTextAsync(string companyName, string keywords)
        {
            try
            {
                // AI'ya göndereceðimiz talimat (prompt)
                var prompt = $@"Please write a short 'About Us' text for a footer section based on:

Company/Blog Name: {companyName}
Keywords: {keywords}

Requirements:
- Write 2-3 sentences (200-300 characters maximum)
- Make it professional and engaging
- Describe what the company/blog does
- Use proper English grammar
- Do NOT include a title or heading
- Just write the about text directly

Write the about text now:";

                // Chat mesajlarý
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("You are a professional copywriter. Write concise and engaging 'About Us' texts."),
                    new UserChatMessage(prompt)
                };

                // OpenAI API'ye istek gönder
                var chatCompletion = await _chatClient.CompleteChatAsync(
                    messages,
                    new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = 150, // Kýsa metin için yeterli
                        Temperature = 0.7f, // Yaratýcýlýk seviyesi
                        FrequencyPenalty = 0.3f,
                        PresencePenalty = 0.3f
                    }
                );

                // AI'dan gelen metni al
                var generatedText = chatCompletion.Value.Content[0].Text;

                return generatedText?.Trim() ?? "About text could not be generated. Please try again.";
            }
            catch (Exception ex)
            {
                throw new Exception($"AI about text generation error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kullanýcýnýn mesajýnýn dilini algýlar ve AYNI DÝLDE otomatik yanýt üretir
        /// Türkçe, Ýngilizce, Korece, Japonca, vb. tüm dilleri destekler
        /// </summary>
        public async Task<string> GenerateMultilingualAutoReplyAsync(string userMessage)
        {
            try
            {
                // AI'ya gönderilecek talimat (prompt)
                var prompt = $@"A user has sent the following message to our contact form:

""{userMessage}""

Your task:
1. Detect the language of the user's message (it could be any language: Turkish, English, Korean, Japanese, Arabic, Spanish, etc.)
2. Generate a polite auto-reply message in THE SAME LANGUAGE that the user used
3. The auto-reply should say something like: ""Thank you for your message. We have received it successfully and will get back to you soon.""

IMPORTANT RULES:
- You MUST reply in the EXACT SAME LANGUAGE as the user's message
- If the message is in Turkish, reply in Turkish
- If the message is in Korean, reply in Korean
- If the message is in Japanese, reply in Japanese
- Keep it short (1-2 sentences maximum)
- Be professional and friendly
- Do NOT include greetings like ""Dear [Name]"" - just write the confirmation message
- Do NOT translate the user's message - just generate the auto-reply

Generate the auto-reply now:";

                // Chat mesajlarý
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("You are a multilingual customer support assistant. You can detect languages and respond in the same language perfectly."),
                    new UserChatMessage(prompt)
                };

                // OpenAI API'ye istek gönder
                var chatCompletion = await _chatClient.CompleteChatAsync(
                    messages,
                    new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = 100, // Kýsa yanýt için yeterli
                        Temperature = 0.5f, // Daha tutarlý sonuçlar için düþük
                        FrequencyPenalty = 0.0f,
                        PresencePenalty = 0.0f
                    }
                );

                // AI'dan gelen yanýtý al
                var autoReply = chatCompletion.Value.Content[0].Text;

                Console.WriteLine($"[OpenAIService] ? Multilingual auto-reply generated successfully");
                Console.WriteLine($"[OpenAIService] User message: {userMessage.Substring(0, Math.Min(50, userMessage.Length))}...");
                Console.WriteLine($"[OpenAIService] Auto-reply: {autoReply}");

                return autoReply?.Trim() ?? "Your message has been received successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenAIService] ? Multilingual auto-reply error: {ex.Message}");
                throw new Exception($"Multilingual auto-reply generation error: {ex.Message}", ex);
            }
        }
    }
}
