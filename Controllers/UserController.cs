using Api.Data;
using Api.Dtos.Account;
using Api.Dtos.User;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
        [HttpGet("{ID}")]
        public IActionResult GetById([FromRoute] int ID)
        {
            var user = _context.Users.Find(ID);
            if (user == null)
            {
                return NotFound();
            }
     return (Ok(user));
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.KULLANICI_ADI == loginDto.Username && u.Password == loginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var key = _configuration["Jwt:SigningKey"];
            if (string.IsNullOrEmpty(key))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "JWT Key is not configured.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.KULLANICI_ADI),
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }


        [HttpPost]
                public IActionResult Create([FromBody] CreateUserRequestDto userDto)
                {
                    var userModel = userDto.ToUserFromCreateDto();
                    _context.Users.Add(userModel);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetById), new { id = userModel.ID }, userModel.ToUserDto());
                }
        

                [HttpPut]
                [Route("{id}")]
                public IActionResult Update([FromRoute] int id, [FromBody] UpdateUserRequestDto updateDto)
                {
                    var userModel = _context.Users.FirstOrDefault(x => x.ID == id);
                    if (userModel == null)
                    {
                        return NotFound();
                    }
                    userModel.ADI = updateDto.ADI;
                    userModel.SOYADI = updateDto.SOYADI;
                    userModel.KULLANICI_ADI = updateDto.KULLANICI_ADI;
                 
                    userModel.Password = updateDto.SIFRE;

                    _context.SaveChanges();
                    return Ok(userModel);

                }
        
                [HttpDelete]
                [Route("{id}")]
                public IActionResult Delete([FromRoute] int id)
                {
                    var userModel = _context.Users.FirstOrDefault(x => x.ID == id);
                    if (userModel == null)
                    {
                        return NotFound();
                    }

                    _context.Users.Remove(userModel);
                    _context.SaveChanges();
                    return NoContent();
                }

        [HttpPost]
        [Route("createWithToken")]
        public IActionResult CreateWithToken([FromBody] CreateUserRequestDto userDto)
        {
            var userModel = userDto.ToUserFromCreateDto();
            _context.Users.Add(userModel);
            _context.SaveChanges();

            // Token oluşturma
            var token = GenerateToken(userModel.ID.ToString()); // Token'ı oluştur

            return Ok(new
            {
                User = userModel.ToUserDto(),
                Token = token
            });
        }
        [HttpPost]
        [Route("resetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // Veritabanında kullanıcıyı safeword'e göre bul
            var user = _context.Users
                .FirstOrDefault(u => u.KULLANICI_ADI == resetPasswordDto.Username && u.SAFEWORD == resetPasswordDto.Safeword);

            if (user == null)
            {
                return BadRequest("Kullanıcı veya safeword hatalı.");
            }

            // Yeni şifreyi belirle
            user.Password = resetPasswordDto.NewPassword;

            // Şifreyi veritabanına kaydet
            _context.SaveChanges();

            // Yeni token oluştur
            var token = GenerateToken(user.ID.ToString());

            return Ok(new
            {
                Message = "Şifre başarıyla güncellendi.",
                Token = token
            });
        }


        private string GenerateToken(string userId)
        {
            var key = _configuration["Jwt:SigningKey"];
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}