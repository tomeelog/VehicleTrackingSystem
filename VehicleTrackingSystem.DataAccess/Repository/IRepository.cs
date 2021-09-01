using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTrackingSystem.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task Add(T entity);
        Task Delete(T entity);
        void Update(T entity);
    }
}
