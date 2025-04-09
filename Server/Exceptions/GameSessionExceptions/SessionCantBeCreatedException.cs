namespace Server.Exceptions.GameSessionExceptions;

public class SessionCantBeCreatedException : Exception
{
	public SessionCantBeCreatedException(string message) : base(message)
	{
		
	}
}