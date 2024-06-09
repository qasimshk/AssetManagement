using System.Net;

namespace assetmgmt.core.Common.Results
{
    public class Result
    {   
        public Result(bool isSuccess, List<string>? errorMessages, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
            StatusCode = statusCode;
        }

        public bool IsSuccess { get; }
        public List<string>? ErrorMessages { get; }
        public HttpStatusCode StatusCode { get; }

        public static Result SuccessResult() => new(true, null, HttpStatusCode.OK);

        public static Result FailedResult(List<string> errorMessages, HttpStatusCode statusCode)
            => new(false, errorMessages, statusCode);
    }
}
