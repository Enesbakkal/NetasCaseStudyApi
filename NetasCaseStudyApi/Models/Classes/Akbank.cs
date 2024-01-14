using NetasCaseStudyApi.Exceptions;
using NetasCaseStudyApi.ViewModels;

namespace NetasCaseStudyApi.Models.Classes
{
    public class Akbank : Bank
    {
        public Akbank() : base(3)
        {

        }

        public override PayInputViewModel Pay(PayInputViewModel paymentTrasaction)
        {
            if (paymentTrasaction.TotalAmount - paymentTrasaction.NetAmount != 0)
            {
                throw new PaymentException("Total Amount and Net Amount should be same value.");
            }
            if (paymentTrasaction.TotalAmount - paymentTrasaction.Amount < 0)
            {
                throw new PaymentException("Total Amount should be bigger than Amount");
            }
            if (paymentTrasaction.NetAmount - paymentTrasaction.Amount < 0)
            {
                throw new PaymentException("Net Amount should be bigger than Amount");
            }
            paymentTrasaction.TotalAmount = paymentTrasaction.TotalAmount;
            paymentTrasaction.NetAmount = paymentTrasaction.TotalAmount - paymentTrasaction.Amount;
            paymentTrasaction.Amount = paymentTrasaction.Amount;
            paymentTrasaction.BankId = this.BankId;
            return paymentTrasaction;
        }

        public override Transactions.Transactions Cancel(Transactions.Transactions cancelTrasaction)
        {
            DateTime ifItInTheSameDay = DateTime.Now;

            // Get the starting time of the day (00:00:00.000)
            DateTime startTimeFoDay = new DateTime(ifItInTheSameDay.Year, ifItInTheSameDay.Month, ifItInTheSameDay.Day, 0, 0, 0, 0);

            // Get the end time of the day (23:59:59.999)
            DateTime endTimeOfDay = new DateTime(ifItInTheSameDay.Year, ifItInTheSameDay.Month, ifItInTheSameDay.Day, 23, 59, 59, 999);

            if (startTimeFoDay >= cancelTrasaction.TransactionDate && cancelTrasaction.TransactionDate >= endTimeOfDay)
            {
                throw new DateMismatchException();
            }
            cancelTrasaction.NetAmount = 0;

            return cancelTrasaction;
        }

        public override Transactions.Transactions Refund(Transactions.Transactions refundTransaction)
        {
            DateTime currentDate = refundTransaction.TransactionDate;

            DateTime nextDay = currentDate.AddDays(1);

            if (DateTime.Now > nextDay)
            {
                throw new DateMismatchException();
            }

            refundTransaction.NetAmount = 0;

            return refundTransaction;
        }
    }
}
