using System.Net;
using Database.DTO;
using Database.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Database.Middlewares;

/// <summary>
/// Делает коммит транзакции после успешного ответа на запрос пользователя.
/// В случае ошибки при коммите транзакции перезаписывает HttpResponse.
/// </summary>
public class DatabaseTransactionMiddleware : IMiddleware
{
    private readonly IBaseUnitOfWork _unitOfWork;
    private readonly ILogger<DatabaseTransactionMiddleware> _logger;

    public DatabaseTransactionMiddleware(IBaseUnitOfWork unitOfWork, ILogger<DatabaseTransactionMiddleware> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _unitOfWork.BeginTransaction();
        
        try
        {
            context.Response.OnStarting(async () => await CommitIfSuccess(context));
            await next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            _unitOfWork.RollbackTransaction();
        }
    }

    private async Task CommitIfSuccess(HttpContext context)
    {
        try
        {
            if (!ResponseSuccessful(context.Response))
                return;

            _unitOfWork.Commit();
            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            _logger.LogError(ex.InnerException?.Message, ex.InnerException);
            _unitOfWork.RollbackTransaction();
            await HandleExceptionAsync(context, ex);
        }
    }

    private bool ResponseSuccessful(HttpResponse httpResponse)
    {
        if ((httpResponse.StatusCode is >= 200 and <= 299 || httpResponse.StatusCode is 302))
            return true;

        return false;
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string errorMessage = JsonConvert.SerializeObject(new TransactionCommitErrorResponse()
        {
            Error = "Произошла ошибка при коммите транзакции"
        });
        
        context.Response.Clear();
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.ContentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(errorMessage);
        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
        
        await context.Response.WriteAsync(errorMessage);
    }
}