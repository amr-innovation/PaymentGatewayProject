using System;
using System.Collections.Generic;

#nullable disable

namespace Entity.Entities
{
    public partial class Transaction
    {
        public string Id { get; set; }
        public double? Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
    }
}
