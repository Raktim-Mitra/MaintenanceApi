
using MaintenanceApi.Data;
using MaintenanceApi.DTOs.Roles;
using MaintenanceApi.Interfaces;
using MaintenanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceApi.Services;

public class RoleService : IRoleService
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .OrderBy(r => r.RoleName)
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description
            })
            .ToListAsync();
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .AsNoTracking()
            .Where(r => r.RoleId == id)
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<RoleDto?> CreateAsync(RoleDto dto)
    {
        var name = dto.RoleName.Trim();

        var exists = await _context.Roles
            .AnyAsync(r => r.RoleName.ToLower() == name.ToLower());

        if (exists)
            return null;

        var role = new Role
        {
            RoleName = name,
            Description = dto.Description
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            Description = role.Description
        };
    }
}
