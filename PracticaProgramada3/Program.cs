using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Application.Mapping;
using PracticaProgramada3.Application.Services;
using PracticaProgramada3.Infrastructure.Data;
using PracticaProgramada3.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controladores de la API (VotacionController y PartidosController)
builder.Services.AddControllers();

// Base de datos: la cadena de conexión "DefaultConnection" está en appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper: convierte entidades en DTOs usando el perfil MapeoClases
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapeoClases>());

// Inyección de dependencias: cada interfaz se asocia con su implementación
builder.Services.AddScoped<IVotanteRepository, VotanteRepository>();
builder.Services.AddScoped<IPartidoRepository, PartidoRepository>();
builder.Services.AddScoped<IVotoRepository, VotoRepository>();
builder.Services.AddScoped<IPartidoServicio, PartidoServicio>();
builder.Services.AddScoped<IVotacionServicio, VotacionServicio>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();