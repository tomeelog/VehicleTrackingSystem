using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.DataAccess.DbCtx;

namespace VehicleTrackingSystem.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        VehicleTrackingCommandsContext CommandsContext { get; }
        VehicleTrackingQueriesContext QueriesContext { get; }
        Task CommitAsync();
    }
}
