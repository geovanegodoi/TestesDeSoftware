using System;
using System.Threading.Tasks;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
