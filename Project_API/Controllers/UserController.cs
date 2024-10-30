using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private  static Project_PRN231Context _context;
        private  static Cloudinary _cloudinary;
        public UserController(Project_PRN231Context context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost("Login")]
        public IActionResult GetUserByLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                
                if (user.Password != password)
                {
                    return BadRequest("Wrong password!");
                }
                return Ok(user);
            }
        }

        [HttpPost("InsertUser")]
        public IActionResult InsertUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var userDb = _context.Users.FirstOrDefault(u => u.Username == user.Username);
                if (userDb == null)
                {
                    userDb = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                    if (userDb == null)
                    {
                        user.RoleId = 2;
                        user.Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgI3iX7tf1HiAnbVKSpyns2b0moiUKSJE2uQ&s";
                        
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        return Ok(user);
                    } else
                    {
                        return NotFound("Email already existed on database!");
                    }
                    
                }
                else
                {
                    return BadRequest("Username already existed on database!");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromForm] UserUpdateDto userUpdateDto)
        {
            
                var userDb = _context.Users.FirstOrDefault(u => u.Username == userUpdateDto.Username);
                if (userDb != null)
                {
                    userDb.Gender = userUpdateDto.Gender == null ? userDb.Gender : userUpdateDto.Gender;
                    userDb.Password = userUpdateDto.Password == null ? userDb.Password : userUpdateDto.Password;
                    if (userUpdateDto.Avatar != null)
                    {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(userUpdateDto.Avatar.FileName, userUpdateDto.Avatar.OpenReadStream()),
                        PublicId = Path.GetFileNameWithoutExtension(userUpdateDto.Avatar.FileName)
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    userDb.Avatar = uploadResult.SecureUri.ToString();
                }

                    _context.Users.Update(userDb);
                    await _context.SaveChangesAsync();
                    return Ok(userDb);


                }
                else
                {
                    return NotFound("Username not existed on database!");
                }
            
            
            
        }
        public class UserUpdateDto
        {
            public string Username { get; set; }
            public string? Password { get; set; }
            public string Email { get; set; }
            public bool Gender { get; set; }
            public IFormFile? Avatar { get; set; } // Nhận file upload
        }
    }

}
