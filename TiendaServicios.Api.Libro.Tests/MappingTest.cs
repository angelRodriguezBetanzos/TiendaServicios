using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion.Dtos;
using TiendaServicios.Api.Libro.Modelos;

namespace TiendaServicios.Api.Libro.Tests
{
	public class MappingTest : Profile
	{
		public MappingTest()
		{
			CreateMap<LibreriaMaterial, LibroDto>();
		}
	}
}
