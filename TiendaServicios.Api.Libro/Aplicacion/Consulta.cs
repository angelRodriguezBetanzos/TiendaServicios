using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Aplicacion.Dtos;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
	public class Consulta
	{
		public class ListaLibro : IRequest<List<LibroDto>>
		{

		}

		public class Manejador : IRequestHandler<ListaLibro, List<LibroDto>>
		{
			private readonly ContextoLibreria _contexto;
			private readonly IMapper _mapper;

			public Manejador(ContextoLibreria contexto, IMapper mapper)
			{
				_contexto = contexto;
				_mapper = mapper;
			}
			public async Task<List<LibroDto>> Handle(ListaLibro request, CancellationToken cancellationToken)
			{
				var libros = await _contexto.LibreriaMaterial.ToListAsync();
				var librosDto = _mapper.Map<List<LibreriaMaterial>,List<LibroDto>>(libros);
				return librosDto;

			}
		}
	}
}
