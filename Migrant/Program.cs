using Microsoft.EntityFrameworkCore;
using Migrant.Application.Abstractions;
using Migrant.Application.Extensions;
using Migrant.Application.Services;
using Migrant.Data.Context;
using Migrant.Data.Extensions;
using Migrant.Services;
using Migrant.Application.Options;

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

builder.Services.AddHttpClient<PassportFileDownloader>();

builder.Services.AddSingleton<ZipExtractor>();
builder.Services.AddSingleton<PassportCsvReader>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<PassportFileSource>();

builder.Services.AddScoped<IPassportSource, PassportFileSource>();
builder.Services.AddScoped<IPassportUpdateRunner, PassportUpdateRunner>();

builder.Services.Configure<PassportUpdateOptions>(
    builder.Configuration.GetSection("PassportUpdate"));

builder.Services.AddHostedService<PassportUpdateBackgroundService>();

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
