namespace Blogy.Business.Configuration
{
    public class OpenAISettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-4-turbo-preview";
        public int MaxTokens { get; set; } = 500;
    }
}
