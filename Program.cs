var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseHttpsRedirection();
  
}


app.MapGet("/machines", () =>
{
    return new[]
    {
        new { Id = 1, Name = "Casting Furnace", Status = "Operational" },
        new { Id = 2, Name = "Conveyor Belt", Status = "Under Maintenance" }
    };
});



app.Run();


