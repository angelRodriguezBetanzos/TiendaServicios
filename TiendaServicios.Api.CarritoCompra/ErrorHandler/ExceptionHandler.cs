using System.Net;

namespace TiendaServicios.Api.CarritoCompra.HandleError
{
	public class ExceptionHandler : Exception
	{
		public HttpStatusCode Codigo { get; }
		public object Errors { get; }
		public ExceptionHandler(HttpStatusCode codigo, object errores = null)
		{
			Codigo = codigo;
			Errors = errores;
		}
	}
}
