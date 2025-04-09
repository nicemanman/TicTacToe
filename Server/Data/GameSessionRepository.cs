using System.Linq.Expressions;
using Database;
using Microsoft.EntityFrameworkCore;
using Server.DataModel;

namespace Server.Data;

public class GameSessionRepository : Repository<GameSession>
{
	public GameSessionRepository(DbSet<GameSession> dbSet) : base(dbSet)
	{
	}

	public override async Task<GameSession> FirstOrDefault(Expression<Func<GameSession, bool>> expression)
	{
		return await _dbSet
			.Include(x=>x.Game)
			.FirstOrDefaultAsync(expression);
	}
}