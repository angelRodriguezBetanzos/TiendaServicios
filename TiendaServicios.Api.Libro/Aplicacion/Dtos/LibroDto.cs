namespace TiendaServicios.Api.Libro.Aplicacion.Dtos
{
	public class LibroDto
	{
		public Guid? LibreriaMaterialId { get; set; }
		public string? Titulo { get; set; }
		public DateTime? FechaPublicacion { get; set; }
		public Guid? AutorLibro { get; set; }
	}
}
