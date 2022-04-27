using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteInterface
{
	public interface ILibrosService
	{
		Task<(bool resultado, LibroRemoto libro, string ErrorMessage)> GetLibro(Guid libroId);
	}
}
