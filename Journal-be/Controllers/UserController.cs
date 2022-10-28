using Journal_be.EndPointController;
using Journal_be.Entities;
using Journal_be.Models;
using Journal_be.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            string query = "SELECT u.Id, u.UserName, u.Email, u.Phone, u.CreatedTime, u.Address, u.RoleId, r.RoleName AS Role, u.Status, u.FirstName, u.LastName, u.Image\r\n" +
                    "FROM tblUser AS u\r\n" +
                    "LEFT JOIN tblRole AS r ON u.RoleId = r.Id\r\n" +
                    "WHERE u.Status = 1 AND u.Id = @Id";
            var userLogin = _journalContext.TblUsers.SingleOrDefault(u => u.UserName == login.UserName);
            if (userLogin == null || !BCrypt.Net.BCrypt.Verify(login.Password, userLogin.Password))
                return StatusCode(401, "Username or Password is incorrect");
            var p1 = new SqlParameter("@Id", userLogin.Id);
            var user = _journalContext.UserEntities.FromSqlRaw(query, p1).SingleOrDefault();

            TokenJwt tokenJwt = new TokenJwt(_configuration);
            var tokenString = tokenJwt.GenerateJwtToken(user);

            return Ok(new { tokenString });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TblUser>>> GetAll()
        {
            try
            {
                string query = "SELECT u.Id, u.UserName, u.Email, u.Phone, u.CreatedTime, u.Address, u.RoleId, r.RoleName AS Role, u.Status, u.FirstName, u.LastName, u.Image\r\n" +
                    "FROM tblUser AS u\r\n" +
                    "LEFT JOIN tblRole AS r ON u.RoleId = r.Id\r\n" + 
                    "WHERE u.Status = 1";
                var users = _journalContext.UserEntities.FromSqlRaw(query);
                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<TblUser>> GetUser(int id)
        {
            try
            {
                string query = "SELECT u.Id, u.UserName, u.Email, u.Phone, u.CreatedTime, u.Address, u.RoleId, r.RoleName AS Role, u.Status, u.FirstName, u.LastName, u.Image\r\n" +
                    "FROM tblUser AS u\r\n" +
                    "LEFT JOIN tblRole AS r ON u.RoleId = r.Id\r\n" +
                    "WHERE u.Status = 1 AND u.Id = @Id";
                var p1 = new SqlParameter("@Id", id);
                var user = _journalContext.UserEntities.FromSqlRaw(query, p1).FirstOrDefault();
                if (user == null)
                    return NotFound("User is not exist");

                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("role/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<TblUser>> GetUserByRoleId(int id)
        {
            try
            {
                string query = "SELECT u.Id, u.UserName, u.Email, u.Phone, u.CreatedTime, u.Address, u.RoleId, r.RoleName AS Role, u.Status, u.FirstName, u.LastName, u.Image\r\n" +
                    "FROM tblUser AS u\r\n" +
                    "LEFT JOIN tblRole AS r ON u.RoleId = r.Id\r\n" +
                    "WHERE u.Status = 1 AND u.RoleId = @Id";
                var p1 = new SqlParameter("@Id", id);
                var users = _journalContext.UserEntities.FromSqlRaw(query, p1).ToList();
                if (users == null)
                    return NotFound("User is not exist");

                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TblUser>> AddCustomer(TblUser user)
        {
            if (_journalContext.TblUsers.Any(u => u.UserName == user.UserName))
                return BadRequest("Username is duplicate");
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreatedTime = DateTime.UtcNow.AddHours(7);
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

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateUser(int id, TblUser user)
        {
            try
            {
                var userUpdate = await _journalContext.TblUsers.FindAsync(id);
                if (userUpdate == null)
                    return NotFound("User is not exist");

                userUpdate.Email = user.Email;
                userUpdate.Phone = user.Phone;
                userUpdate.Address = user.Address;
                userUpdate.Status = user.Status;
                userUpdate.FirstName = user.FirstName;
                userUpdate.LastName = user.LastName;
                userUpdate.Image = user.Image;

                await _journalContext.SaveChangesAsync();

                return StatusCode(200, "Update Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
