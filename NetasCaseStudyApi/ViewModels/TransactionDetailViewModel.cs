namespace NetasCaseStudyApi.ViewModels
{
    public class TransactionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string? TransactionType { get; set; }
        public string? Status { get; set; }
        public int Amount { get; set; }
    }
}
