using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Validators;
using BankSystem.Infrastructure;
using ClientStorage;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BankSystemDb");
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddDbContext<BankSystemDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IClientStorage, BankSystem.Data.Storage.ClientStorage>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeStorage, EmployeeStorage>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<ClientDtoValidator>();
});

builder.Services.AddHttpClient<CurrencyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();