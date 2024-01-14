using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using NetasCaseStudyApi.Exceptions;
using NetasCaseStudyApi.Models.Classes;
using NetasCaseStudyApi.Models.Transactions;
using NetasCaseStudyApi.ViewModels;
using System.Drawing;
using System.Linq;
using System.Transactions;

namespace NetasCaseStudyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly NetasCaseStudyContext _context;

        private static readonly List<Bank> bankList = new List<Bank>
        {
            new YapiKredi(),
            new Garanti(),
            new Akbank()
        };

        public BanksController(NetasCaseStudyContext context)
        {
            _context = context;
        }
        
        [HttpPost("pay")]
        public async Task<ActionResult<PayOutputViewModel>> PayTransaction([FromBody] PayInputViewModel payInputViewModel)
        {
            var bank = bankList.FirstOrDefault(b => b.BankId == payInputViewModel.BankId);
            if (bank == null)
            {
                return BadRequest("Unknown BankId");
            }

            var bankTransaction = bank.Pay(payInputViewModel);

            
            Guid transactionId = Guid.NewGuid();
            Guid transactionDetailId = Guid.NewGuid();

            var transactionEntity = new Transactions
            {
                Id = transactionId,
                BankId = bankTransaction.BankId,
                TotalAmount = bankTransaction.TotalAmount,
                NetAmount = bankTransaction.NetAmount,
                Status = "Success",
                OrderReferance = bankTransaction.OrderReferance,
                TransactionDate = DateTime.Now,
            };

            var childEntity = new TransactionDetails
            {
                Id = transactionDetailId, 
                TransactionId = transactionId,
                TransactionType = "Sale",
                Status = "Success",
                Amount = bankTransaction.Amount,

            };
            transactionEntity.TransactionDetails.Add(childEntity);

            _context.Transactions.Add(transactionEntity);
            await _context.SaveChangesAsync();

            return Ok(new PayOutputViewModel { TransactionId = transactionId, TransactionDetailId = transactionDetailId}); 
        }

        [HttpPost("refund")]
        public async Task<ActionResult<RefundOutputViewModel>> RefundTransaction(Guid transactionId)
        {
            var transactionEntity = _context.Transactions.Where(t => t.Id == transactionId).FirstOrDefault();

            if (transactionEntity == null)
            {
                return BadRequest("No Transaction with this Id.");
            }

            var bank = bankList.FirstOrDefault(b => b.BankId == transactionEntity.BankId);
            if (bank == null)
            {
                return BadRequest("Unknown BankId");
            }

            Transactions updatedTransaction = null;

            try
            {
                updatedTransaction = bank.Refund(transactionEntity);
            }
            catch (DateMismatchException)
            {
                return BadRequest("You have to wait at least one day from the start time of pay transaction for refund transaction");
            }

            var childEntity = new TransactionDetails
            {
                Id = Guid.NewGuid(),
                TransactionId = updatedTransaction.Id,
                TransactionType = "Refund",
                Status = "Success",
                Amount = 0,
            };
            
            _context.TransactionDetails.Add(childEntity);
            _context.Transactions.Update(updatedTransaction);


            await _context.SaveChangesAsync();

            return Ok(new RefundOutputViewModel { TransactionId = updatedTransaction.Id, TransactionDetailId = childEntity.Id });
        }

        [HttpPost("cancel")]
        //[HttpPost("{bankId}")]
        public async Task<ActionResult<CancelOutputViewModel>> CancelTransaction(Guid transactionId)
        {

            var transactionEntity = _context.Transactions.Where(t => t.Id == transactionId).FirstOrDefault();

            if (transactionEntity == null)
            {
                return BadRequest("No Transaction with this Id.");
            }

            var bank = bankList.FirstOrDefault(b => b.BankId == transactionEntity.BankId);
            if (bank == null)
            {
                return BadRequest("Unknown BankId");
            }

            Transactions cancaledTransaction = null;

            try
            {
                cancaledTransaction = bank.Cancel(transactionEntity);
            }
            catch (DateMismatchException)
            {
                return BadRequest("You cant cancel this transaction because it is created today.");
            }

            var childEntity = new TransactionDetails
            {
                Id = Guid.NewGuid(),
                TransactionId = cancaledTransaction.Id,
                TransactionType = "Cancel",
                Status = "Success",
                Amount = 0,
            };

            _context.TransactionDetails.Add(childEntity);
            _context.Transactions.Update(cancaledTransaction);

            await _context.SaveChangesAsync();

            return Ok(new CancelOutputViewModel { TransactionId = cancaledTransaction.Id, TransactionDetailId = childEntity.Id });
        }

        [HttpPost("report")]
        //[HttpPost("{bankId}")]
        public async Task<ActionResult<IEnumerable<Transactions>>> ReportTransaction([FromBody] ReportInputViewModel reportInputViewModel)
        {
            if(_context.Transactions == null)
            {
                return Ok(new List<Transactions>());
            }

            IQueryable<Transactions> query = _context.Transactions;

            if (reportInputViewModel.BankId != null)
            {
                query = query.Where(t => t.BankId == reportInputViewModel.BankId);
            }

            if (reportInputViewModel.TransactionStatus != null)
            {
                query = query.Where(t => t.Status != null && t.Status.Contains(reportInputViewModel.TransactionStatus));
            }

            if (reportInputViewModel.OrderReferance != null)
            {
                query = query.Where(t => t.OrderReferance != null && t.OrderReferance.Contains(reportInputViewModel.OrderReferance));
            }

            if (reportInputViewModel.TransactionDateStart != null)
            {
                query = query.Where(t => t.TransactionDate >= reportInputViewModel.TransactionDateStart);
            }

            if (reportInputViewModel.TransactionDateEnd != null)
            {
                query = query.Where(t => t.TransactionDate <= reportInputViewModel.TransactionDateEnd);
            }

            return Ok(query.Include(i => i.TransactionDetails).ToList());
        }
    }
}
