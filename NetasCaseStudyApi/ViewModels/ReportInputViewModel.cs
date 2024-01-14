namespace NetasCaseStudyApi.ViewModels
{
    public class ReportInputViewModel
    {
        public int? BankId { get; set; }
        public string? TransactionStatus { get; set; }
        public string? OrderReferance { get; set; }
        public DateTime? TransactionDateStart { get; set; }
        public DateTime? TransactionDateEnd { get; set; }
    }
}
