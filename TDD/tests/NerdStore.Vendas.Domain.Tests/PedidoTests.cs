﻿using System;
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

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        public void AtualizarItemPedido_ItemNaoExiste_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: Pedido.MAX_UNIDADES_ITEM + 1, valor: 100);

            // Act e Assert
            Assert.Throws<DomainException>(testCode: () => pedido.AtualizarItem(pedidoItem));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(id: productId, nome: "Produto Teste", quantidade: 1, valor: 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(id: productId, nome: "Produto Teste", quantidade: 5, valor: 100);

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Act e Assert
            Assert.Equal(expected: 5, actual: pedido.Items.FirstOrDefault(i => i.Id == productId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        public void AtualizarItemPedido_ItemsDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();

            var pedidoItem1 = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste A", quantidade: 2, valor: 100);
            var pedidoItem2 = new PedidoItem(id: productId, nome: "Produto Teste B", quantidade: 3, valor: 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var pedidoItemAtualizado = new PedidoItem(id: productId, nome: "Produto Teste B", quantidade: 5, valor: 15);

            var totalPedido = (pedidoItem1.Quantidade * pedidoItem1.ValorUnitario) +
                              (pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario);
            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(expected: totalPedido, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade Acima do Permitido")]
        public void AtualizarItemPedido_ItemsAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: 3, valor: 100);

            pedido.AdicionarItem(pedidoItem1);

            var pedidoItemAtualizado = new PedidoItem(id: productId, nome: "Produto Teste", quantidade: Pedido.MAX_UNIDADES_ITEM + 1, valor: 15);

            // Act e Assert
            Assert.Throws<DomainException>(testCode: () => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        public void RemoverItemPedido_ItemNaoExiste_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste", quantidade: Pedido.MAX_UNIDADES_ITEM + 1, valor: 100);

            // Act e Assert
            Assert.Throws<DomainException>(testCode: () => pedido.RemoverItem(pedidoItem));
        }

        [Fact(DisplayName = "Remover Item Pedido")]
        public void RemoverItemPedido_ItemExiste_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.Factory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();

            var pedidoItem1 = new PedidoItem(id: Guid.NewGuid(), nome: "Produto Teste 1", quantidade: 2, valor: 100);
            var pedidoItem2 = new PedidoItem(id: productId, nome: "Produto Teste 2", quantidade: 3, valor: 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var totalPedido = (pedidoItem2.Quantidade * pedidoItem2.ValorUnitario);

            // Act
            pedido.RemoverItem(pedidoItem1);

            // Assert
            Assert.Equal(expected: totalPedido, actual: pedido.ValorTotal);
        }
    }
}