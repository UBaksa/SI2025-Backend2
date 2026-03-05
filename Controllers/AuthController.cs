using carGooBackend.Repositories;
using carGooBackend.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using carGooBackend.Models;
using carGooBackend.Data;
using carGooBackend.Services;
using Microsoft.AspNetCore.Authentication;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Korisnik> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly CarGooDataContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ImageUploadService _imageUploadService;

        public AuthController(UserManager<Korisnik> userManager, ITokenRepository tokenRepository, CarGooDataContext context, IEmailService emailService, IConfiguration configuration, ImageUploadService imageUploadService)
        {
            this.userManager = userManager;
            _context = context;
            this.tokenRepository = tokenRepository;
            this._emailService = emailService;
            this._configuration = configuration;
            _imageUploadService = imageUploadService;
        }
        // POST api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDTO registerRequestDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Mail);
            var preduzeces = await _context.Preduzeca.ToListAsync();

            if (existingUser != null)
            {
                return BadRequest(new { Message = $"Email '{registerRequestDto.Mail}' je već zauzet." });
            }
            string imgUrl = null;

            if (registerRequestDto.Image != null)
            {
                var result = await _imageUploadService.UploadImageAsync(registerRequestDto.Image);
                if (!result.Success)
                    return BadRequest(result.ErrorMessage);

                imgUrl = result.Url;
            }
            else
            {
                imgUrl = "https://res.cloudinary.com/djhncrqvne/image/upload/v1731345613/cargoo_users/default-avatar.png";
            }


            var identityUser = new Korisnik
            {
                UserName = registerRequestDto.Mail,
                Email = registerRequestDto.Mail,
                FirstName = registerRequestDto.FirstName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                LastName = registerRequestDto.LastName,
                PreduzeceId = registerRequestDto.PreduzeceId,
                Languages = registerRequestDto.Languages ?? new List<string>(),
                EmailConfirmed = false,
                UserPicture = imgUrl,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                // Generate email confirmation token
                var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                // Create confirmation link
                var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = identityUser.Id, token = token },
                    protocol: HttpContext.Request.Scheme);

                // Send confirmation email
                var message = $@"
                <h2>Welcome to {identityUser.FirstName}!</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <p><a href='{confirmationLink}'>Confirm Email</a></p>
                <p>If you didn't register for this account, please ignore this email.</p>
                <p>Your Password is Pass12345,please change password with first logining, for your own privacy.</p>";

                await _emailService.SendEmailAsync(identityUser.Email, "Confirm your email", message);

                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                }

                return Ok(new
                {
                    Message = "Registration successful! Please check your email to confirm your account.",
                    Id = identityUser.Id
                });
            }

            return BadRequest(new { Message = "Registration failed!", Errors = identityResult.Errors });
        }
        //POST:LOGIN
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.EmailAddress);
            if (user == null)
            {
                return BadRequest(new { Message = "Uneti mail ne postoji" });
            }

            // Check email verification BEFORE checking password
            if (!user.EmailConfirmed)
            {
                // Generate new confirmation token and link
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = user.Id, token = token },
                    protocol: HttpContext.Request.Scheme);

                // Resend confirmation email
                var message = $@"
        <h2>Welcome to {user.FirstName}!</h2>
        <p>Please confirm your email by clicking the link below:</p>
        <p><a href='{confirmationLink}'>Confirm Email</a></p>
        <p>If you didn't register for this account, please ignore this email.</p>";

                await _emailService.SendEmailAsync(user.Email, "Confirm your email", message);

                return Unauthorized(new
                {
                    Message = "Email nije potvrđen. Novi link za potvrdu je poslat na vašu email adresu.",
                    RequiresEmailConfirmation = true
                });
            }

            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!checkPasswordResult)
            {
                return BadRequest(new { Message = "Pogrešna lozinka" });
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null)
            {
                return BadRequest(new { Message = "Greška pri dobijanju korisničkih uloga" });
            }

            var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
            var response = new LoginResponseDTO
            {
                JwtToken = jwtToken,
                UserId = user.Id,
                CompanyId = user.PreduzeceId,
                Roles = roles.ToList()
            };

            return Ok(response);
        }


        //GET:Confirm Email
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid email confirmation link");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Error confirming your email.");

            // Redirect to frontend login page
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var redirectUrl = $"{frontendUrl}/login";

            return Redirect(redirectUrl);
        }


        // Put api/auth/PutbyId
        [HttpPut]
        [Route("user/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm] UpdateUserDto model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound(new { Message = "Korisnik nije pronađen." });

                if (model.UserPicture != null)
                {
                    var resultimg = await _imageUploadService.UploadImageAsync(model.UserPicture);
                    if (!resultimg.Success)
                        return BadRequest(resultimg.ErrorMessage);

                    user.UserPicture = resultimg.Url;
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Languages = model.Languages.Split(',').ToList();

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(new { Message = "Greška pri ažuriranju korisnika.", Errors = result.Errors });

                return Ok(new { Message = "Korisnik uspešno ažuriran." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Error = ex.Message });
            }
        }


        // GET api/auth/GetAllUsers
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();

                var userDtos = new List<object>();

                foreach (var user in users)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    userDtos.Add(new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        user.PreduzeceId,
                        Roles = roles,
                        user.Languages,
                        user.UserPicture
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Došlo je do greške na serveru.", Error = ex.Message });
            }
        }

        // GET api/auth/user/{id}
        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { Message = "Korisnik nije pronađen." });
                }

                var roles = await userManager.GetRolesAsync(user);
                var preduzece = await _context.Preduzeca.FirstOrDefaultAsync(p => p.Id == user.PreduzeceId);

                var userDto = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    user.PreduzeceId,
                    user.Languages,
                    UserPicture = user.UserPicture ?? "",
                    Company = preduzece != null ? new
                    {
                        preduzece.Id,
                        preduzece.CompanyName,
                        preduzece.CompanyState,
                        preduzece.CompanyCity,
                        preduzece.CompanyPhone,
                        preduzece.CompanyMail
                    } : null,
                    Roles = roles
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Error = ex.Message });
            }
        }

        // DELETE api/auth/DeleteUser/{id}
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { Message = "Korisnik sa datim ID-jem nije pronađen." });
                }

                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Korisnik uspešno obrisan." });
                }
                else
                {
                    return BadRequest(new { Message = "Došlo je do greške prilikom brisanja korisnika.", Errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Došlo je do greške na serveru.", Error = ex.Message });
            }
        }
    }
}
