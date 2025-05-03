using Core.Exceptions;
using Core.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (GuardUnauthorizedException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized user",
                Detail = GetJsonResponse(ex.ErrorCode, ex.Details)
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (GuardForbiddenException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden error",
                Detail = GetJsonResponse(ex.ErrorCode, ex.Details)
            };

            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (AppException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error occured on server",
                Detail = GetJsonResponse(ex.ErrorCode, ex.Details)
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (CustomExceptionBase ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad request",
                Detail = GetJsonResponse(ex.ErrorCode, ex.Details)
            };

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (Exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error occured on server"
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

            throw;
        }
    }

    private static string GetJsonResponse(ErrorCode errorCode, object? details = null)
    {
        var response = new
        {
            ErrorCode = errorCode,
            Details = details
        };

        return JsonSerializer.Serialize(response);
    }
}
