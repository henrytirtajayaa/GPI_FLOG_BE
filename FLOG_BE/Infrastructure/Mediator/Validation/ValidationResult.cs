using System;
namespace Infrastructure.Mediator
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }

        public int HttpStatusCode { get; }
        public string Error;

        internal ValidationResult(bool isSuccessful, int httpStatusCode, string errorDescription, string errorCode)
        {
            IsSuccessful = isSuccessful;
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, (int)System.Net.HttpStatusCode.OK, null, null);
        }

        public static ValidationResult Forbidden(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Forbidden, errorDescription, errorCode);
        }

        public static ValidationResult Unauthorized(string errorDescription = null, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Unauthorized, "No access", errorCode);
        }

        public static ValidationResult BadRequest(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.BadRequest, errorDescription, errorCode);
        }

        public static ValidationResult NotFound(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotFound, errorDescription, errorCode);
        }

        public static ValidationResult InternalServerError(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.InternalServerError, errorDescription, errorCode);
        }

        public static ValidationResult NotAcceptable(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotAcceptable, errorDescription, errorCode);
        }

        public static ValidationResult ValidationError(string errorDescription, string errorCode = null)
        {
            return new ValidationResult(false, 460, errorDescription, errorCode);
        }

        public static ValidationResult ErrorDuringProcessing(string errorDescription = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.InternalServerError, "Something went wrong during the execution of the request. Please contact the support department.", null);
        }

    }
}
