using Journal_be.EndPointController;
using Journal_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly JournalContext _journalContext;

        public PaymentController(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetPayments()
        {
            try
            {
                var payments = (from p in _journalContext.TblPayments
                            join u in _journalContext.TblUsers on p.UserId equals u.Id
                            select new
                            {
                                p.Id, p.Method, p.Status, p.UserId, u.UserName, UserFirstName = u.FirstName,
                                UserLastName = u.LastName
                            }).ToList();

                if (payments.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No payment is found" }));

                return await Task.FromResult(Ok(payments));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetPaymentById(int id)
        {
            try
            {
                var payments = (from p in _journalContext.TblPayments
                            join u in _journalContext.TblUsers on p.UserId equals u.Id
                            where p.Id == id
                            select new
                            {
                                p.Id, p.Method, p.Status, p.UserId, u.UserName, UserFirstName = u.FirstName,
                                UserLastName = u.LastName
                            }).FirstOrDefault();

                if (payments == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Payment is not exist" }));

                return await Task.FromResult(Ok(payments));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreatePayment(TblPayment payment)
        {
            try
            {
                _journalContext.TblPayments.Add(payment);
                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Create Successful" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateArticle(int id, TblPayment payment)
        {
            try
            {
                var paymentUpdate = await _journalContext.TblPayments.FindAsync(id);

                if (paymentUpdate == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Payment is not exist" }));

                paymentUpdate.Method = payment.Method;
                paymentUpdate.Status = payment.Status;
                paymentUpdate.UserId = payment.UserId;

                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Update Successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
