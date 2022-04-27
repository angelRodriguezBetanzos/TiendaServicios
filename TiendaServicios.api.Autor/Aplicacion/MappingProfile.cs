using AutoMapper;
using TiendaServicios.Api.Autor.Aplicacion.Dtos;
using TiendaServicios.Api.Autor.Modelos;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<AutorLibro, AutorDto>();
		}
	}
}
