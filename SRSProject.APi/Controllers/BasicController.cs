using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagment.Application.common;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        protected IActionResult HandleResult<TValue>(
           Result<TValue> result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Ok(result);
        }


        protected IActionResult HandleResult<TValue>(
            Result<TValue> result,
            Func<TValue, IActionResult> onSuccess)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return onSuccess(result.Value);
        }


        protected IActionResult HandleResult(Result result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return NoContent();
        }


        protected IActionResult HandleCreatedResult<TValue>(
            Result<TValue> result,
            string actionName,
            Func<TValue, object?> routeValuesFactory)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(actionName);
            ArgumentNullException.ThrowIfNull(routeValuesFactory);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return CreatedAtAction(
                actionName,
                routeValuesFactory(result.Value),
                result.Value);
        }


        protected IActionResult HandleCreatedResult<TValue>(
            Result<TValue> result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return StatusCode(
                StatusCodes.Status201Created,
                result.Value);
        }


        protected IActionResult HandleAcceptedResult<TValue>(
            Result<TValue> result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Accepted(result.Value);
        }


        protected IActionResult HandleFailure(Error error)
        {
            ArgumentNullException.ThrowIfNull(error);

            if (error == Error.None)
            {
                throw new InvalidOperationException(
                    "A failed result must contain an error.");
            }

            if (error is ValidationError validationError)
            {
                return HandleValidationFailure(validationError);
            }

            int statusCode = GetStatusCode(error.Type);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(error.Type),
                Detail = error.Description,
                Instance = HttpContext.Request.Path
            };

            problemDetails.Extensions["code"] = error.Code;
            problemDetails.Extensions["traceId"] =
                HttpContext.TraceIdentifier;

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        private IActionResult HandleValidationFailure(
            ValidationError validationError)
        {
            var errors = validationError.Errors.ToDictionary(
                error => error.Key,
                error => error.Value);

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = validationError.Description,
                Instance = HttpContext.Request.Path
            };

            problemDetails.Extensions["code"] =
                validationError.Code;

            problemDetails.Extensions["traceId"] =
                HttpContext.TraceIdentifier;

            return BadRequest(problemDetails);
        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation =>
                    StatusCodes.Status400BadRequest,

                ErrorType.Unauthorized =>
                    StatusCodes.Status401Unauthorized,

                ErrorType.Forbidden =>
                    StatusCodes.Status403Forbidden,

                ErrorType.NotFound =>
                    StatusCodes.Status404NotFound,

                ErrorType.Conflict =>
                    StatusCodes.Status409Conflict,

                ErrorType.BusinessRule =>
                    StatusCodes.Status422UnprocessableEntity,

                ErrorType.Failure =>
                    StatusCodes.Status500InternalServerError,

                ErrorType.None =>
                    throw new InvalidOperationException(
                        "ErrorType.None cannot be converted to an HTTP response."),

                _ =>
                    StatusCodes.Status500InternalServerError
            };
        }

        private static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation =>
                    "Validation error",

                ErrorType.Unauthorized =>
                    "Authentication required",

                ErrorType.Forbidden =>
                    "Access forbidden",

                ErrorType.NotFound =>
                    "Resource not found",

                ErrorType.Conflict =>
                    "Resource conflict",

                ErrorType.BusinessRule =>
                    "Business rule violation",

                ErrorType.Failure =>
                    "Server error",

                ErrorType.None =>
                    throw new InvalidOperationException(
                        "ErrorType.None cannot have an error title."),

                _ =>
                    "Server error"
            };
        }
    }
}
