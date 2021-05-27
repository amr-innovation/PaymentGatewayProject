using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{

    /// <summary>
    /// This is the base controller
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderInformationControllerBase" /> class.
        /// </summary>
        /// <param name="logger">Reference to ILogger.</param>
        public BaseController(ILogger<BaseController> logger) => Logger = logger;

        /// <summary>
        ///     Gets Logger member.
        /// </summary>
        protected ILogger<BaseController> Logger { get; }
    }
}
