using MaintenanceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IMachineService _machineService;

    public MachinesController(IMachineService machineService)
    {
        _machineService = machineService;
    }

    // GET: /api/machines?departmentId=1&status=Operational&search=furnace
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? departmentId,
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var machines = await _machineService.GetAllAsync(
            departmentId,
            status,
            search);

        return Ok(machines);
    }

    // GET: /api/machines/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var machine = await _machineService.GetByIdAsync(id);

        if (machine == null)
            return NotFound(new { message = "Machine not found." });

        return Ok(machine);
    }
}