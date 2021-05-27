using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IStripePaymentsRepository
    {
        void CreateTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
    }
}
