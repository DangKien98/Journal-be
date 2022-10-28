using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        public async Task<ActionResult<IEnumerable<TestFile>>> GetAll()
        {
            try
            {
                var files = _journalContext.TestFiles.ToList();
                return Ok(files);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TestFile>> CreateCategory(TestFile testFile)
        {

            try
            {
/*                TestFile testFile = new TestFile();
                string filePath = @"D:\testpdf.pdf";
                string filename = Path.GetFileName(filePath);
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((Int32)fs.Length);
                testFile.Filene = bytes;
                br.Close();
                fs.Close();*/

                _journalContext.TestFiles.Add(testFile);
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
