using System;
using System.Collections.Generic;
using System.Linq;
using NerdStore.Core.DomainObjects;
using FluentValidation.Results;

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

        public decimal Desconto { get; private set; }

        private readonly List<PedidoItem> _items;

        public IReadOnlyCollection<PedidoItem> Items => _items;

        public PedidoStatus Status{ get; private set; }

        public Voucher Voucher { get; private set; }

        public bool VoucherUtilizado { get; private set; }

        protected Pedido()
        {
            _items = new List<PedidoItem>();
        }

        private bool PedidoItemExiste(PedidoItem item)
        {
            return _items.Any(i => i.Id == item.Id);
        }

        private void ValidarPedidoItemNaoExiste(PedidoItem item)
        {
            if (!PedidoItemExiste(item)) throw new DomainException("O item nao existe no pedido");
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

            CarcularValorPedido();
        }

        public void AtualizarItem(PedidoItem item)
        {
            ValidarPedidoItemNaoExiste(item);

            ValidarQuantidadeItemPermitida(item);

            var itemExistente = _items.FirstOrDefault(i => i.Id == item.Id);
            _items.Remove(itemExistente);
            _items.Add(item);

            CarcularValorPedido();
        }

        public void RemoverItem(PedidoItem item)
        {
            ValidarPedidoItemNaoExiste(item);

            _items.Remove(item);

            CarcularValorPedido();
        }

        private void CarcularValorPedido()
        { 
            ValorTotal = _items.Sum(i => i.CalcularValor());

            CalcularValorTotalDesconto();
        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotalDesconto();

            return result;
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valorComDesconto = ValorTotal;

            if (Voucher.TipoDesconto == TipoDesconto.Valor)
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valorComDesconto -= desconto;
                }
            }
            else
            {
                if (Voucher.PercentualDesconto.HasValue)
                {
                    desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;
                    valorComDesconto -= desconto;
                }
            }
            ValorTotal = valorComDesconto < 0 ? 0 : valorComDesconto;
            Desconto = desconto;
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
