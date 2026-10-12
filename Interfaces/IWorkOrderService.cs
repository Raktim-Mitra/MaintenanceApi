
using MaintenanceApi.DTOs.WorkOrders;

namespace MaintenanceApi.Interfaces;

public interface IWorkOrderService
{
    Task<List<WorkOrderResponseDto>> GetAllAsync(
        int? departmentId,
        string? status);

    Task<WorkOrderResponseDto?> GetByIdAsync(int id);

    Task<WorkOrderResponseDto> CreateAsync(
        WorkOrderCreateDto dto);

    Task<bool> UpdateStatusAsync(
        int id,
        string newStatus,
        string? remarks);
}
