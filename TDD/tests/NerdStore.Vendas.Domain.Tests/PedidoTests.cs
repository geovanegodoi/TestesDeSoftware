using System;
using Xunit;
using System.Linq;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Novo Item Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: 2, valor: 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(expected: 200, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Existente Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveAtualizarQuantidadesEValor()
        {
            // Arrange
            var pedido      = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId   = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(id: produtoId, nome: "Produto Teste", quantidade: 2, valor: 100);
            var pedidoItem2 = new PedidoItem(id: produtoId, nome: pedidoItem1.Nome, quantidade: 1, valor: pedidoItem1.ValorUnitario);

            // Act
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(expected: 300, actual: pedido.ValorTotal);
            Assert.Equal(expected: 1, actual: pedido.Items.Count);
            Assert.Equal(expected: 3, actual: pedido.Items.FirstOrDefault(i => i.Id == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Pedido com itens acima do permitido")]
        public void AdicionarItemPedido_ItemsAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: Pedido.MAX_UNIDADES_ITEM + 1, valor: 100);

            // Act e Assert
            Assert.Throws<DomainException>(testCode: () => pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Pedido com itens existente acima do permitido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido      = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId   = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(id: produtoId, nome: "Produto Teste", quantidade: Pedido.MAX_UNIDADES_ITEM, valor: 100);
            var pedidoItem2 = new PedidoItem(id: produtoId, nome: pedidoItem1.Nome, quantidade: 1, valor: pedidoItem1.ValorUnitario);

            // Act e Assert
            pedido.AdicionarItem(pedidoItem1);
            Assert.Throws<DomainException>(testCode: () => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Pedido com itens abaixo do permitido")]
        public void AdicionarItemPedido_ItemsAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            Assert.Throws<DomainException>(testCode: () => new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: Pedido.MIN_UNIDADES_ITEM - 1, valor: 100));
        }
    }
}