using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationEventLogEF.Utilities
{
    public class ResilientTransaction
    {
        private DbContext _context;
        private ResilientTransaction(DbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    await action();
                    transaction.Commit();
                }
            });
        }
    }
}
