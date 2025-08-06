using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Persistance;
using MediatR;
using Application.Features.Commands.ComplaintCommands;
using Persistance.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<BiCozumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Repository servisleri
builder.Services.AddPersistenceServices();

// MediatR register
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateComplaintCommand).Assembly)
);

// Controller
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
