using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Middleware;
using TiendaServicios.Api.Autor.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Controller-Fluent Validation

builder.Services.AddControllers()
	.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

#endregion
// Learn more about configuring Swagger/OpenApi at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Contexto de datos

builder.Services.AddDbContext<ContextoAutor>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

#endregion

#region Mediatr

builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

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
