namespace CodeHub.Data
{
    using System;
    using System.Collections.Generic;

    using CodeHub.Data.Common.Models;
    using CodeHub.Data.Common.Repositories;
    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;
    using CodeHub.Data.Repositories;

    public class CodeHubData : ICodeHubData
    {
        private readonly ICodeHubDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public CodeHubData(ICodeHubDbContext context)
        {
            this.context = context;
        }

        public ICodeHubDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IDeletableEntityRepository<Paste> Pastes
        {
            get
            {
                return this.GetDeletableEntityRepository<Paste>();
            }
        }

        public IDeletableEntityRepository<Repo> Repos
        {
            get
            {
                return this.GetDeletableEntityRepository<Repo>();
            }
        }

        public IDeletableEntityRepository<Syntax> Syntaxes
        {
            get
            {
                return this.GetDeletableEntityRepository<Syntax>();
            }
        }

        public IDeletableEntityRepository<Comment> Comments
        {
            get
            {
                return this.GetDeletableEntityRepository<Comment>();
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        private IDeletableEntityRepository<T> GetDeletableEntityRepository<T>() where T : class, IDeletableEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DeletableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}