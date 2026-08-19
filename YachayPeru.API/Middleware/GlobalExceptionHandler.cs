using Microsoft.AspNetCore.Http.HttpResults;
using YachayPeru.API.Contracts.Common;
using YachayPeru.Application.Common.Exceptions;
using UnauthorizedException = YachayPeru.Application.Common.Exceptions.UnauthorizedException;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace YachayPeru.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger) {
            _next = next;
            _logger= logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var requestBody = await ReadRequestBodyAsync(context);

            var originalResponseBodyStream = context.Response.Body;

            await using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;
            try
            {
                await _next(context);

                stopwatch.Stop();

                var responseBody = await ReadResponseBodyAsync(context.Response);

                _logger.LogInformation(
                    """
                    HTTP Request/Response
                    Method: {Method}
                    Path: {Path}
                    QueryString: {QueryString}
                    StatusCode: {StatusCode}
                    ElapsedMs: {ElapsedMs}
                    TraceId: {TraceId}
                    RequestBody: {RequestBody}
                    ResponseBody: {ResponseBody}
                    """,
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    context.TraceIdentifier,
                    requestBody,
                    responseBody);

               await CopyResponseToOriginalStreamAsync(responseBodyStream, originalResponseBodyStream);

            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                context.Response.Body = originalResponseBodyStream;
                context.Response.Clear();
                LogException(context, ex);
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            int statusCode;
            object response;

            switch (ex)
            {
                case ValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.Error(
                        "Se encontraron errores de validación.",null,
                        validationException.Errors
                    );
                    break;

                case BusinessException businessException:
                    statusCode = StatusCodes.Status409Conflict;
                    response = ApiResponse<object>.Error(businessException.Message);
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    response = ApiResponse<object>.Error(
                        unauthorizedException.Message, null,
                        new[] { unauthorizedException.Code }
                    );
                    break;

                case KeyNotFoundException keyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    response = ApiResponse<object>.Error(keyNotFoundException.Message);
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    response = ApiResponse<object>.Error("Ocurrió un error interno del servidor.");
                    break;
            }

            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response, _jsonOptions);
            await context.Response.WriteAsync(json);
        }

        private void LogException(HttpContext context, Exception ex)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var traceId = context.TraceIdentifier;

            switch (ex)
            {
                case ValidationException validationException:
                    _logger.LogWarning(
                        validationException,
                        "Error de validación en {Method} {Path}. Code: {Code}. TraceId: {TraceId}. Errors:{Errors}",
                        method,
                        path,
                        validationException.Code,
                        traceId,
                        validationException.Errors);
                    break;

                case BusinessException businessException:
                    _logger.LogWarning(
                        businessException,
                        "Error de negocio en {Method} {Path}. Code: {Code}. TraceId: {TraceId}. Mensaje:{Message}. Errors:{Errors}",
                        method,
                        path,
                        businessException.Code,
                        traceId,
                        businessException.Message,
                        businessException.Errors);
                    break;

                case UnauthorizedException unauthorizedException:
                    _logger.LogWarning(
                        unauthorizedException,
                        "No autorizado en {Method} {Path}. Code: {Code}. TraceId: {TraceId}",
                        method,
                        path,
                        unauthorizedException.Code,
                        traceId);
                    break;

                case KeyNotFoundException:
                    _logger.LogInformation(
                        ex,
                        "Recurso no encontrado en {Method} {Path}. TraceId: {TraceId}",
                        method,
                        path,
                        traceId);
                    break;

                default:
                    _logger.LogError(
                        ex,
                        "Excepción no controlada en {Method} {Path}. TraceId: {TraceId}. Mensaje:{Message}.",
                        method,
                        path,
                        traceId,ex.Message);
                    break;
            }
        }

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            return body;
        }

        private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        private static async Task CopyResponseToOriginalStreamAsync(
            MemoryStream responseBodyStream,
            Stream originalResponseBodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }
}
