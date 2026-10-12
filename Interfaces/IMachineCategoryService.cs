using MaintenanceApi.DTOs.MachineCategories;

namespace MaintenanceApi.Interfaces;

public interface IMachineCategoryService
{
    Task<List<MachineCategoryDto>> GetAllAsync();
    Task<MachineCategoryDto?> GetByIdAsync(int id);
    Task<MachineCategoryDto> CreateAsync(MachineCategoryDto dto);
}