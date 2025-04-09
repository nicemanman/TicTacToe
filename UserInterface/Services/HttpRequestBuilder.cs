namespace UserInterface.Services;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

public static class HttpRequestBuilder
{
	public static HttpRequestMessage BuildRequest(
		HttpMethod method,
		string url,
		IDictionary<string, string>? headers = null,
		HttpContent? content = null)
	{
		var request = new HttpRequestMessage(method, url);

		if (headers != null)
		{
			foreach (var header in headers)
			{
				// Попробуем добавить как обычный заголовок
				if (request.Headers.TryAddWithoutValidation(header.Key, header.Value)) 
					continue;
				
				// Если не получилось — возможно это заголовок контента (например, Content-Type)
				if (content == null) 
					continue;
				
				if (!content.Headers.TryAddWithoutValidation(header.Key, header.Value))
					throw new InvalidOperationException($"Не удалось добавить заголовок: {header.Key}");
			}
		}

		if (content != null)
		{
			request.Content = content;
		}

		return request;
	}
}
