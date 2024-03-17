using Xunit;
using Moq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Services.Handlers;
using Alura.CoisasAFazer.Infrastructure;

namespace Alura.CoisasAFazer.Testes
{
    public class ObtemCategoriaPorIdExecute
    {
        [Fact]
        public void TestaQuantidadeDeChamadasDoMetodoObtemCategoriaPorId()
        {
            // Arrange
            var idCategoria = 20;
            var mock = new Mock<IRepositorioTarefas>();
            var repo = mock.Object;

            var comando = new ObtemCategoriaPorId(idCategoria);
            var handler = new ObtemCategoriaPorIdHandler(repo);

            // Act
            handler.Execute(comando);

            //Assert
            mock.Verify(r => r.ObtemCategoriaPorId(idCategoria), Times.Once());

        }

    }
}
