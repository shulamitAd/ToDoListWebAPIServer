using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using WebAPIServer.CustomAttributes;
using WebAPIServer.Models;
using WebAPIServer.Repositories;
using WebAPIServer.Services;
using WebAPIServer.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ILoggerService>(provider =>
{
    var configValue = builder.Configuration.GetValue<string>("Logging:LogLevel:Default");
    var logLevel = (LoggerService.LogLevel)Enum.Parse(typeof(LoggerService.LogLevel), configValue.ToString());
    return new LoggerService(logLevel);
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new DeveloperNameLoggingAttribute(
        builder.Services.BuildServiceProvider().GetService<ILoggerService>()
    ));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TODODBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<ToDoItemValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
