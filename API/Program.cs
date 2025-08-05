using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Persistance.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<BiCozumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Controller
builder.Services.AddControllers();
builder.Services.AddPersistenceServices();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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