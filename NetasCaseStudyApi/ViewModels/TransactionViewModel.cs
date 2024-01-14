using System.Text.Json.Serialization;

namespace NetasCaseStudyApi.ViewModels
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public int BankId { get; set; }
        public int TotalAmount { get; set; }
        public int NetAmount { get; set; }
        public string? Status { get; set; }
        public string? OrderReferance { get; set; }
        public DateTime TransactionDate { get; set; }
        [JsonIgnore]
        public IList<TransactionDetailViewModel> TransactionDetailViewModel { get; set; } = new List<TransactionDetailViewModel>();
    }
}
