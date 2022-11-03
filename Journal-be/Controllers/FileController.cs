using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly JournalContext _journalContext;

        public FileController(JournalContext journalContext, IWebHostEnvironment hostEnvironment)
        {
            _journalContext = journalContext;
            environment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFileById(int id)
        {
            try
            {
                var file = _journalContext.TestFiles.Where(x => x.Id == id).FirstOrDefault();
                var memory = new MemoryStream();
                var content = new System.IO.MemoryStream(file.FileTest);
                await content.CopyToAsync(memory);
                memory.Position = 0;

                return File(memory, "application/octet-stream", "test.pdf");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateCategory(IFormFile fileData)
        {
            try
            {
                var fileDetails = new TestFile();
                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileTest = stream.ToArray();
                }
                _journalContext.TestFiles.Add(fileDetails);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Create Successful" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
