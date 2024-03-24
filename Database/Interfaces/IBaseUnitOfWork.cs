namespace Database.Interfaces;

/// <summary>
/// Базовый UnitOfWork
/// </summary>
public interface IBaseUnitOfWork : IDisposable
{
    /// <summary>
    /// Строка подключения к БД
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// Был ли освобожден UnitOfWork
    /// </summary>
    public bool IsDisposed { get; }
    
    /// <summary>
    /// Применить изменения
    /// </summary>
    public void Commit();
    
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    public void BeginTransaction();

    /// <summary>
    /// Отменить транзакцию
    /// </summary>
    public void RollbackTransaction();

    /// <summary>
    /// Применить транзакцию
    /// </summary>
    public void CommitTransaction();

    /// <summary>
    /// Выполнить миграцию
    /// </summary>
    public void Migrate();
}