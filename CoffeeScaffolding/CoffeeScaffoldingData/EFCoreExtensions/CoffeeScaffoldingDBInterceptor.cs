using CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoffeeScaffolding.CoffeeScaffoldingData.EFCoreExtensions
{
    public class CoffeeScaffoldingDBInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SavingDomainEntities(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SavingDomainEntities(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void SavingDomainEntities(DbContextEventData eventData)
        {
            if (eventData.Context != null)
            {
                var Entries = eventData.Context.ChangeTracker.Entries();

                foreach (var Entry in Entries)
                {
                    if (Entry.Entity is BaseWithConcurrencyTable)
                    {
                        var BaseWithConcurrencyTable = (BaseWithConcurrencyTable)Entry.Entity;
                        BaseWithConcurrencyTable.Save(Entry);
                    }
                    else if (Entry.Entity is BaseTable)
                    {
                        var BaseTable = (BaseTable)Entry.Entity;
                        BaseTable.Save(Entry);
                    }
                }
            }
        }
    }
}
