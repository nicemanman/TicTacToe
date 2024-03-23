using Database.Interfaces;
using Server.DataModel;

namespace Server.Data.Interfaces;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IRepository<Game> GamesRepository { get; set; }
}