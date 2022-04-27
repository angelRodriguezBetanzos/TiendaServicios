using FluentValidation;
using MediatR;
using System.Net;
using TiendaServicios.Api.Libro.HandleError;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
	public class Nuevo
	{
		public class Agregar : IRequest
		{
			public string? Titulo { get; set; }
			public DateTime? FechaPublicacion { get; set; }
			public Guid? AutorLibro { get; set; }
		}

		public class ValidarAgregar : AbstractValidator<Agregar>
		{
			public ValidarAgregar()
			{
				RuleFor(x => x.Titulo).NotEmpty();
				RuleFor(x => x.FechaPublicacion).NotEmpty();
				RuleFor(x => x.AutorLibro).NotEmpty();
			}
		}

		public class Manejador : IRequestHandler<Agregar>
		{
			private readonly ContextoLibreria _contexto;

			public Manejador(ContextoLibreria contexto)
			{
				_contexto = contexto;
			}
			public async Task<Unit> Handle(Agregar request, CancellationToken cancellationToken)
			{
				var libro = new LibreriaMaterial
				{
					Titulo = request.Titulo,
					FechaPublicacion = request.FechaPublicacion,
					AutorLibro = request.AutorLibro,
				};

				await _contexto.LibreriaMaterial.AddRangeAsync(libro);
				var resultado = await _contexto.SaveChangesAsync();
				if (resultado > 0)
				{
					return Unit.Value;
				}
				throw new ExceptionHandler(HttpStatusCode.InternalServerError, new { message = "No se pudo agregar el libro" });
			}
		}

	}
}
