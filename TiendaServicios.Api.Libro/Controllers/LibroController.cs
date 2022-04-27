using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Aplicacion.Dtos;

namespace TiendaServicios.Api.Libro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibroController : ControllerBase
	{
		private readonly IMediator _mediator;

		public LibroController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<Unit>> Crear(Nuevo.Agregar agregar) 
		{
			return await _mediator.Send(agregar);
		}

		[HttpGet]
		public async Task<ActionResult<List<LibroDto>>> GetLibros() 
		{
			return await _mediator.Send(new Consulta.ListaLibro());
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<LibroDto>> GetLibro(Guid id)
		{
			return await _mediator.Send(new ConsultaFiltro.LibroUnico { LibroId = id});
		}
	}
}
