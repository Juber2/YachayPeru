using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Common;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.API.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult FromResult(this ControllerBase controller, Result result)
        {
            if (result.IsSuccess)
                return controller.Ok(ApiResponse<object?>.Ok(null));

            var response = ApiResponse<object?>.Error(result.Message!, code: result.Code);
            return result.Code switch
            {
                ResultCodes.Conflict   => controller.Conflict(response),
                ResultCodes.NotFound   => controller.NotFound(response),
                ResultCodes.Validation => controller.BadRequest(response),
                _                      => controller.BadRequest(response)
            };
        }

        public static IActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
        {
            if (result.IsSuccess)
                return controller.Ok(ApiResponse<T>.Ok(result.Value));

            var response = ApiResponse<T>.Error(result.Message!, code: result.Code);
            return result.Code switch
            {
                ResultCodes.Conflict   => controller.Conflict(response),
                ResultCodes.NotFound   => controller.NotFound(response),
                ResultCodes.Validation => controller.BadRequest(response),
                _                      => controller.BadRequest(response)
            };
        }
    }
}
