using Journal_be.EndPointController;
using Journal_be.Entities;
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

        [HttpPost("check")]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult> GetTransactionById(CheckEntity check)
        {
            try
            {
                var transactionDetails = (from td in _journalContext.TblTransactionDetails
                                          join t in _journalContext.TblTransactions on td.TransactionId equals t.Id
                                          join p in _journalContext.TblPayments on t.PaymentId equals p.Id
                                          where td.ArticleId == check.ArticleId && p.UserId == check.UserId
                                          select new {td.ArticleId, p.UserId }).FirstOrDefault();
                if (transactionDetails != null)
                    return await Task.FromResult(Ok(new { Status = "Success", Message = "User bought this article"}));
                else
                    return await Task.FromResult(Ok(new { Status = "Fail", Message = "This user did not buy this article" }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
