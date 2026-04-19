using System.Threading;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models.Aggregator.Interfaces;
using DAL.Models.Aggregator.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DAL.Interceptors
{
    public sealed class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) 
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is not AuditableEntityOfAggregator auditable) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.SetCreated();
                        break;

                    case EntityState.Modified:
                        auditable.SetUpdated();
                        break;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null) 
                return base.SavingChanges(eventData, result);

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is not AuditableEntityOfAggregator auditable) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.SetCreated();
                        break;

                    case EntityState.Modified:
                        auditable.SetUpdated();
                        break;
                }
            }

            return base.SavingChanges(eventData, result);
        }
    }

}
