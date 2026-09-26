using EventaroApi.Data;
using EventaroApi.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace EventaroApi.Services
{
    public class EventStatusUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventStatusUpdateService> _logger;

        public EventStatusUpdateService(IServiceProvider serviceProvider, ILogger<EventStatusUpdateService> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           using(var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var now = DateTime.UtcNow;
                var eventsUpdate = 0;

                var eventToComplete = await context.Events.Where(e => e.EventEnd < now && (e.StatusEvent == StatusEvent.Draft || e.StatusEvent == StatusEvent.Published)).ToListAsync(stoppingToken);

                foreach(var evt in eventToComplete)
                {
                    evt.StatusEvent = StatusEvent.Completed;
                    eventsUpdate++;
                }

                var eventToPublish = await context.Events.Where(e => e.EventStart <= now && e.EventEnd >= now && e.StatusEvent == StatusEvent.Draft).ToListAsync(stoppingToken);
                foreach(var evt in eventToPublish)
                {
                    evt.StatusEvent = StatusEvent.Published;
                    eventsUpdate++;
                }

                if(eventsUpdate > 0)
                {
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"{eventsUpdate} evento(s) actualizado(s)");
                }
            }
        }
    }
}
