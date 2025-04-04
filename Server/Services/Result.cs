namespace Server.Services;

public class Result
{
	public bool Success { get; }
	
	public string Error { get; }
	
	public bool Failure => !Success;

	protected Result(bool success, string error)
	{
		if (success && !string.IsNullOrWhiteSpace(error))
			throw new InvalidOperationException();
		
		if (!success && string.IsNullOrWhiteSpace(error))
			throw new InvalidOperationException();
		
		Success = success;
		Error = error;
	}
	
	public static Result<T> Fail<T>(string message) => new Result<T>(default(T), false, message);
	
	public static Result<T> Ok<T>(T value) => new Result<T>(value, true, String.Empty);
}

public class Result<T> : Result
{
	public T Value { get; }

	public Result(T value, bool success, string error)
		: base(success, error)
	{
		Value = value;
	}
}