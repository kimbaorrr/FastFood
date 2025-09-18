using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using System.Collections.Generic;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Services
{
    public class ArticleService : CommonService, IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly string _articleVirtualPath;
        public ArticleService(IWebHostEnvironment webHostEnvironment, IArticleRepository articleRepository, IFileUploadService fileUploadService)
        {
            _articleRepository = articleRepository;
            _fileUploadService = fileUploadService;
            _articleVirtualPath = webHostEnvironment.WebRootPath;
            _articleVirtualPath = Path.Combine(this._articleVirtualPath, "admin_page/uploads/articles");
        }
        public async Task<(bool, string)> NewArticle(NewArticleViewModel newArticleViewModel)
        {
            var articles = await this._articleRepository.GetArticles();

            if (articles.Any(x => x.Title.Equals(newArticleViewModel.Title)))
            {
                return (false, "Tiêu đề bài viết đã tồn tại trên hệ thống !");
            }

            (bool success, string message, string newFilePath) = await this._fileUploadService.ImageUpload(newArticleViewModel.CoverImage, this._articleVirtualPath);

            if (success && !string.IsNullOrEmpty(newFilePath))
            {
                Article newArticle = new()
                {
                    Title = newArticleViewModel.Title,
                    Summary = newArticleViewModel.Summary,
                    Content = newArticleViewModel.Content,
                    CoverImage = newFilePath,
                    AuthorId = newArticleViewModel.AuthorId,
                    CreatedAt = DateTime.Now,
                    IsApproved = false
                };
                await this._articleRepository.AddArticle(newArticle);
                return (true, string.Empty);
            }
            return (false, message);


        }
        public async Task<IPagedList<Article>> GetArticlesPagedList(int page, int size)
        {
            var approvedArticles = await this._articleRepository.GetArticlesByApproved(true);
            return approvedArticles.OrderBy(m => m.ArticleId).ToPagedList(page, size);
        }
        public async Task<(bool, string)> DeleteArticle(int articleId)
        {
            var article = await this._articleRepository.GetArticleById(articleId);

            if (article != null)
            {
                await this._articleRepository.DeleteArticle(article);
                CommonHelper.DeleteFile(article.CoverImage!);
                return (true, $"Xóa bài viết {article.ArticleId} thành công !");
            }

            return (false, $"Không tìm thấy bài viết {articleId} !");
        }
        public async Task<(bool, string)> DeleteArticles(int[] articleIds)
        {
            int totalDeleted = 0;
            foreach (var articleId in articleIds)
            {
                var article = await this._articleRepository.GetArticleById(articleId);

                if (article != null)
                {
                    await this._articleRepository.DeleteArticle(article);
                    CommonHelper.DeleteFile(article.CoverImage!);
                    totalDeleted++;
                }
            }
            return (true, $"Đã xóa thành công {totalDeleted} bài viết !");
        }
        public async Task<(bool, string)> ApproveArticle(int articleId, int approverId, bool isApproved)
        {
            var article = await this._articleRepository.GetArticleById(articleId);
            if (article != null)
            {
                if (!isApproved)
                {
                    CommonHelper.DeleteFile(article.CoverImage!);
                    return (true, $"Bài viết {articleId} đã được rút lại !");
                }

                article.ApproverId = approverId;
                article.ApproveAt = DateTime.Now;
                article.IsApproved = true;
                await this._articleRepository.UpdateArticle(article);
                return (true, $"Bài viết {articleId} đã được xuất bản !");
            }
            return (false, $"Không tìm thấy bài viết {articleId} !");
        }
        public async Task<(bool, string)> ApproveArticles(int[] articleIds, int approverId, bool isApproved)
        {
            List<int> articleNulls = new List<int>();
            foreach(var articleId in articleIds)
            {
                var article = await this._articleRepository.GetArticleById(articleId);
                if (article != null)
                {
                    if (!isApproved)
                    {
                        CommonHelper.DeleteFile(article.CoverImage!);
                        return (true, $"Bài viết {articleId} đã được rút lại !");
                    }

                    article.ApproverId = approverId;
                    article.ApproveAt = DateTime.Now;
                    article.IsApproved = true;
                    await this._articleRepository.UpdateArticle(article);
                    return (true, $"Bài viết {articleId} đã được xuất bản !");
                }
                
            }
            return (false, $"Không tìm thấy các bài viết {string.Join(",", articleNulls)} !");

        }
        public async Task<List<Article>> GetArticlesNotApprove()
        {
            return await this._articleRepository.GetArticlesByApproved(false);
        }
        public async Task<(bool, string)> EditArticle(EditArticleViewModel editArticleViewModel)
        {
            var article = await this._articleRepository.GetArticleById(editArticleViewModel.ArticleId);

            if (article != null)
            {
                article.Title = editArticleViewModel.Title;
                article.Summary = editArticleViewModel.Summary;
                article.Content = editArticleViewModel.Content;
                return (true, $"Sửa bài viết {editArticleViewModel.ArticleId} thành công !");
            }
            return (false, $"Không tìm thấy bài viết {editArticleViewModel.ArticleId} !");
        }
    }
}
