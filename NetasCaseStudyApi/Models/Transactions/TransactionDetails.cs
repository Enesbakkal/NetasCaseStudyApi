using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetasCaseStudyApi.Models.Transactions
{
    public class TransactionDetails
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string? TransactionType { get; set; }
        public string? Status { get; set; }
        public int Amount { get; set; }
        [JsonIgnore]
        public Transactions? Transaction { get; set; }

    }
}
