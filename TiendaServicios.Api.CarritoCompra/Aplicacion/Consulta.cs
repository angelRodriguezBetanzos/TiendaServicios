using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Aplicacion.Dtos;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
	public class Consulta
	{
		public class Buscar : IRequest<CarritoDto>
		{
			public int CarritoSesionId { get; set; }
		}

		public class Manejador : IRequestHandler<Buscar, CarritoDto>
		{
			private readonly CarritoContexto _contexto;
			private readonly ILibrosService _libroService;

			public Manejador(CarritoContexto contexto, ILibrosService libroService)
			{
				_contexto = contexto;
				_libroService = libroService;
			}
			public async Task<CarritoDto> Handle(Buscar request, CancellationToken cancellationToken)
			{
				var carritoSesion = await _contexto.CarritoSesion
					.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
				var carritoSesionDetalle = await _contexto.CarritoSesionDetalle
							.Where(x => x.CarritoSesionId == request.CarritoSesionId)
							.ToListAsync();

				var listaCarritoDto = new List<CarritoDetalleDto>();
				foreach (var libro in carritoSesionDetalle) 
				{ 
					var response = await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));
					if (response.resultado)
					{
						var objetoLibro = response.libro;
						var carritoDetalle = new CarritoDetalleDto
						{
							TituloLibro = objetoLibro.Titulo,
							FechaPublicacion = objetoLibro.FechaPublicacion,
							LibroId = (Guid)objetoLibro.LibreriaMaterialId,
						};
						listaCarritoDto.Add(carritoDetalle);

					}
				}

				var carritoSesionDto = new CarritoDto
				{
					CarritoId = carritoSesion.CarritoSesionId,
					FechaCreacion = carritoSesion.FechaCreacion,
					CarritoDetalles = listaCarritoDto

				};

				return carritoSesionDto;
			}
		}
	}
}
