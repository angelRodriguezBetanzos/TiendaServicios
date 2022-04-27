using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TiendaServicios.Api.Autor.Aplicacion.Dtos;
using TiendaServicios.Api.Autor.HandleError;
using TiendaServicios.Api.Autor.Modelos;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class ConsultaFiltro
	{
		public class AutorUnico : IRequest<AutorDto>
		{
			public string AutorGuid { get; set; }
		}

		public class Manejador : IRequestHandler<AutorUnico, AutorDto>
		{
			private readonly ContextoAutor _contexto;
			private readonly IMapper _mapper;

			public Manejador(ContextoAutor contexto, IMapper mapper)
			{
				_contexto = contexto;
				_mapper = mapper;
			}
			public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
			{
				var autor = await _contexto.AutorLibro
									.Where(x => x.AutorLibroGuid == request.AutorGuid)
									.FirstOrDefaultAsync();
				if(autor is null)
				{
					throw new ExceptionHandler(HttpStatusCode.NotFound, new { message = "No se encontró el autor" });
				}
				var autorDto = _mapper.Map<AutorLibro, AutorDto>(autor);
				return autorDto;
			}
		}
	}
}
