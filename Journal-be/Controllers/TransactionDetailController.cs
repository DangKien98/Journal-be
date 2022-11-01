using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "transaction-details")]
    [ApiController]
    public class TransactionDetailController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public TransactionDetailController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreateTransactionDetail(TblTransactionDetail transactionDetail)
        {
            try
            {
                transactionDetail.CreatedTime = DateTime.UtcNow.AddHours(7);
                _journalContext.TblTransactionDetails.Add(transactionDetail);
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
