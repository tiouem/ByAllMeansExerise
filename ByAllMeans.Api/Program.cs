using System.Reflection;
using ByAllMeans.Api.Database;
using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISneakersRepository, SneakersRepository>();
builder.Services.AddDbContext<AppDbContext>((provider, optionsBuilder) =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.CustomSchemaIds(s => s.FullName?.Replace("+", ".")); });


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();

app.MapControllers();
app.UseExceptionHandler();

app.Run();

public partial class Program
{
}