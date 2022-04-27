using MediatR;
using System.Net;
using TiendaServicios.Api.CarritoCompra.HandleError;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
	public class Nuevo
	{
		public class Agregar : IRequest
		{
			public DateTime? FechaCreacion { get; set; }
			public List<string>? ProductoLista { get;set; }
		}

		public class Manejador : IRequestHandler<Agregar>
		{
			private readonly CarritoContexto _contexto;

			public Manejador(CarritoContexto contexto)
			{
				_contexto = contexto;
			}
			public async Task<Unit> Handle(Agregar request, CancellationToken cancellationToken)
			{
				var carritoSesion = new CarritoSesion
				{
					FechaCreacion = request.FechaCreacion,
				};

				await _contexto.CarritoSesion.AddAsync(carritoSesion);
				var resultado = await _contexto.SaveChangesAsync();

				if (resultado == 0)
				{
					throw new ExceptionHandler(HttpStatusCode.InternalServerError, new { message = "No se pudo agregar la sesión" });
				}

				var id = carritoSesion.CarritoSesionId;
				if (request.ProductoLista is not null)
				{
					request.ProductoLista.ForEach(async x =>
					{
						var detalleSesion = new CarritoSesionDetalle
						{
							FechaCreacion = DateTime.Now,
							CarritoSesionId = id,
							ProductoSeleccionado = x
						};
						await _contexto.CarritoSesionDetalle.AddAsync(detalleSesion);
					});

					resultado = await _contexto.SaveChangesAsync();
					if(resultado > 0)
					{
						return Unit.Value;
					}
					throw new ExceptionHandler(HttpStatusCode.InternalServerError, new { message = "No se pudo insertar el carrito de compras" });
				}
				throw new ExceptionHandler(HttpStatusCode.BadRequest, new { message = "No se enviaron los datos correctos" });
			}
		}
	}
}
