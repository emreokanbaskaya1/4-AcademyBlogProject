namespace Blogy.Business.DTOs.OpenAIDtos
{
    public class GenerateArticleRequestDto
    {
        public string Keywords { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
    }
}
