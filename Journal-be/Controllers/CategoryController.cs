using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<TblCategory>>> GetAll()
        {
            try
            {
                var categories = _journalContext.TblCategories.ToList();
                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TblCategory>> GetCategory(int id)
        {
            var category = await _journalContext.TblCategories.FindAsync(id);
            if (category == null)
                return NotFound("Category is not exist");

            return Ok(category);
        }

        [HttpPost]
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
