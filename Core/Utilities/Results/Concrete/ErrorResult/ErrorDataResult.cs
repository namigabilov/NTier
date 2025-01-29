using System.Net;

namespace Core.Utilities.Results.Concrete.ErrorResult
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(string message, HttpStatusCode statusCode, Exception exception) : base(default, false, message, statusCode, exception)
        {
        }

        public ErrorDataResult(string message, HttpStatusCode statusCode, string errorMessage) : base(default, false, message, statusCode, errorMessage)
        {
        }

        public ErrorDataResult(string message, HttpStatusCode statusCode) : base(default, false, message, statusCode, message)
        {
        }

    }
}
