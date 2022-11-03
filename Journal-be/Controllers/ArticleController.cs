using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public ArticleController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetByStatus(int status)
        {
            try
            {
                var articles = (from a in _journalContext.TblArticles
                            join u in _journalContext.TblUsers on a.UserId equals u.Id
                            join c in _journalContext.TblCategories on a.CategoryId equals c.Id
                            where a.Status == status
                            select new
                            {
                                a.Id, a.Title, a.CreatedDate, a.Description, a.Status, a.Price, a.ArtFile,
                                a.LastEditedTime, a.CategoryId, c.CategoryName, a.UserId, u.UserName,
                                UserFirstName = u.FirstName, UserLastName = u.LastName, a.StatusPost, a.Comment
                            }).ToList();

                if (articles.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No article is found" }));

                return await Task.FromResult(Ok(articles));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetArticle(int id)
        {
            try
            {
                var article = (from a in _journalContext.TblArticles
                                join u in _journalContext.TblUsers on a.UserId equals u.Id
                                join c in _journalContext.TblCategories on a.CategoryId equals c.Id
                                where a.Id == id
                                select new
                                {
                                    a.Id, a.Title, a.CreatedDate, a.Description, a.Status, a.Price, a.ArtFile,
                                    a.LastEditedTime, a.CategoryId, c.CategoryName, a.UserId, u.UserName,
                                    UserFirstName = u.FirstName, UserLastName = u.LastName, a.StatusPost, a.Comment
                                }).FirstOrDefault();

                if (article == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Article is not exist" }));

                return await Task.FromResult(Ok(article));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreateArticle(TblArticle article)
        {
            try
            {
                article.CreatedDate = DateTime.UtcNow.AddHours(7);
                article.LastEditedTime = DateTime.UtcNow.AddHours(7);
                _journalContext.TblArticles.Add(article);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Create Successful" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateArticle(int id, TblArticle article)
        {
            try
            {
                var articleUpdate = await _journalContext.TblArticles.FindAsync(id);

                if (articleUpdate == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Article is not exist" }));

                articleUpdate.Title = article.Title;
                articleUpdate.Description = article.Description;
                articleUpdate.Status = article.Status;
                articleUpdate.Price = article.Price;
                articleUpdate.UserId = article.UserId;
                articleUpdate.CategoryId = article.CategoryId;
                articleUpdate.ArtFile = article.ArtFile;
                articleUpdate.LastEditedTime = DateTime.UtcNow.AddHours(7);
                articleUpdate.StatusPost = article.StatusPost;
                articleUpdate.Comment = article.Comment;

                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Update Successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
