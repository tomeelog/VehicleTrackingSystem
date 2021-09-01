using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTrackingSystem.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _unitOfWork.QueriesContext.Set<T>().ToListAsync();
            //return _unitOfWork.QueriesContext.Set<T>().AsEnumerable<T>();
        }

        public async Task Add(T entity)
        {
            await _unitOfWork.CommandsContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            T existing = await _unitOfWork.QueriesContext.Set<T>().FindAsync(entity);
            if (existing != null) _unitOfWork.CommandsContext.Set<T>().Remove(existing);
        }
        public void Update(T entity)
        {
            _unitOfWork.CommandsContext.Entry(entity).State = EntityState.Modified;
            _unitOfWork.CommandsContext.Set<T>().Update(entity);
        }
    }
}
