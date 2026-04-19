using System.Threading;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models.Aggregator.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DAL.Interceptors
{
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) 
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in context.ChangeTracker.Entries<ISoftDeletableOfAggregator>())
            {
                if (entry.State != EntityState.Deleted) continue;

                entry.State = EntityState.Modified;
                var entity = entry.Entity;
                entity.Delete();
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

            foreach (var entry in context.ChangeTracker.Entries<ISoftDeletableOfAggregator>())
            {
                if (entry.State != EntityState.Deleted) continue;

                entry.State = EntityState.Modified;
                var entity = entry.Entity;
                entity.Delete();
            }

            return base.SavingChanges(eventData, result);
        }
    }

}
