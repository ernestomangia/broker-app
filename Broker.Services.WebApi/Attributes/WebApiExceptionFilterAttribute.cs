using System.Net;
using Broker.Application.Core.Exceptions;
using Broker.Infrastructure.Integration.Services.Core.Exceptions;
using Broker.Services.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Broker.Services.WebApi.Attributes;

public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var result = new ExceptionModel
        {
            Message = context.Exception.Message,
            DateTime = DateTime.UtcNow
        };

        if (context.Exception is DataValidationException
            || context.Exception is ApiServiceException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            result.StatusCode = HttpStatusCode.BadRequest;
        }
        else
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            result.StatusCode = HttpStatusCode.InternalServerError;
        }

        context.Result = new JsonResult(result);
    }
}