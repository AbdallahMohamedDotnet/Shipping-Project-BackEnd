using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShippingContext ctx;
        private readonly ConcurrentDictionary<Type, object> repositories = new();
        private IDbContextTransaction? tx;
        private readonly ILoggerFactory loggerFactory;

        public UnitOfWork(ShippingContext ctx, ILoggerFactory loggerFactory)
        {
            this.ctx = ctx;
            this.loggerFactory = loggerFactory;
        }

        public ITableRepository<T> Repository<T>() where T : BaseTable
        {
            return (ITableRepository<T>)repositories.GetOrAdd(
                typeof(T),
                _ => new TableRepository<T>(
                        ctx,
                        loggerFactory.CreateLogger<TableRepository<T>>()));
        }
        public async Task BeginTransactionAsync()
            => tx = await ctx.Database.BeginTransactionAsync();

        public async Task CommitAsync()
        {
            await ctx.SaveChangesAsync();
            if (tx is not null) await tx.CommitAsync();
        }

        public async Task RollbackAsync()
            => await tx?.RollbackAsync()!;

        public Task<int> SaveChangesAsync() => ctx.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            if (tx is not null) await tx.DisposeAsync();
            await ctx.DisposeAsync();
        }
    }

}
