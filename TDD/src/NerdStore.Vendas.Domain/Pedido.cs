using System;
using System.Collections.Generic;
using System.Linq;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public enum PedidoStatus
    {
        Rascunho = 0, Iniciado=1, Pago=2, Entregue=3, Cancelado=4
    }

    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM = 15;
        public static int MIN_UNIDADES_ITEM = 1;

        public Guid ClienteId { get; private set; }

        public decimal ValorTotal { get; private set; }

        private readonly List<PedidoItem> _items;

        public IReadOnlyCollection<PedidoItem> Items => _items;

        public PedidoStatus Status{ get; private set; }

        protected Pedido()
        {
            _items = new List<PedidoItem>();
        }

        private bool PedidoItemExiste(PedidoItem item)
        {
            return _items.Any(i => i.Id == item.Id);
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var qtdeItems = item.Quantidade;
            if (PedidoItemExiste(item))
            {
                var itemExistente = _items.FirstOrDefault(i => i.Id == item.Id);
                qtdeItems += itemExistente.Quantidade;
            }
            if (qtdeItems > MAX_UNIDADES_ITEM)
            {
                throw new DomainException($"Maximo de {MAX_UNIDADES_ITEM} unidades por produto");
            }
        }

        public void AdicionarItem(PedidoItem item)
        {
            ValidarQuantidadeItemPermitida(item);

            if (PedidoItemExiste(item))
            {
                var itemExistente = _items.FirstOrDefault(i => i.Id == item.Id);
                itemExistente.AdicionarQuantidade(item.Quantidade);
                item = itemExistente;
                _items.Remove(itemExistente);
            }
            _items.Add(item);

            this.CarcularValorPedido();
        }

        private void CarcularValorPedido()
        {
            ValorTotal = _items.Sum(i => i.CalcularValor());
        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public static class Factory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId
                };
                pedido.TornarRascunho();

                return pedido;
            }
        }
    }
}
