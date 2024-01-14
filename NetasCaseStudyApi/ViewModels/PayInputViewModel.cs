namespace NetasCaseStudyApi.ViewModels
{
    public class PayInputViewModel
    {
        public int BankId { get; set; }
        public int TotalAmount { get; set; } = 0;
        public int NetAmount { get; set; } = 0;
        public string? OrderReferance { get; set; }
        public int Amount { get; set; }
    }
}
