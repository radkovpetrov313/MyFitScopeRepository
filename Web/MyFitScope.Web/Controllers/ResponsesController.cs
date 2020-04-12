﻿namespace MyFitScope.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyFitScope.Data.Models;
    using MyFitScope.Services.Data;

    public class ResponsesController : BaseController
    {
        private readonly IResponsesService responsesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ResponsesController(IResponsesService responsesService, UserManager<ApplicationUser> userManager)
        {
            this.responsesService = responsesService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddResponse(string responseContent, string parentCommentId, string articleId)
        {
            var userId = this.userManager.GetUserId(this.User);

            await this.responsesService.CreateResponseAsync(responseContent, parentCommentId, articleId, userId);

            return this.RedirectToAction("Details", "Articles", new { articleId });
        }

        public async Task<IActionResult> DeleteResponse(string responseId, string articleId)
        {
            await this.responsesService.DeleteResponseAsync(responseId);

            return this.RedirectToAction("Details", "Articles", new { articleId });
        }
    }
}
