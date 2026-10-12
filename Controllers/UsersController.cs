
using MaintenanceApi.DTOs.Users;
using MaintenanceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(
        UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new
            {
                message = "Full name, email and password are required."
            });
        }

        if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
            .IsValid(dto.Email))
        {
            return BadRequest(new { message = "Invalid email address." });
        }

        if (dto.Password.Length < 8)
        {
            return BadRequest(new
            {
                message = "Password must contain at least 8 characters."
            });
        }

        var user = await _service.CreateAsync(dto);

        if (user == null)
            return Conflict(new { message = "Email already exists." });

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.UserId },
            user
        );
    }
}
