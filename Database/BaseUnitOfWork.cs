using System.Diagnostics;
using Database.Interfaces;
using Database.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using LogLevel = NLog.LogLevel;

namespace Database;

public class BaseUnitOfWork : DbContext, IBaseUnitOfWork
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private bool _isDisposed;
    
    public string ConnectionString { get; set; }
    public bool UseAutoMigration { get; set; }
    
    public BaseUnitOfWork(IConfiguration configuration)
    {
        ConnectionString = configuration.GetSection(DbConnectionOptions.ConnectionStringConfigString).Value;
        UseAutoMigration = configuration.GetSection(DbConnectionOptions.UseAutoMigrationConfigString).Get<bool>();

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.Log(LogLevel.Trace, $"New UnitOfWork with ContextId {ContextId} has been created. " +
                                        $"Connection string is set to {ConnectionString}, " +
                                        $"Automigrations set to {UseAutoMigration}");
        }
    }


    public bool IsDisposed
    {
        get => _isDisposed;
    }

    public void Commit()
    {
        SaveChanges();
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.Log(LogLevel.Trace, $"UnitOfWork with ContextId {ContextId} has saved some changes");
    }

    public void BeginTransaction()
    {
        if (Database.CurrentTransaction == null)
            Database.BeginTransaction();
        
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.Log(LogLevel.Trace, $"UnitOfWork with ContextId {ContextId} has started transaction {Database.CurrentTransaction.TransactionId}");
    }

    public void RollbackTransaction()
    {
        if (Database.CurrentTransaction is null) 
            return;
        
        Database.RollbackTransaction();
        var currentTransactionId = Database.CurrentTransaction?.TransactionId;
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.Log(LogLevel.Trace, $"UnitOfWork with ContextId {ContextId} has canceled transaction {currentTransactionId}");
    }

    public void CommitTransaction()
    {
        if (Database.CurrentTransaction is null) 
            return;
        
        Guid? currentTransactionId = Database.CurrentTransaction?.TransactionId;
        Database.CommitTransaction();
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.Log(LogLevel.Trace, $"UnitOfWork with ContextId {ContextId} has commited transaction {currentTransactionId}");
    }

    public override void Dispose()
    {
        _isDisposed = true;
        base.Dispose();
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.Log(LogLevel.Trace, $"UnitOfWork with ContextId {ContextId} has been disposed. StackTrace - {new StackTrace()}");
    }
}