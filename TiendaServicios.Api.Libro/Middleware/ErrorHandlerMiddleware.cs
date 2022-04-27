using Newtonsoft.Json;
using System.Net;
using TiendaServicios.Api.Libro.HandleError;

namespace TiendaServicios.Api.Libro.Middleware
{
	public class ErrorHandlerMiddleware
	{
		private readonly RequestDelegate _requestDelegate;
		private readonly ILogger<ErrorHandlerMiddleware> _logger;
		public ErrorHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlerMiddleware> logger)
		{
			_requestDelegate = requestDelegate;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _requestDelegate(context);
			}
			catch (Exception ex)
			{
				await HandleException(context, ex, _logger);
			}

		}

		private async Task HandleException(HttpContext context, Exception ex, ILogger<ErrorHandlerMiddleware> logger)
		{
			object errores = null;
			switch (ex)
			{
				case ExceptionHandler me:
					logger.LogError(ex, "Manejador Error");
					errores = me.Errors;
					context.Response.StatusCode = (int)me.Codigo;
					break;
				case Exception e:
					logger.LogError(ex, "Error de Servidor");
					errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}
			context.Response.ContentType = "application/json";
			if (errores != null)
			{
				var resultados = JsonConvert.SerializeObject(new { errores });
				await context.Response.WriteAsync(resultados);
			}
		}
	}
}
