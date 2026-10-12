using MaintenanceApi.Data;
using MaintenanceApi.DTOs.MachineCategories;
using MaintenanceApi.Interfaces;
using MaintenanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceApi.Services;

public class MachineCategoryService : IMachineCategoryService
{
    private readonly AppDbContext _context;

    public MachineCategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MachineCategoryDto>> GetAllAsync()
    {
        return await _context.MachineCategories
            .AsNoTracking()
            .OrderBy(c => c.CategoryName)
            .Select(c => new MachineCategoryDto
            {
                MachineCategoryId = c.MachineCategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            })
            .ToListAsync();
    }

    public async Task<MachineCategoryDto?> GetByIdAsync(int id)
    {
        return await _context.MachineCategories
            .AsNoTracking()
            .Where(c => c.MachineCategoryId == id)
            .Select(c => new MachineCategoryDto
            {
                MachineCategoryId = c.MachineCategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MachineCategoryDto> CreateAsync(MachineCategoryDto dto)
    {
        var category = new MachineCategory
        {
            CategoryName = dto.CategoryName.Trim(),
            Description = dto.Description
        };

        _context.MachineCategories.Add(category);
        await _context.SaveChangesAsync();

        dto.MachineCategoryId = category.MachineCategoryId;
        dto.CategoryName = category.CategoryName;

        return dto;
    }
}