using System;
namespace Infrastructure.Mediator
{
    public class ApiResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }

        public int HttpStatusCode { get; }
        public TValue Value { get; }
        public string ErrorCode { get; }

        internal ApiResult(TValue value, bool isSuccessful, int httpStatusCode, string errorDescription, string errorCode = null)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

        public static ApiResult<TValue> Ok(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.OK, null);
        }

        public static ApiResult<TValue> Accepted(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.Accepted, null);
        }

        public static ApiResult<TValue> Created(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.Created, null);
        }

        public static ApiResult<TValue> NoContent()
        {
            return new ApiResult<TValue>(default(TValue), true, (int)System.Net.HttpStatusCode.NoContent, null);
        }

        public static ApiResult<TValue> Forbidden(string errorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Forbidden, errorDescription);
        }

        public static ApiResult<TValue> Unauthorized(string errorDescription = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Unauthorized, "No access");
        }

        public static ApiResult<TValue> BadRequest(string errorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, errorDescription);
        }

        public static ApiResult<TValue> ErrorDuringProcessing(string errorDescription = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.InternalServerError, "Something went wrong during the execution of the request. Please contact the support department.", null);
        }

        public static ApiResult<TValue> BadRequest(string errorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, errorDescription, errorCode);
        }

        public static ApiResult<TValue> NotFound(string errorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.NotFound, errorDescription, errorCode);
        }

        public static ApiResult<TValue> InternalServerError(string errorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.InternalServerError, errorDescription);
        }

        public static ApiResult<TValue> NotAcceptable(string errorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.NotAcceptable, errorDescription);
        }

        public static ApiResult<TValue> ValidationError(string errorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, 460, errorDescription, errorCode);
        }

        public static ApiResult<TValue> Fail(int httpStatusCode, string errorDescription, string errorCode = null, TValue result = default(TValue))
        {
            return new ApiResult<TValue>(result, false, httpStatusCode, errorDescription, errorCode);
        }

        public static ApiResult<TValue> Translate(ApiResult result, TValue value)
        {
            return new ApiResult<TValue>(value, result.IsSuccessful, result.HttpStatusCode, result.ErrorDescription);
        }
    }

    public class ApiResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }

        public int HttpStatusCode { get; }
        public string ErrorCode { get; }

        internal ApiResult(bool isSuccessful, int httpStatusCode, string errorDescription, string errorCode = null)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

        public static ApiResult Ok()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.OK, null);
        }

        public static ApiResult Accepted()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.Accepted, null);
        }

        public static ApiResult Created()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.Created, null);
        }

        public static ApiResult NoContent()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.NoContent, null);
        }

        public static ApiResult Forbidden(string errorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Forbidden, errorDescription);
        }

        public static ApiResult Unauthorized(string errorDescription = null)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Unauthorized, "No access");
        }

        public static ApiResult BadRequest(string errorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.BadRequest, errorDescription);
        }

        public static ApiResult NotFound(string errorDescription, string errorCode = null)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.NotFound, errorDescription, errorCode);
        }

        public static ApiResult InternalServerError(string errorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.InternalServerError, errorDescription);
        }

        public static ApiResult Fail(int httpStatusCode, string errordescription, string errorCode = null)
        {
            return new ApiResult(false, httpStatusCode, errordescription, errorCode);
        }

        public static ApiResult ValidationError(string errorDescription, string errorCode = null)
        {
            return new ApiResult(false, 460, errorDescription, errorCode);
        }

        public static ApiResult Translate<TValue>(ApiResult<TValue> result)
        {
            return new ApiResult(result.IsSuccessful, result.HttpStatusCode, result.ErrorDescription);
        }
    }
}
