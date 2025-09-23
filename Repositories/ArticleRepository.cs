using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class ArticleRepository : CommonRepository, IArticleRepository
{
    public ArticleRepository(FastFoodEntities fastFoodEntities) : base (fastFoodEntities) { }
    
    public async Task<List<Article>> GetArticles()
    {
        return await this._fastFoodEntities.Articles.ToListAsync();
    }

    public async Task<List<Article>> GetArticlesByApproved(bool isApproved)
    {
        return await this._fastFoodEntities.Articles.Where(x => x.IsApproved == isApproved).ToListAsync();
    }

    public async Task<List<Article>> GetRecentArticles(int excludedArticleId, int take)
    {
        return await this._fastFoodEntities.Articles
            .Where(x=>x.ArticleId != excludedArticleId)
            .OrderByDescending(x=>x.CreatedAt)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Article> GetArticleById(int articleId)
    {
        return await this._fastFoodEntities.Articles
            .Where(x => x.ArticleId == articleId)
            .FirstOrDefaultAsync() ?? new Article();
    }

    public async Task UpdateCoverImage(int articleId, string coverImagePath)
    {
        var article = await this.GetArticleById(articleId);
        article.CoverImage = coverImagePath;
        this._fastFoodEntities.Articles.Update(article);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task AddArticle(Article article)
    {
        await this._fastFoodEntities.AddAsync(article);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task DeleteArticle(Article article)
    {
        this._fastFoodEntities.Remove(article);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateArticle(Article article)
    {
        this._fastFoodEntities.Update(article);
        await this._fastFoodEntities.SaveChangesAsync();
    }


}
