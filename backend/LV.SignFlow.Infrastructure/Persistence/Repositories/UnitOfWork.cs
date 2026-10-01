using LV.SignFlow.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDb)
        {
            _appDbContext = appDb;
        }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _appDbContext.SaveChangesAsync(cancellationToken);        }
    }
}
