using MaintenanceApi.DTOs.Machines;

namespace MaintenanceApi.Interfaces;

public interface IMachineService
{
    Task<List<MachineResponseDto>> GetAllAsync(
        int? departmentId,
        string? status,
        string? search);

    Task<MachineResponseDto?> GetByIdAsync(int id);
}