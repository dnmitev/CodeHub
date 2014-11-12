namespace CodeHub.Data.Contracts
{
    using CodeHub.Data.Common.Repositories;
    using CodeHub.Data.Models;

    public interface ICodeHubData
    {
        ICodeHubDbContext Context { get; }

        IRepository<User> Users { get; }

        IDeletableEntityRepository<Paste> Pastes { get; }

        IDeletableEntityRepository<Repo> Repos { get; }

        IDeletableEntityRepository<Syntax> Syntaxes { get; }

        IDeletableEntityRepository<Comment> Comments { get; }

        int SaveChanges();
    }
}