using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Entity.Entities;

using Repositories.IRepositories;
using server.Models;
using server.Service;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UnitTest
{
    [TestClass]
   public class AuthorizeCapturePaymentGatewayTest
    {
        private Mock<IStripePaymentsRepository> mockStripePaymentsRepository;
        private MockRepository mockRepository;
        private const string status = "completed";
        private const string email = "aneezm12@gmail.com";
        private const string transactionStatus = "pending";
        private const string manual = "manual";
        
        //StripeConfiguration  = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStripePaymentsRepository = this.mockRepository.Create<IStripePaymentsRepository>();
        }

        [TestMethod]
        private AuthorizeCapturePaymentGateway CreateAuthorizeCapturePaymentGateway()
        {

            return new AuthorizeCapturePaymentGateway(
               this.mockStripePaymentsRepository.Object
               );

        }

        [TestMethod]
      //  [ExpectedException(typeof(ArgumentNullException))]
        public async Task Authorize_StateUnderTest_ReturnNull()
        {
            //Arrange
            string expectedParam = "request";
            var authorizeCapturePaymentGateway = this.CreateAuthorizeCapturePaymentGateway();
            PaymentIntentCreateRequest paymentIntentCreateRequest = null;
            // Act
           
                var result = authorizeCapturePaymentGateway.Authorize(paymentIntentCreateRequest);
            //Assert
                Assert.AreEqual(result.Status,TaskStatus.Faulted);
          
          
        }
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            return config;
        }
        [TestMethod]
        public async Task Authorize_StateUnderTest_ReturnExpectedBehavior()
        {
            //Entity.Configuration.StripeOptions
            //Arrange
            // var config = InitConfiguration();
              Transaction transaction = new Transaction
            {
                Id = Convert.ToString(Guid.NewGuid()),
                CreatedDate = DateTime.Now,
                Currency = "usd",
                Status = transactionStatus,
                TransactionId = "pi_1IvbEGH4DR7BOnAWz5fUldyf",
                Type = "card"
            };

            var authorizeCapturePaymentGateway = new AuthorizeCapturePaymentGateway(
               this.mockStripePaymentsRepository.Object
               );
            var paymentMethodTypes = new List<string>();
            paymentMethodTypes.Add("card");

            PaymentIntentCreateRequest paymentIntentCreateRequest = new PaymentIntentCreateRequest
            {
                Amount = 2000,
                CaptureMethod = "capture",
                PaymentMethodTypes = paymentMethodTypes,
                Payment_id = "py88991122kju",
                Currency = "usd"
            };
            AuthorizeCaptureResponse authorizeCaptureResponse = new AuthorizeCaptureResponse
            {
                clientSecret = "pi_1IvdZlH4DR7BOnAWQYLF59AG_secret_EDlzFgqGmjjF14Z5aHR4ZAibe"
            };
            var options2 = new PaymentIntentCreateOptions
            {
                Amount =3000,
                Currency ="usd",
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                CaptureMethod = manual
            };
            StripeConfiguration.ApiKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";

            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(options2);
          //  this.mockStripePaymentsRepository.Setup(x => x.CreateTransaction(transaction));
            this.mockStripePaymentsRepository.Setup(x => x.CreateTransaction(It.IsAny<Transaction>()));
            //Act
            var response = await authorizeCapturePaymentGateway.Authorize(paymentIntentCreateRequest).ConfigureAwait(false);
            //Assert
            Assert.IsNotNull(response);

        }
        
        [TestMethod]
        public void Capture_StateUnderTest_ReturnExpectedBehaviour()
        {
            //Arrange
            var authorizeCapturePaymentGateway = this.CreateAuthorizeCapturePaymentGateway();
            PaymentIntentCreateRequest paymentIntentCreateRequest = new PaymentIntentCreateRequest
            {
                Amount = 2000,
            };
            var options = new PaymentIntentCaptureOptions
            {
                AmountToCapture = 100,
            };

            var service = new PaymentIntentService();
            StripeConfiguration.ApiKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";

           // var paymentIntent = service.Capture("pi_1IvbEGH4DR7BOnAWz5fUldyf", options);
            Transaction updateTransaction = new Transaction
            {
                TransactionId = "pi_1IvbEGH4DR7BOnAWz5fUldyf",
                Currency = "usd",
                Amount = 2000,
                Status = status,
                Email = email,
                CreatedDate = DateTime.Now
            };
            AuthorizeCaptureResponse authorizeCaptureResponse = new AuthorizeCaptureResponse
            {
                clientSecret = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL"
            };
            //Act
            this.mockStripePaymentsRepository.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>()));
            var response =  authorizeCapturePaymentGateway.Capture(paymentIntentCreateRequest);
            //Assert
            Assert.IsNotNull(response);

        }

    }
}
