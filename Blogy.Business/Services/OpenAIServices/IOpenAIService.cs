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

        /// <summary>
        /// Generates an "About Us" text for the footer section using AI
        /// </summary>
        /// <param name="companyName">Company or blog name</param>
        /// <param name="keywords">Keywords about the company/blog (e.g., "technology, blog, community")</param>
        /// <returns>Generated about text (approximately 200-300 characters)</returns>
        Task<string> GenerateAboutTextAsync(string companyName, string keywords);

        /// <summary>
        /// Detects the language of user's message and generates an auto-reply in the SAME language
        /// Supports all languages including Korean, Japanese, Turkish, English, etc.
        /// </summary>
        /// <param name="userMessage">The message sent by the user</param>
        /// <returns>Auto-reply message in the same language as the user's message</returns>
        Task<string> GenerateMultilingualAutoReplyAsync(string userMessage);
    }
}
