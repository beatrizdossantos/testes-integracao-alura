using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Alura.CoisasAFazer.WebApp.Controllers;
using Alura.CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alura.CoisasAFazer.Testes
{
    public class TarefasControllerEndpointCadastraTarefa
    {
        [Fact]
        public void TestarRetornoValidoStatus200()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;
            var context = new DbTarefasContext(options);

            context.Categorias.Add(new Categoria(20, "Estudo"));
            context.SaveChanges();

            var repo = new RepositorioTarefa(context);

            var controller = new TarefasController(repo, mockLogger.Object);
            var model = new CadastraTarefaVM();
            model.IdCategoria = 20;
            model.Titulo = "Estudar Xunit";
            model.Prazo = new DateTime(2019, 12, 31);

            // Act
            var endpointReturn = controller.EndpointCadastraTarefa(model);

            // Assert
            Assert.IsType<OkResult>(endpointReturn); // Ok = StatusCode 200
        }

        [Fact]
        public void TestarExcecaoStatus500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.ObtemCategoriaPorId(20)).Returns(new Categoria(20, "Estudo"));
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro"));
            var repo = mock.Object;

            var controller = new TarefasController(repo, mockLogger.Object);
            var model = new CadastraTarefaVM();
            model.IdCategoria = 20;
            model.Titulo = "Estudar Xunit";
            model.Prazo = new DateTime(2019, 12, 31);

            // Act
            var endpointReturn = controller.EndpointCadastraTarefa(model);

            // Assert
            Assert.IsType<StatusCodeResult>(endpointReturn);
            var statusCodeRetornado = (endpointReturn as StatusCodeResult).StatusCode;
            Assert.Equal(500, statusCodeRetornado);
        }
    }
}
