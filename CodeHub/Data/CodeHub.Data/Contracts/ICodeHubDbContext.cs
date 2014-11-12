namespace CodeHub.Data.Contracts
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    using CodeHub.Data.Models;

    public interface ICodeHubDbContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Paste> Pastes { get; set; }

        IDbSet<Repo> Repos { get; set; }

        IDbSet<Syntax> Syntaxes { get; set; }

        IDbSet<Comment> Comments { get; set; }

        int SaveChanges();

        void Dispose();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}