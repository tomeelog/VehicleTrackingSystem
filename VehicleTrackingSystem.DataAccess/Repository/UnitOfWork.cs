using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.DataAccess.DbCtx;

namespace VehicleTrackingSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public VehicleTrackingCommandsContext CommandsContext { get; }
        public VehicleTrackingQueriesContext QueriesContext { get; }

        public UnitOfWork(VehicleTrackingCommandsContext commandsContext, VehicleTrackingQueriesContext queriesContext)
        {
            CommandsContext = commandsContext;
            QueriesContext = queriesContext;
        }

        public async Task CommitAsync()
        {
           await CommandsContext.SaveChangesAsync();
        }


        public void Dispose()
        {
            CommandsContext.Dispose();
            QueriesContext.Dispose();
        }
    }
}
