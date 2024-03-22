namespace Database.Interfaces;

public interface IBaseUnitOfWork : IDisposable
{
    public string ConnectionString { get; set; }
    
    public bool IsDisposed { get; }
    
    public void Commit();
    
    public void BeginTransaction();

    public void RollbackTransaction();

    public void CommitTransaction();

    public void Migrate();
}