using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var categories = _journalContext.TblCategories.ToList();
                if (categories.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No category is found" }));

                List<object> model = new List<object>();
                foreach (var category in categories)
                {
                    var articles = (from a in _journalContext.TblArticles
                                    join u in _journalContext.TblUsers on a.UserId equals u.Id
                                    join c in _journalContext.TblCategories on a.CategoryId equals c.Id
                                    where a.CategoryId == category.Id
                                    select new
                                    {
                                        a.Id, a.Title, a.CreatedDate, a.Description, a.Status, a.Price, a.ArtFile,
                                        a.LastEditedTime, a.CategoryId, c.CategoryName, a.UserId, u.UserName,
                                        UserFirstName = u.FirstName, UserLastName = u.LastName
                                    }).ToList();

                    object value = new { category = category, articles = articles };
                    model.Add(value);
                }

                return await Task.FromResult(Ok(model));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _journalContext.TblCategories.FindAsync(id);

            if (category == null)
                return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Category is not exist" }));

            var articles = (from a in _journalContext.TblArticles
                            join u in _journalContext.TblUsers on a.UserId equals u.Id
                            join c in _journalContext.TblCategories on a.CategoryId equals c.Id
                            where a.CategoryId == id
                            select new
                            {
                                a.Id, a.Title, a.CreatedDate, a.Description, a.Status, a.Price, a.ArtFile,
                                a.LastEditedTime, a.CategoryId, c.CategoryName, a.UserId, u.UserName,
                                UserFirstName = u.FirstName, UserLastName = u.LastName
                            }).ToList();

            object model = new { category = category, articles = articles };

            return await Task.FromResult(Ok(model));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateCategory(TblCategory category)
        {
            try
            {
                _journalContext.TblCategories.Add(category);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Create Successful" });
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
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Category is not exist" }));

                categoryUpdate.CategoryName = category.CategoryName;

                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Update Successful" });
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
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Category is not exist" }));

                _journalContext.TblCategories.Remove(category);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
