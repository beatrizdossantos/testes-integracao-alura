using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alura.CoisasAFazer.Testes
{
    public class GerenciaPrazoDasTarefasHandlerExecute
    {
        [Fact]
        public void TestaMudarStatusDeTarefasAtrasadas()
        {
            // Arrange
            var categoriaFaculdade = new Categoria(1, "Tarefas da Faculdade");
            var categoriaCasa = new Categoria(2, "Tarefas da Casa");

            var listaTarefas = new List<Tarefa>
            {
                new Tarefa(1, "Limpar Sapatos", categoriaCasa, new DateTime(2024, 01, 31), null, StatusTarefa.Pendente),
                new Tarefa(2, "Baixar Material", categoriaFaculdade, new DateTime(2024, 01, 31), new DateTime(2024, 01, 05), StatusTarefa.Concluida),
                new Tarefa(5, "Estudar para Prova", categoriaFaculdade, new DateTime(2024, 01, 31), null, StatusTarefa.Criada),
                
                // Tarefas Atrasadas
                new Tarefa(3, "Fazer Revisão", categoriaFaculdade, new DateTime(2024, 01, 04), null, StatusTarefa.Pendente),
                new Tarefa(4, "Arrumar Quarto", categoriaCasa, new DateTime(2024, 01, 02), null, StatusTarefa.Criada)
            };

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
               .UseInMemoryDatabase("DbTarefasContext")
               .Options;

            var contexto = new DbTarefasContext(options);
            var repositorio = new RepositorioTarefa(contexto);
            repositorio.IncluirTarefas(listaTarefas.ToArray());

            var comando = new GerenciaPrazoDasTarefas(new DateTime(2024, 01, 06));
            var handler = new GerenciaPrazoDasTarefasHandler(repositorio);

            var tarefasEmAtraso = repositorio.ObtemTarefas(t => t.Status == StatusTarefa.EmAtraso);

            // Act
            handler.Execute(comando);

            // Assert
            Assert.Equal(2, tarefasEmAtraso.Count());
        }

        [Fact]
        public void TestaQuantidadeDeChamadasDoMetodoDeAtualizarTarefas()
        {
            // Arrange
            var categoriaFaculdade = new Categoria(1, "Tarefas da Faculdade");
            var categoriaCasa = new Categoria(2, "Tarefas da Casa");

            var listaTarefas = new List<Tarefa>
            {
                // Tarefas Atrasadas
                new Tarefa(3, "Fazer Revisão", categoriaFaculdade, new DateTime(2024, 01, 04), null, StatusTarefa.Pendente),
                new Tarefa(4, "Arrumar Quarto", categoriaCasa, new DateTime(2024, 01, 02), null, StatusTarefa.Criada)
            };

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.ObtemTarefas(It.IsAny<Func<Tarefa, bool>>()))
                .Returns(listaTarefas);

            var repositorio = mock.Object;

            var comando = new GerenciaPrazoDasTarefas(new DateTime(2024, 01, 06));
            var handler = new GerenciaPrazoDasTarefasHandler(repositorio);

            // Act
            handler.Execute(comando);

            // Asert
            mock.Verify(r => r.AtualizarTarefas(It.IsAny<Tarefa[]>()), Times.Once());

        }
    }
}
