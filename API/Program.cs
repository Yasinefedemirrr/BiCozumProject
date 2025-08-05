using Microsoft.EntityFrameworkCore;
using Persistance.Context;

var builder = WebApplication.CreateBuilder(args);

// DbContext konfigürasyonu
builder.Services.AddDbContext<BiCozumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Controller servisi
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development ortamýnda Swagger aktif et
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
