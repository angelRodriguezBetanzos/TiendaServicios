namespace TiendaServicios.Api.CarritoCompra.Aplicacion.Dtos
{
	public class CarritoDto
	{
		public int CarritoId { get; set; }
		public DateTime? FechaCreacion { get; set; }
		public List<CarritoDetalleDto> CarritoDetalles { get; set; }
	}
}
