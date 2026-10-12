using Microsoft.EntityFrameworkCore;
using MaintenanceApi.Data;
using MaintenanceApi.Data.Seed;
using MaintenanceApi.Interfaces;
using MaintenanceApi.Services;
using Microsoft.AspNetCore.Identity;
using MaintenanceApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IMachineCategoryService, MachineCategoryService>();
builder.Services.AddScoped<IMachineSpecificationService,MachineSpecificationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IMaintenancePlanService,MaintenancePlanService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IMaintenanceTaskService,MaintenanceTaskService>();
builder.Services.AddScoped<IMaintenanceRecordService,MaintenanceRecordService>();
builder.Services.AddScoped<IWorkOrderStatusHistoryService,WorkOrderStatusHistoryService>();
var app = builder.Build();

// Seed initial departments
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DepartmentSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();