using System;
using Xunit;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using FluentValidation.Results;

namespace NerdStore.Vendas.Application.Tests.Pedido
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Command Valido")]
        public void AdicionarItemPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: Guid.NewGuid(), produtoId: Guid.NewGuid(),
                                                               nome: "Produto 1", quantidade: 1, valorUnitario: 100);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar Item Command Invalido")]
        public void AdicionarItemPedidoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: Guid.Empty, produtoId: Guid.Empty,
                                                               nome: null, quantidade: 0, valorUnitario: 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(expected: 5, actual: pedidoCommand.ValidationResult.Errors.Count);
        }

        [Theory(DisplayName = "Adicionar Item Command Unidades Dentro do Limite")]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(15)]
        public void AdicionarItemPedidoCommand_PedidoUnidadesDentroDoLimite_DevePassarNaValidacao(int unidades)
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: Guid.NewGuid(), produtoId: Guid.NewGuid(),
                                                               nome: "Produto Teste", quantidade: unidades, valorUnitario: 10);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(pedidoCommand.ValidationResult.Errors);
        }

        [Theory(DisplayName = "Adicionar Item Command Unidades Fora do Limite")]
        [InlineData(0)]
        [InlineData(16)]
        public void AdicionarItemPedidoCommand_PedidoUnidadesForaDoLimite_NaoDevePassarNaValidacao(int unidades)
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId: Guid.NewGuid(), produtoId: Guid.NewGuid(),
                                                               nome: "Produto Teste", quantidade: unidades, valorUnitario: 10);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(expected: 1, actual: pedidoCommand.ValidationResult.Errors.Count);
        }
    }
}
