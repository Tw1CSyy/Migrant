using Microsoft.EntityFrameworkCore;
using Migrant.Data.Context;
using Migrant.Data.Extensions;
using Migrant.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Регистрируем Data слой
builder.Services.AddData(builder.Configuration);

// 2. Регистрируем Application слой
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PassportDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
