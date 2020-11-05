using System;
using System.Threading.Tasks;
using NerdStore.Core.Data;

namespace NerdStore.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Task<Pedido> ObterPedidoPorClienteId(Guid clientId);
        void AdicionarItem(PedidoItem item);
        void AtualizarItem(PedidoItem item);
    }
}
