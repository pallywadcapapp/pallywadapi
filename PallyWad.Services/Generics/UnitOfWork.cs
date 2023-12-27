using Microsoft.EntityFrameworkCore;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Generics
{
    //[TransientRegistration]
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        //public DbContext Context { get; }
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }
    }

    public interface IUnitOfWork : IDisposable  //<TContext> where TContext : DbContext, new()
    {
        //TContext Init();
        void Commit();
        Task CommitAsync();
    }
}
