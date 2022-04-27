using FluentValidation;
using MediatR;
using System.Net;
using TiendaServicios.Api.Autor.HandleError;
using TiendaServicios.Api.Autor.Modelos;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class Nuevo
	{
		public class Agregar : IRequest
		{
			public string Nombre { get; set; }
			public string Apellido { get; set; }
			public DateTime? FechaNacimiento { get; set; }
		}

		public class ValidarAgregar : AbstractValidator<Agregar>
		{
			public ValidarAgregar()
			{
				RuleFor(x => x.Nombre).NotEmpty();
				RuleFor(x => x.Apellido).NotEmpty();
			}
		}

		public class Manejador : IRequestHandler<Agregar>
		{
			private readonly ContextoAutor _contexto;

			public Manejador(ContextoAutor contexto)
			{
				_contexto = contexto;
			}
			public async Task<Unit> Handle(Agregar request, CancellationToken cancellationToken)
			{
				var autor = new AutorLibro
				{
					Nombre = request.Nombre,
					Apellido = request.Apellido,
					FechaNacimiento = request.FechaNacimiento,
					AutorLibroGuid = Convert.ToString(Guid.NewGuid()),
				};

				await _contexto.AutorLibro.AddAsync(autor);
				var resultado = await _contexto.SaveChangesAsync();
				if (resultado > 0)
				{
					return Unit.Value;
				}
				throw new ExceptionHandler(HttpStatusCode.InternalServerError, new { message = "No se pudo agregar el autor" });
			}
		}
	}
}
