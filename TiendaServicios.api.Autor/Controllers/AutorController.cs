using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Aplicacion.Dtos;
using TiendaServicios.Api.Autor.Modelos;

namespace TiendaServicios.Api.Autor.Controllers
{
	[Route("Api/[controller]")]
	[ApiController]
	public class AutorController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AutorController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<Unit>> Crear(Nuevo.Agregar agregar)
		{
			return await _mediator.Send(agregar);
		}

		[HttpGet]
		public async Task<ActionResult<List<AutorDto>>> GetAutores()
		{
			return await _mediator.Send(new Consulta.ListaAutor());
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<AutorDto>> GetAutor(string id)
		{
			return await _mediator.Send(new ConsultaFiltro.AutorUnico { AutorGuid = id });
		}

	}
}
