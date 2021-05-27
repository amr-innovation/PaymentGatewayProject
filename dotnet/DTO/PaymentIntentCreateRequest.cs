using System.Collections.Generic;

namespace server.Models
{
    public class PaymentIntentCreateRequest
    {
        public long? Amount { get; set; }

        public string Currency { get; set; }

        public List<string> PaymentMethodTypes { get; set; }
        public string CaptureMethod { get; set; }

        public string Payment_id { get; set; }
    }
}
