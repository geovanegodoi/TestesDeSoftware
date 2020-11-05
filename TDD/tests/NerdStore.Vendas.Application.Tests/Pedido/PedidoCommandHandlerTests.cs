using System;
using Xunit;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using Moq.AutoMock;
using Moq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Tests.Pedido
{
    public class PedidoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoHandler;
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly Domain.Pedido _pedido;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();
            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _pedido = Domain.Pedido.Factory.NovoPedidoRascunho(_clienteId);
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        public async void AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand  = new AdicionarItemPedidoCommand(clienteId: Guid.NewGuid(), produtoId: Guid.NewGuid(),
                                                                nome: "Produto Teste", quantidade: 1, valorUnitario: 100);

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Domain.Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //_mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Novo Item ao Pedido em Andamento com Sucesso")]
        public async void AdicionarItem_NovoItemPedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItem = new PedidoItem(id: Guid.NewGuid(), nome: "Produto 1", quantidade: 2, valor: 100);
            _pedido.AdicionarItem(pedidoItem);

            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: _clienteId, produtoId: Guid.NewGuid(), nome: "Produto 2", quantidade: 1, valorUnitario: 50);

            _mocker.GetMock<IPedidoRepository>().Setup(expression: r => r.ObterPedidoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<Domain.PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Domain.Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Existente em Pedido em Andamento com Sucesso")]
        public async void AdicionarItem_ItemExistentePedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItem = new PedidoItem(id: _produtoId, nome: "Produto 1", quantidade: 2, valor: 100);
            _pedido.AdicionarItem(pedidoItem);

            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: _clienteId, produtoId: _produtoId, nome: "Produto 1", quantidade: 1, valorUnitario: 100);

            _mocker.GetMock<IPedidoRepository>().Setup(expression: r => r.ObterPedidoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<Domain.PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Domain.Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Comando Invalido")]
        public async void AdicionarItem_ComandoInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: Guid.Empty, produtoId: Guid.Empty, nome: null, quantidade: 0, valorUnitario: 0);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.Equal(expected: 5, actual: pedidoCommand.ValidationResult.Errors.Count);
        }
    }
}
