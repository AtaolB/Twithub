using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TwitHub.Data;
using TwitHub.Data.Entities;
using TwitHub.Data.Repositories;

namespace ContosoUniversity.DAL
{
    public class GenericTweetHubRepository<T> : IDisposable, ITweetHubRepository<T> where T : class
    {
        private protected ApplicationDbContext _context;

        public GenericTweetHubRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._contextSet = _context.Set<T>();
        }
        public virtual DbSet<T> _contextSet { get; private set; }

        public IEnumerable<T> Listing()
        {
            return this._contextSet.ToList();
        }

        public T GetContentById(Guid Id)
        {
            return this._contextSet.Find(Id);
        }

        public void Insert(T Entity)
        {
            _contextSet.Add(Entity);
        }

        public void Delete(T Entity)
        {
            T FindEntity = _contextSet.Find(Entity);
            _contextSet.Remove(FindEntity);
        }

        public void Update(T Entity)
        {
            T FindEntity = _contextSet.Find(Entity);
            _contextSet.Update(FindEntity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}