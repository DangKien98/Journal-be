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
        public async Task<ActionResult> Login(Login login)
        {
            var user = (from u in _journalContext.TblUsers
                        join r in _journalContext.TblRoles on u.RoleId equals r.Id
                        where u.UserName == login.UserName
                        select new UserEntity
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Password = u.Password,
                            Email = u.Email,
                            Phone = u.Phone,
                            CreatedTime = u.CreatedTime,
                            Address = u.Address,
                            Status = u.Status,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Image = u.Image,
                            Role = r.RoleName
                        }).FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return await Task.FromResult(NotFound(new { Status = "Fail", Message = "Username or Password is incorrect" }));

            TokenJwt tokenJwt = new TokenJwt(_configuration);
            var tokenString = tokenJwt.GenerateJwtToken(user);

            return await Task.FromResult(Ok(new {Status = "Success", Message = "Login Successful", tokenString}));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var users = (from u in _journalContext.TblUsers
                            join r in _journalContext.TblRoles on u.RoleId equals r.Id
                            where u.Status == 1
                            select new UserEntity
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                Email = u.Email,
                                Phone = u.Phone,
                                CreatedTime = u.CreatedTime,
                                Address = u.Address,
                                Status = u.Status,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Image = u.Image,
                                Role = r.RoleName
                            }).ToList();

                return await Task.FromResult(Ok(users));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var user = (from u in _journalContext.TblUsers
                            join r in _journalContext.TblRoles on u.RoleId equals r.Id
                            where u.Id == id
                            select new UserEntity
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                Password = u.Password,
                                Email = u.Email,
                                Phone = u.Phone,
                                CreatedTime = u.CreatedTime,
                                Address = u.Address,
                                Status = u.Status,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Image = u.Image,
                                Role = r.RoleName
                            }).FirstOrDefault();

                if (user == null)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "User is not exist" }));

                return await Task.FromResult(Ok(user));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("role/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetUserByRoleId(int id)
        {
            try
            {
                var users = (from u in _journalContext.TblUsers
                             join r in _journalContext.TblRoles on u.RoleId equals r.Id
                             where u.Status == 1 && u.RoleId == id
                             select new UserEntity
                             {
                                 Id = u.Id,
                                 UserName = u.UserName,
                                 Email = u.Email,
                                 Phone = u.Phone,
                                 CreatedTime = u.CreatedTime,
                                 Address = u.Address,
                                 Status = u.Status,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Image = u.Image,
                                 Role = r.RoleName
                             }).ToList();

                if (users.Count == 0)
                    return await Task.FromResult(NotFound(new { Status = "Fail", Message = "No user is found" }));

                return await Task.FromResult(Ok(users));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(TblUser user)
        {
            if (_journalContext.TblUsers.Any(u => u.UserName == user.UserName))
                return await Task.FromResult(BadRequest(new { Status = "Fail", Message = "Username is exist" }));
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreatedTime = DateTime.UtcNow.AddHours(7);
                _journalContext.TblUsers.Add(user);
                await _journalContext.SaveChangesAsync();

                return Ok(new {Status = "Success", Message = "Register Successful"});
            }
            catch (Exception ex)
            {
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
                    return await Task.FromResult(BadRequest(new { Status = "Fail", Message = "User is not exist" }));

                userUpdate.Email = user.Email;
                userUpdate.Phone = user.Phone;
                userUpdate.Address = user.Address;
                userUpdate.Status = user.Status;
                userUpdate.FirstName = user.FirstName;
                userUpdate.LastName = user.LastName;
                userUpdate.Image = user.Image;

                await _journalContext.SaveChangesAsync();

                return Ok(new { Status = "Success", Message = "Update Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
