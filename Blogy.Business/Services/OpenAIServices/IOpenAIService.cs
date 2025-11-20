namespace Blogy.Business.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        /// <summary>
        /// Generates a blog article using AI based on the given keywords and short description
        /// </summary>
        /// <param name="keywords">Keywords for the blog (e.g., "ASP.NET Core, MVC, Web Development")</param>
        /// <param name="shortDescription">A short description about the blog topic</param>
        /// <returns>Generated blog article content (approximately 1000 characters)</returns>
        Task<string> GenerateArticleAsync(string keywords, string shortDescription);
    }
}
