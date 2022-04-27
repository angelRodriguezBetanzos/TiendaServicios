using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TiendaServicios.Api.Libro.Aplicacion.Dtos;
using TiendaServicios.Api.Libro.HandleError;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
	public class ConsultaFiltro
	{
		public class LibroUnico : IRequest<LibroDto>
		{
			public Guid LibroId { get; set; }
		}

		public class Manejador : IRequestHandler<LibroUnico, LibroDto>
		{
			private readonly ContextoLibreria _contexto;
			private readonly IMapper _mapper;

			public Manejador(ContextoLibreria contexto, IMapper mapper)
			{
				_contexto = contexto;
				_mapper = mapper;
			}
			public async Task<LibroDto> Handle(LibroUnico request, CancellationToken cancellationToken)
			{
				var libro = await _contexto.LibreriaMaterial
						.Where(x => x.LibreriaMaterialId == request.LibroId)
						.FirstOrDefaultAsync();
				if (libro is null)
				{
					throw new ExceptionHandler(HttpStatusCode.NotFound, new { message = "No se encontró el libro" });
				}
				var libroDto = _mapper.Map<LibreriaMaterial,LibroDto>(libro);
				return libroDto;

			}
		}
	}
}
