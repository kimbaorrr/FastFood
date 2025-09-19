using FastFood.DB;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services
{
    public interface IArticleService
    {
        Task<(bool, string)> NewArticle(NewArticleViewModel newArticleViewModel);
        Task<IPagedList<Article>> GetArticlesPagedList(int page, int size);
        Task<(bool, string)> DeleteArticle(int articleId);
        Task<(bool, string)> DeleteArticles(int[] articleIds);
        Task<(bool, string)> ApproveArticle(int articleId, int? approverId, bool isApproved);
        Task<(bool, string)> ApproveArticles(int[] articleIds, int? approverId, bool isApproved);
        Task<List<Article>> GetArticlesNotApprove();
        Task<(bool, string)> EditArticle(EditArticleViewModel editArticleViewModel);
    }
}