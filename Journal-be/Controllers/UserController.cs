using Journal_be.EndPointController;
using Journal_be.Models;
using Journal_be.Security;
using Microsoft.AspNetCore.Mvc;

namespace Journal_be.Controllers
{
    [Route(EndPoint.Prefix + EndPoint.Version + "users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JournalContext _journalContext;
        private readonly IConfiguration _configuration;

        public UserController(JournalContext journalContext, IConfiguration config)
        {
            _journalContext = journalContext;
            _configuration = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = _journalContext.TblUsers.SingleOrDefault(u => u.UserName == login.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return StatusCode(401, "Username or Password is incorrect");

            TokenJwt tokenJwt = new TokenJwt(_configuration);
            var tokenString = tokenJwt.GenerateJwtToken(user);

            return Ok(new { tokenString });
        }

        [HttpPost]
        public async Task<ActionResult<TblUser>> AddCustomer(TblUser user)
        {
            if (_journalContext.TblUsers.Any(u => u.UserName == user.UserName))
                return BadRequest("Username is duplicate");
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreateTimed = DateTime.UtcNow.AddHours(7);
                _journalContext.TblUsers.Add(user);
                await _journalContext.SaveChangesAsync();

                return Ok(CreatedAtAction("CreateUser", new { id = user.Id }, user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
