using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<TblArticle>>> GetAll(int status)
        {
            try
            {
                string query = "SELECT a.Id, a.Title, a.CreatedDate, a.Description, a.AuthorName, a.Status, a.Price, a.UserId, u.UserName, u.FirstName AS UserFirstName, u.LastName AS UserLastName, a.CategoryID, c.CategoryName, a.Image, a.LastEditedTime\r\n" +
                    "FROM tblArticle AS a\r\n" +
                    "LEFT JOIN tblUser AS u ON a.UserId = u.Id\r\n" +
                    "LEFT JOIN tblCategory as c ON a.CategoryID = c.Id\r\n" +
                    "WHERE a.Status = @Status";
                var p1 = new SqlParameter("@Status", status);
                var articles = _journalContext.ArticleEntities.FromSqlRaw(query, p1).ToList();
                return Ok(articles);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<TblArticle>> GetArticle(int id)
        {
            try
            {
                string query = "SELECT a.Id, a.Title, a.CreatedDate, a.Description, a.AuthorName, a.Status, a.Price, a.UserId, u.UserName, u.FirstName AS UserFirstName, u.LastName AS UserLastName, a.CategoryID, c.CategoryName, a.Image, a.LastEditedTime\r\n" +
                    "FROM tblArticle AS a\r\n" +
                    "LEFT JOIN tblUser AS u ON a.UserId = u.Id\r\n" +
                    "LEFT JOIN tblCategory as c ON a.CategoryID = c.Id\r\n" +
                    "WHERE a.Id = @Id";
                var p1 = new SqlParameter("@Id", id);
                var artcle = _journalContext.ArticleEntities.FromSqlRaw(query, p1).FirstOrDefault();
                if (artcle == null)
                    return NotFound("Article is not exist");

                return Ok(artcle);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<TblArticle>> CreateCategory(TblArticle article)
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
                    return NotFound("article is not exist");

                articleUpdate.Title = article.Title;
                articleUpdate.Description = article.Description;
                articleUpdate.AuthorName = article.AuthorName;
                articleUpdate.Status = article.Status;
                articleUpdate.Price = article.Price;
                articleUpdate.UserId = article.UserId;
                articleUpdate.CategoryId = article.CategoryId;
                articleUpdate.ArtFile = article.ArtFile;
                articleUpdate.LastEditedTime = DateTime.UtcNow.AddHours(7);

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
