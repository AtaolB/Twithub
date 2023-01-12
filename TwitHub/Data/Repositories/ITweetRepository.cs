using System;
using System.Collections.Generic;
using TwitHub.Data.Entities;
using TwitHub.Models;

namespace TwitHub.Data.Repositories
{
    public interface ITweetHubRepository<T> : IDisposable
    {
        IEnumerable<T> Listing();

        T GetContentById(Guid Id);

        void Insert(T entity);

        void Delete(T entity);

        void Update(T entity);

        void Save();
    }
}
