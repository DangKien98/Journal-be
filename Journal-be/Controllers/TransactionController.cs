using Journal_be.EndPointController;
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
                                        t.Id, t.Status, PaymentId = p.Id, p.Method, p.UserId, u.UserName,
                                        UserFirstName = u.FirstName, UserLastName = u.LastName
                                    }).ToList();

                if (transactions.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No transaction is found" }));

                List<object> model = new List<object>();
                foreach (var transaction in transactions)
                {
                    var transactionDetails = (from td in _journalContext.TblTransactionDetails
                                            select new
                                            {
                                                td.Id, td.Name, td.Description, td.Status, td.CreatedTime,
                                                td.TransactionId, td.ArticleId
                                            }).ToList();
                    object value = new { transaction, transactionDetails };
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
                                        t.Id, t.Status, PaymentId = p.Id, p.Method, p.UserId, u.UserName,
                                        UserFirstName = u.FirstName, UserLastName = u.LastName
                                    }).FirstOrDefault();

                if (transaction == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "transaction is not exist" }));


                var transactionDetails = (from td in _journalContext.TblTransactionDetails
                                            select new
                                            {
                                                td.Id, td.Name, td.Description, td.Status, td.CreatedTime,
                                                td.TransactionId, td.ArticleId
                                            }).ToList();
                object model = new { transaction, transactionDetails };

                return await Task.FromResult(Ok(model));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTransaction(TblTransaction transaction)
        {
            try
            {
                _journalContext.TblTransactions.Add(transaction);
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
