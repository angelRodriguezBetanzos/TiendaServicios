using System.Text.Json;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
	public class LibrosService : ILibrosService
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly ILogger<LibrosService> _logger;

		public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}
		public async Task<(bool resultado, LibroRemoto libro, string? ErrorMessage)> GetLibro(Guid libroId)
		{
			try
			{
				var cliente = _httpClient.CreateClient("Libros");
				var response = await cliente.GetAsync($"api/Libro/{libroId}");
				if (response.IsSuccessStatusCode)
				{
					var contenido = await response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
					var resultado = JsonSerializer.Deserialize<LibroRemoto>(contenido,options);
					return (true, resultado, null);
				}
				return (false, null, response.ReasonPhrase);

			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				return (false, null, e.Message);
			}
		}
	}
}
