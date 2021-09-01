using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Settings;
using VehicleTrackingSystem.DataAccess.DbCtx;

namespace VehicleTrackingSystem.API
{
    public partial class Startup
    {
        private void AddVehicleTrackingSystemDependencies(IServiceCollection services, AppSettings settings)
        {
            if (settings.InMemoryDatabase)
            {
                services.AddDbContext<VehicleTrackingCommandsContext>(options => options.UseInMemoryDatabase("VehicleTrackingContext"));
                services.AddDbContext<VehicleTrackingQueriesContext>(options => options.UseInMemoryDatabase("VehicleTrackingContext"));
            }
            else
            {
                services.AddDbContextPool<VehicleTrackingCommandsContext>(options => options.UseSqlServer(settings.ConnectionStrings.SqlServer.Commands));
                services.AddDbContextPool<VehicleTrackingQueriesContext>(options => options.UseSqlServer(settings.ConnectionStrings.SqlServer.Queries));
            }

        }
    }
}
