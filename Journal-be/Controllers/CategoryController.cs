using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public CategoryController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<TblCategory>>> GetAll()
        {
            try
            {
                var categories = _journalContext.TblCategories.ToList();
                if (categories == null)
                    return NotFound(new { Status = "Failed", Message = "Can not find any Category" });
                List<object> listArticles = new List<object>();
                foreach (var category in categories)
                {
                    string query = "SELECT a.Id, a.Titile, a.CreatedTime, a.Description, a.AuthorName, a.Status, a.Price, a.UserId, u.UserName, u.FirstName AS UserFirstName, u.LastName AS UserLastName, a.CategoryID, c.CategoryName, a.Image, a.LastEditedTime\r\n" +
                    "FROM tblArticle AS a\r\n" +
                    "LEFT JOIN tblUser AS u ON a.UserId = u.Id\r\n" +
                    "LEFT JOIN tblCategory as c ON a.CategoryID = c.Id\r\n" +
                    "WHERE a.CategoryID = @Id";
                    var p1 = new SqlParameter("@Id", category.Id);
                    var articles = _journalContext.ArticleEntities.FromSqlRaw(query, p1).ToList();
                    object value = new { category = category, articles = articles };
                    listArticles.Add(value);
                }

                return Ok(listArticles);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<TblCategory>> GetCategory(int id)
        {
            var category = await _journalContext.TblCategories.FindAsync(id);
            if (category == null)
                return NotFound(new { Status = "Failed", Message = "Category is not exist" });
            string query = "SELECT a.Id, a.Titile, a.CreatedTime, a.Description, a.AuthorName, a.Status, a.Price, a.UserId, u.UserName, u.FirstName AS UserFirstName, u.LastName AS UserLastName, a.CategoryID, c.CategoryName, a.Image, a.LastEditedTime\r\n" +
            "FROM tblArticle AS a\r\n" +
            "LEFT JOIN tblUser AS u ON a.UserId = u.Id\r\n" +
            "LEFT JOIN tblCategory as c ON a.CategoryID = c.Id\r\n" +
            "WHERE a.CategoryID = @Id";
            var p1 = new SqlParameter("@Id", category.Id);
            var articles = _journalContext.ArticleEntities.FromSqlRaw(query, p1).ToList();
            object value = new { category = category, articles = articles };

            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TblCategory>> CreateCategory(TblCategory category)
        {
            try
            {
                _journalContext.TblCategories.Add(category);
                await _journalContext.SaveChangesAsync();

                return Ok(CreatedAtAction("GetCategory", new { id = category.Id }, category));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCategory(int id, TblCategory category)
        {
            try
            {
                var categoryUpdate = await _journalContext.TblCategories.FindAsync(id);
                if (categoryUpdate == null)
                    return NotFound("Category is not exist");

                categoryUpdate.CategoryName = category.CategoryName;


                await _journalContext.SaveChangesAsync();

                return StatusCode(200, "Update Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TblCategory>> DeleteCategory(int id)
        {
            try
            {
                var category = await _journalContext.TblCategories.FindAsync(id);
                if (category == null)
                    return NotFound("Category is not exist");

                _journalContext.TblCategories.Remove(category);
                await _journalContext.SaveChangesAsync();

                return StatusCode(200, "Delete Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
