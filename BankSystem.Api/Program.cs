/*using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation.AspNetCore;
using Infrastructure.Storge;*/

using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using ClientStorage;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BankSystemDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IClientStorage, BankSystem.Data.Storage.ClientStorage>();
builder.Services.AddScoped<IClientService, ClientService>();


builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<ClientDto>();
});



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