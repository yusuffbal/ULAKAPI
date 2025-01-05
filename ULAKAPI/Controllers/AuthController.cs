using Business.Abstract;
using Dataaccess.Abstract;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ULAKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsersDal _userDal;


        public AuthController(IAuthService authService, IUsersDal usersDal)
        {
            _authService = authService;
            _userDal = usersDal;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto user)
        {
            try
            {

                var token = _authService.Register(user, user.Password);
                return Ok(new { token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var token = _authService.Login(login.Email, login.Password);
            if (token == null)
            {
                return Unauthorized("Geçersiz kimlik bilgisi.");
            }

            return Ok(new { token });
        }

        [HttpGet("userdata")]
        [Authorize]  
        public async Task<IActionResult> GetUserData()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Kullanıcı ID'si bulunamadı.");
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Geçersiz kullanıcı ID'si.");
            }

            var user =  _userDal.GetFirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new
            {
                user.UserId,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }
    }
}

