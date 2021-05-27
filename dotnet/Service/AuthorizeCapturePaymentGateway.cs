using Entity.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using Stripe;
using server.Models;
using Ardalis.GuardClauses;
using System.Threading.Tasks;

namespace server.Service
{

    /// <summary>
    ///  This is the service layer for the Authorize and Capture implementation 
    /// </summary>
    public class AuthorizeCapturePaymentGateway : IAuthorizeCapturePaymentGateway
    {
        private readonly IStripePaymentsRepository stripePaymentsRepository;

        private const string status = "completed";
        private const string email = "aneezm12@gmail.com";
        private const string transactionStatus = "pending";
        private const string manual = "manual";

        public AuthorizeCapturePaymentGateway(IStripePaymentsRepository stripePaymentsRepository)
        {
            this.stripePaymentsRepository = stripePaymentsRepository;
        }


        /// <summary>
        /// Implemented Authorize method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthorizeCaptureResponse> Authorize(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));

            var options2 = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                CaptureMethod = manual
            };

            var paymentIntents = new PaymentIntentService();

            var paymentIntent = paymentIntents.Create(options2);
            await Task.Run(() =>
            {
                Transaction transaction = new Transaction
                {
                    Id = Convert.ToString(Guid.NewGuid()),
                    CreatedDate = DateTime.Now,
                    Currency = request.Currency,
                    Status = transactionStatus,
                    TransactionId = paymentIntent.Id,
                    Type= options2.PaymentMethodTypes[0],
                    Amount = request.Amount
                };
                stripePaymentsRepository.CreateTransaction(transaction);
            });

            return  new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };
           
        }


        /// <summary>
        /// Implemented capture method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthorizeCaptureResponse> Capture(PaymentIntentCreateRequest request)
        {
            var options = new PaymentIntentCaptureOptions
            {
                AmountToCapture = request.Amount,
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Capture(request.Payment_id, options);
            await Task.Run(() =>
            {
                Transaction updateTransaction = new Transaction
                {
                    TransactionId = paymentIntent.Id,
                    Currency = paymentIntent.Currency,
                    Amount = request.Amount,
                    Status = status,
                    Email = email,
                    CreatedDate = DateTime.Now,
                    Type = paymentIntent.PaymentMethodTypes[0]
                };

                stripePaymentsRepository.UpdateTransaction(updateTransaction);
            });

            return new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };


        }
    }
}
