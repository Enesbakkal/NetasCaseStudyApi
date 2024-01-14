using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetasCaseStudyApi.Models.Transactions;
using NetasCaseStudyApi.ViewModels;

namespace NetasCaseStudyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDetailsController : ControllerBase
    {
        private readonly NetasCaseStudyContext _context;

        public TransactionDetailsController(NetasCaseStudyContext context)
        {
            _context = context;
        }

        // GET: api/TransactionDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDetails>>> GetTransactionDetails()
        {
            return await _context.TransactionDetails.ToListAsync();
        }

        // GET: api/TransactionDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDetails>> GetTransactionDetails(Guid id)
        {
            var transactionDetails = await _context.TransactionDetails.FindAsync(id);

            if (transactionDetails == null)
            {
                return NotFound();
            }

            return transactionDetails;
        }
    }
}
