using System;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Application.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoPorClienteId(clientId: request.ClienteId);
            var pedidoItem = new PedidoItem(request.ProdutoId, request.Nome, request.Quantidade, request.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.Factory.NovoPedidoRascunho(request.ClienteId);
                pedido.AdicionarItem(pedidoItem);
                _pedidoRepository.Adicionar(pedido);
            }
            else
            {
                var pedidoItemExiste = pedido.PedidoItemExiste(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExiste)
                {
                    _pedidoRepository.AtualizarItem(pedido.Items.FirstOrDefault(i => i.Id == pedidoItem.Id));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }
                _pedidoRepository.Atualizar(pedido);
            }
            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(request.ClienteId, pedido.Id, request.ProdutoId, request.Nome, request.ValorUnitario, request.Quantidade));

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;
            
            foreach (var erro in message.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(message.MessageType, erro.ErrorMessage));
            }
            return false;
        }
    }
}
