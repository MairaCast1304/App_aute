using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _svc;

    public UsersController(UserService svc) => _svc = svc;

    // ADMIN: listar
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    // ADMIN: obtener por id
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _svc.GetAsync(id));

    // ADMIN: crear
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        => Ok(await _svc.CreateAsync(dto));

    // ADMIN: actualizar
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    // ADMIN: eliminar
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }

    // ADMIN: desbloquear usuario bloqueado manualmente
    [HttpPost("{id:guid}/unlock")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Unlock(Guid id)
    {
        await _svc.UnlockAsync(id);
        return NoContent();
    }

    // USER: actualizar propio perfil
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(ClaimTypes.Name) // fallback
                     ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userId, out var id))
            return Unauthorized();

        await _svc.UpdateProfileAsync(id, dto);
        return NoContent();
    }
}
