using Database;
using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Server.Data.Interfaces;
using Server.DataModel;

namespace Server.Data;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    
    public DbSet<Game> Games { get; set; }
    
    public DbSet<GameSession> GameSessions { get; set; }
    
    public IRepository<Game> GamesRepository { get; set; }
    
    public IRepository<GameSession> GameSessionRepository { get; set; }
    
    public UnitOfWork(IConfiguration configuration, ILogger<UnitOfWork> logger) : base(configuration)
    {
        _logger = logger;
        GamesRepository = new Repository<Game>(Games);
        GameSessionRepository = new GameSessionRepository(GameSessions);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().OwnsOne(x => x.GameMap, builder =>
        {
            builder.OwnsMany(x => x.Fields);
        });
        modelBuilder.Entity<Game>().HasKey(x => x.UUID);
        modelBuilder.Entity<Game>()
            .Property(x => x.UUID)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Game>()
            .Property(x=>x.State)
            .HasConversion(
                v => v.ToString(),
                v => (GameState)Enum.Parse(typeof(GameState), v));

        modelBuilder.Entity<GameSession>().HasKey(x => x.UUID);
        modelBuilder.Entity<GameSession>()
            .HasOne(x => x.Game);
        modelBuilder.Entity<GameSession>().Property(x=>x.UUID)
            .ValueGeneratedOnAdd();
        
        base.OnModelCreating(modelBuilder);
    }
}