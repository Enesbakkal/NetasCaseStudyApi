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
    public class TransactionsController : ControllerBase
    {
        private readonly NetasCaseStudyContext _context;

        public TransactionsController(NetasCaseStudyContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transactions>> GetTransactions(Guid id)
        {
            var transactions = await _context.Transactions.FindAsync(id);

            if (transactions == null)
            {
                return NotFound();
            }

            return transactions;
        }
    }
}
