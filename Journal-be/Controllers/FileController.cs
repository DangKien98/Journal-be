using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Mvc;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public FileController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFileById(int id)
        {
            try
            {
                var file = _journalContext.TestFiles.Where(x => x.Id == id).FirstOrDefault();
                if (file == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "File is not exist" }));

                var content = new System.IO.MemoryStream(file.FileTest);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "FileDownloaded.pdf");
                using(var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await content.CopyToAsync(fileStream);
                }


                return await Task.FromResult(Ok(new { Status = "Success", Message = "Download file Successful", FilePath = path }));
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
