using System;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public int Quantidade { get; private set; }

        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid id, string nome, int quantidade, decimal valor)
        {
            if (quantidade < Pedido.MIN_UNIDADES_ITEM) throw new DomainException($"Minimo de {Pedido.MIN_UNIDADES_ITEM} unidade por produto");

            Id = id;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valor;
        }

        internal void AdicionarQuantidade(int quantidade)
        {
            Quantidade += quantidade;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AtualizarQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }
    }
}
