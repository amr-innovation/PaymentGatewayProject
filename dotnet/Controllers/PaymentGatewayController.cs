using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Models;
using server.Service;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{

    /// <summary>
    /// This is the main controller which contains the Authorize and Capture method
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : BaseController
    {
        private readonly IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway;

        public readonly IOptions<Entity.Configuration.StripeOptions> options;

        public PaymentGatewayController(IOptions<Entity.Configuration.StripeOptions> options, IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway, ILogger<PaymentGatewayController> logger) : base(logger)
        {
            this.options = options;
            this.authorizeCapturePaymentGateway = authorizeCapturePaymentGateway;
        }

        [HttpGet("getconfig")]
        public Config GetConfig()
        {
            return new Config
            {
                PublishableKey = options.Value.PublishableKey,
            };
        }

        [HttpPost("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<AuthorizeCaptureResponse>> Authorize(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));
            Logger.LogInformation("Authorize method");
            try
            {
                return await authorizeCapturePaymentGateway.Authorize(request);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize]
        [HttpPost("capture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<AuthorizeCaptureResponse>> Capture(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));
            Logger.LogInformation("Capture method");
            try
            {
                return await authorizeCapturePaymentGateway.Capture(request);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
