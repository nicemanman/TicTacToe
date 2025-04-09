namespace Server.Middlewares;

public class ImplicitRegistrationMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		string id = context.Session.GetString(TicTacToeConstants.UserIdField)!;
		
		if (string.IsNullOrWhiteSpace(id))
		{
			context.Session.SetString(TicTacToeConstants.UserIdField, Guid.NewGuid().ToString());
			context.Session.SetString(TicTacToeConstants.UserNameField, "Аноним");
		}

		await next(context);
	}
}