using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Middleware;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ILibrosService, LibrosService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Contexto de datos

builder.Services.AddDbContext<CarritoContexto>(options =>
{
	var connection = builder.Configuration.GetConnectionString("ConexionDb");
	options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

#endregion

#region Mediatr

builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

#endregion

#region HttpClient

builder.Services.AddHttpClient("Libros", config =>
{
	config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

#region Configuración de Custom Middleware

app.UseMiddleware<ErrorHandlerMiddleware>();

#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
