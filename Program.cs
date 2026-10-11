using Microsoft.EntityFrameworkCore;
using MaintenanceApi.Data;
using MaintenanceApi.Data.Seed;
using MaintenanceApi.Interfaces;
using MaintenanceApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IMachineService, MachineService>();
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