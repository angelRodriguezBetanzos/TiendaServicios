using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
	public class LibrosServiceTest
	{
		private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
		{
			A.Configure<LibreriaMaterial>()
				.Fill(x => x.Titulo).AsArticleTitle()
				.Fill(x => x.LibreriaMaterialId, () => {return Guid.NewGuid(); });

			var lista = A.ListOf<LibreriaMaterial>(30);
			lista[0].LibreriaMaterialId = Guid.Empty;

			return lista;
		}

		private Mock<ContextoLibreria> CrearContexto()
		{
			var dataPrueba = ObtenerDataPrueba().AsQueryable();
			var dbSet = new Mock<DbSet<LibreriaMaterial>>();
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

			dbSet.As<IAsyncEnumerable<LibreriaMaterial>>()
				.Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
				.Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

			dbSet.As<IQueryable<LibreriaMaterial>>()
				.Setup(x => x.Provider)
				.Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));
			
			var contexto = new Mock<ContextoLibreria>();
			contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
			return contexto;

		}

		[Fact]
		public async Task GetLibros()
		{
			System.Diagnostics.Debugger.Launch();
			//Que métpdp dentro del microservice libro se está encargando
			//de realizar la consulta de libros en la base de datos
			//1. Emular a la instancia de Entity Framework Core
			//para emular las acciones y eventos de un objeto en un ambiente de unit test
			//utilizamos objetos de tipo mock

			var mockContext = CrearContexto();

			//2. Emular el mapper

			var mapConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingTest());
			});

			var mockMapper = mapConfig.CreateMapper();

			//3. Instanciar a la clase Manejador y pasarle los parámetros

			var manejador = new Consulta.Manejador(mockContext.Object, mockMapper);

			var request = new Consulta.ListaLibro();
			var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

			Assert.True(lista.Any());
		}

		[Fact]
		public async Task GetLibroPorId()
		{
			var mockContext = CrearContexto();

			var mapConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingTest());
			});

			var mockMapper = mapConfig.CreateMapper();

			var request = new ConsultaFiltro.LibroUnico();
			request.LibroId = Guid.Empty;

			var manejador = new ConsultaFiltro.Manejador(mockContext.Object, mockMapper);

			var result = await manejador.Handle(request, new System.Threading.CancellationToken());

			Assert.NotNull(result);
			Assert.True(result.LibreriaMaterialId == Guid.Empty);
		}

		[Fact]
		public async Task GuardarLibro()
		{
			//Este est es para unit test Libro
			System.Diagnostics.Debugger.Launch();
			var options = new DbContextOptionsBuilder<ContextoLibreria>()
				.UseInMemoryDatabase(databaseName: "BaseDatosLibro")
				.Options;
			var context = new ContextoLibreria(options);

			var request = new Nuevo.Agregar();
			request.Titulo = "Libro de Microservice";
			request.AutorLibro = Guid.Empty;
			request.FechaPublicacion = DateTime.Now;

			var manejador = new Nuevo.Manejador(context);
			var resultado = await manejador.Handle(request, new System.Threading.CancellationToken());

			Assert.True(resultado == MediatR.Unit.Value);

		}
	}
}
