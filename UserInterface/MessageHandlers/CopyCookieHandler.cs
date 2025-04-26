using Microsoft.Net.Http.Headers;

namespace UserInterface.MessageHandlers;

public class CopyCookieHandler(IHttpContextAccessor context) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
		CancellationToken cancellationToken)
	{
		//Если есть куки копируем их в последующий запрос
		if (context.HttpContext.Request.Cookies.Count > 0)
		{
			var cookieHeader = string.Join("; ",
				context.HttpContext.Request.Cookies.Select(c => $"{c.Key}={c.Value}"));
			request.Headers.Add(HeaderNames.Cookie, cookieHeader);
		}

		var response = await base.SendAsync(request, cancellationToken);

		return response;
	}
}