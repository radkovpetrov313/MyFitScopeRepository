﻿namespace MyFitScope.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MyFitScope.Data.Common.Repositories;
    using MyFitScope.Data.Models.BlogModels;
    using MyFitScope.Data.Models.BlogModels.Enums;
    using MyFitScope.Services.Mapping;
    using MyFitScope.Web.ViewModels.Articles;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;

        public ArticlesService(IDeletableEntityRepository<Article> articlesRepository)
        {
            this.articlesRepository = articlesRepository;
        }

        public async Task<string> CreateArticle(string articleTitle, ArticleCategory articleCategory, string articleImageUrl, string articleContent, string userId)
        {
            var article = new Article
            {
                Title = articleTitle,
                ArticleCategory = articleCategory,
                ImageUrl = articleImageUrl,
                Content = articleContent,
                UserId = userId,
            };

            await this.articlesRepository.AddAsync(article);
            await this.articlesRepository.SaveChangesAsync();

            return article.Id;
        }

        public async Task DeleteArticleAsync(string articleId)
        {
            var articleToDelete = await this.articlesRepository.GetByIdWithDeletedAsync(articleId);

            articleToDelete.IsDeleted = true;

            await this.articlesRepository.SaveChangesAsync();
        }

        public DetailsArticleViewModel GetArticleById(string articleId)
                    => this.articlesRepository.All()
                            .Where(a => a.Id == articleId)
                            .To<DetailsArticleViewModel>()
                            .FirstOrDefault();

        public IEnumerable<ArticleViewModel> GetArticlesByCategory(string articleCategoryInput = null)
        {
            var result = this.articlesRepository.All();

            if (articleCategoryInput != null)
            {
                result = result.Where(a => a.ArticleCategory == (ArticleCategory)Enum.Parse(typeof(ArticleCategory), articleCategoryInput));
            }

            return result.To<ArticleViewModel>().ToList();
        }
    }
}
