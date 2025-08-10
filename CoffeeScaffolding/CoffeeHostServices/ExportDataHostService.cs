using CoffeeScaffolding.CoffeeScaffoldingData;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CoffeeScaffolding.CoffeeHostServices
{
    public class ExportDataHostService : BackgroundService
    {
        private readonly IServiceScope? ServiceScope;
        private readonly CoffeeScaffoldingDBContext? dbContext;
        public ExportDataHostService(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
            dbContext = ServiceScope.ServiceProvider.GetService<CoffeeScaffoldingDBContext>();
        }
        public override void Dispose()
        {
            base.Dispose();
            dbContext?.Dispose();
            ServiceScope?.Dispose();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (dbContext != null)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var users = await dbContext.CoffeeUser.ToListAsync();
                        foreach (var item in users)
                        {
                            await File.WriteAllTextAsync(@"D:\temp\text.txt", item.ToString(), Encoding.UTF8);
                        }
                    }
                    Console.WriteLine("Export end");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
