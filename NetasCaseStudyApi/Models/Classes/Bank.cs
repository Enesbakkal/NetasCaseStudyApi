using Microsoft.AspNetCore.Mvc;
using NetasCaseStudyApi.ViewModels;
using System.Net;

namespace NetasCaseStudyApi.Models.Classes
{
    public abstract class Bank
    {
        public int BankId { get; }
        public Bank(int bankId)
        {
            BankId = bankId;
        }

        public abstract PayInputViewModel Pay(PayInputViewModel paymentTrasaction);

        public abstract Transactions.Transactions Cancel(Transactions.Transactions cancelTrasaction);

        public abstract Transactions.Transactions Refund(Transactions.Transactions transactions);
    }
}
