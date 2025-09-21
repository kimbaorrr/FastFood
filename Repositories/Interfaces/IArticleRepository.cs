using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticles();
        Task<List<Article>> GetArticlesByApproved(bool isApproved);
        Task<List<Article>> GetRecentArticles(int articleId, int take);
        Task UpdateCoverImage(int articleId, string coverImagePath);
        Task<Article> GetArticleById(int articleId);
        Task AddArticle(Article article);
        Task DeleteArticle(Article article);
        Task UpdateArticle(Article article);
    }
}
