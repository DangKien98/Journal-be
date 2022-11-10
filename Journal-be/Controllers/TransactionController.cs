using Journal_be.EndPointController;
using Journal_be.Entities;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public TransactionController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetTransactions()
        {
            try
            {
                var transactions = (from t in _journalContext.TblTransactions
                                    join p in _journalContext.TblPayments on t.PaymentId equals p.Id
                                    join u in _journalContext.TblUsers on p.UserId equals u.Id
                                    select new
                                    {
                                        t.Id, t.Status, PaymentId = p.Id, t.Description, t.CreatedDate, t.ArticleId, 
                                        p.Method, p.UserId, u.UserName, UserFirstName = u.FirstName, 
                                        UserLastName = u.LastName
                                    }).ToList();

                if (transactions.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No transaction is found" }));

                return await Task.FromResult(Ok(transactions));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetTransactionById(int id)
        {
            try
            {
                var transaction = (from t in _journalContext.TblTransactions
                                    join p in _journalContext.TblPayments on t.PaymentId equals p.Id
                                    join u in _journalContext.TblUsers on p.UserId equals u.Id
                                    where t.Id == id
                                    select new
                                    {
                                        t.Id, t.Status, PaymentId = p.Id, t.Description, t.CreatedDate, t.ArticleId, 
                                        p.Method, p.UserId, u.UserName, UserFirstName = u.FirstName, 
                                        UserLastName = u.LastName
                                    }).FirstOrDefault();

                if (transaction == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "transaction is not exist" }));

                return await Task.FromResult(Ok(transaction));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreateTransaction(TblTransaction transaction)
        {
            try
            {
                _journalContext.TblTransactions.Add(transaction);
                transaction.CreatedDate = DateTime.UtcNow.AddHours(7);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Create Successful" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("check")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetTransactionById(CheckEntity check)
        {
            try
            {
                var transaction = (from t in _journalContext.TblTransactions
                                  join p in _journalContext.TblPayments on t.PaymentId equals p.Id
                                  where t.ArticleId == check.ArticleId && p.UserId == check.UserId
                                  select new { t.ArticleId, p.UserId }).FirstOrDefault();

                if (transaction != null)
                    return await Task.FromResult(Ok(new { Status = "Success", Message = "User bought this article" }));
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
