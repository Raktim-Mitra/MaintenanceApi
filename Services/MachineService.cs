using MaintenanceApi.Data;
using MaintenanceApi.DTOs.Machines;
using MaintenanceApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceApi.Services;

public class MachineService : IMachineService
{
    private readonly AppDbContext _context;

    public MachineService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MachineResponseDto>> GetAllAsync(
        int? departmentId,
        string? status,
        string? search)
    {
        var query = _context.Machines
            .AsNoTracking()
            .Include(m => m.Department)
            .Include(m => m.MachineCategory)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(m => m.DepartmentId == departmentId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(m => m.CurrentStatus == status);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            var lowerTerm = term.ToLower();

            query = query.Where(m =>
                m.MachineName.ToLower().Contains(lowerTerm) ||
                m.AssetCode.ToLower().Contains(lowerTerm));
        }

        return await query
            .OrderBy(m => m.MachineName)
            .Select(m => new MachineResponseDto
            {
                MachineId = m.MachineId,
                AssetCode = m.AssetCode,
                MachineName = m.MachineName,
                DepartmentId = m.DepartmentId,
                DepartmentName = m.Department.DepartmentName,
                MachineCategoryId = m.MachineCategoryId,
                CategoryName = m.MachineCategory.CategoryName,
                Manufacturer = m.Manufacturer,
                ModelNumber = m.ModelNumber,
                SerialNumber = m.SerialNumber,
                InstallationDate = m.InstallationDate,
                Criticality = m.Criticality,
                CurrentStatus = m.CurrentStatus,
                Location = m.Location,
                IsActive = m.IsActive
            })
            .ToListAsync();
    }

    public async Task<MachineResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Machines
            .AsNoTracking()
            .Where(m => m.MachineId == id)
            .Select(m => new MachineResponseDto
            {
                MachineId = m.MachineId,
                AssetCode = m.AssetCode,
                MachineName = m.MachineName,
                DepartmentId = m.DepartmentId,
                DepartmentName = m.Department.DepartmentName,
                MachineCategoryId = m.MachineCategoryId,
                CategoryName = m.MachineCategory.CategoryName,
                Manufacturer = m.Manufacturer,
                ModelNumber = m.ModelNumber,
                SerialNumber = m.SerialNumber,
                InstallationDate = m.InstallationDate,
                Criticality = m.Criticality,
                CurrentStatus = m.CurrentStatus,
                Location = m.Location,
                IsActive = m.IsActive
            })
            .FirstOrDefaultAsync();
    }
}