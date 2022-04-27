using AutoMapper;
using TiendaServicios.Api.Libro.Aplicacion.Dtos;
using TiendaServicios.Api.Libro.Modelos;

namespace TiendaServicios.Api.Libro.Aplicacion
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<LibreriaMaterial, LibroDto>();
		}
	}
}
