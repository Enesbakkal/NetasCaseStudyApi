using Microsoft.EntityFrameworkCore;

namespace NetasCaseStudyApi.Models.Transactions
{
    public class NetasCaseStudyContext : DbContext
    {
        public NetasCaseStudyContext(DbContextOptions<NetasCaseStudyContext> options)
            :base(options)
        {
                
        }
        public DbSet<Transactions>? Transactions { get; set; }
        public DbSet<TransactionDetails>? TransactionDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionDetails>()
                .HasOne(c => c.Transaction)
                .WithMany(p => p.TransactionDetails)
                .HasForeignKey(c => c.TransactionId)
                .IsRequired();
        }
    }
}
