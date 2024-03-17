using Alura.CoisasAFazer.Core.Commands;
using Xunit;
using System;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Services.Handlers;
using Alura.CoisasAFazer.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;

namespace Alura.CoisasAFazer.Testes
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void TestaIncluirTarefaValidaNoBD()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var contexto = new DbTarefasContext(options);
            var repositorio = new RepositorioTarefa(contexto);
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2024, 01, 31));
            var handler = new CadastraTarefaHandler(repositorio, mockLogger.Object);

            // Act
            handler.Execute(comando);

            // Assert
            var tarefa = repositorio.ObtemTarefas(t => t.Titulo == "Estudar xUnit").FirstOrDefault();
            Assert.NotNull(tarefa);

        }

        [Fact]
        public void TestaIncluirTarefaComErro()
        {
            // Arrange
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2024, 01, 31));

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro na inclusão de tarefas!"));
            var repo = mock.Object;

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void TestaLogDaMensagemDaException()
        {
            // Arrange
            var mensagemErro = "Houve um erro na inclusão de tarefas";
            var excecaoEsperada = new Exception(mensagemErro);
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2024, 01, 31));

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(excecaoEsperada);
            var repo = mock.Object;

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            mockLogger.Verify(l =>
                l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<object>((v, t) => true),
                    excecaoEsperada,
                    It.Is<Func<object, Exception, string>>((v, t) => true)),
                Times.Once());
        }
    }
}