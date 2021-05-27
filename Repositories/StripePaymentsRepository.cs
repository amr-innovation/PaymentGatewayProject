using Ardalis.GuardClauses;
using Entity.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    /// This is the repo for stripepayment
    /// </summary>
    public class StripePaymentsRepository : IStripePaymentsRepository
    {
        private stripeContext stripeContext;
        private static readonly string status = "Completed";
        public StripePaymentsRepository(stripeContext stripeContext)
        {
            this.stripeContext = stripeContext;
        }
        public void CreateTransaction(Transaction transaction)
        {
            stripeContext.Transactions.Add(transaction);
            stripeContext.SaveChanges();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var updateTransaction = stripeContext.Transactions.Where(x => x.TransactionId == transaction.TransactionId).FirstOrDefault();
            Guard.Against.Null(updateTransaction, nameof(updateTransaction));

            updateTransaction.Email = transaction.Email;
            updateTransaction.Currency = transaction.Currency;
            updateTransaction.Amount = transaction.Amount;
            updateTransaction.CreatedDate = transaction.CreatedDate;
            updateTransaction.Type = transaction.Type;
            updateTransaction.Status = status;
            stripeContext.Transactions.Update(updateTransaction);
            stripeContext.SaveChanges();

        }
    }
}
