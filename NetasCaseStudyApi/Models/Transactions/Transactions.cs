using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetasCaseStudyApi.Models.Transactions
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public int BankId { get; set; }
        public int TotalAmount { get; set; }
        public int NetAmount { get; set; }
        public string? Status { get; set; }
        public string? OrderReferance { get; set; }
        public DateTime TransactionDate { get; set; }
        public ICollection<TransactionDetails> TransactionDetails { get; set; } = new List<TransactionDetails>();
    }
}
