using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Aplicacion.Dtos;

namespace TiendaServicios.Api.CarritoCompra.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarritoComprasController : ControllerBase
	{
		private readonly IMediator _mediator; 

		public CarritoComprasController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<Unit>> Crear(Nuevo.Agregar agregar)
		{
			return await _mediator.Send(agregar);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CarritoDto>> GetCarrito(int id)
		{
			return await _mediator.Send(new Consulta.Buscar { CarritoSesionId = id });
		}
	}
}
