using Core.Utilities.Results.Abstract;
using System;
using System.Net;

namespace Core.Utilities.Results.Concrete
{
    public class Result : IResult
    {
        public bool Success { get; }

        public string Message { get; }

        public HttpStatusCode StatusCode { get; }

        public Exception? Exception { get; set; }
        public string? ErrorMessage { get; set; }

        public Result(bool success, string message, HttpStatusCode statusCode) : this(success, statusCode)
        {
            Message = message;
        } 

        public Result(bool success, string message, HttpStatusCode statusCode, Exception exception) : this(success, statusCode)
        {
            Message = message;
            Exception = exception;
        }

        public Result(bool success, string message, HttpStatusCode statusCode, string errorMessage) : this(success, statusCode)
        {
            Message = message;
            ErrorMessage = errorMessage;
        }

        public Result(bool success, HttpStatusCode statusCode)
        {
            Success = success;
            StatusCode = statusCode;
        }

        public Result(bool success, HttpStatusCode statusCode, Exception exception)
        {
            Exception = exception;
            Success = success;
            StatusCode = statusCode;
        }

        public Result(bool success, HttpStatusCode statusCode, string errorMessage)
        {
            ErrorMessage = errorMessage;
            Success = success;
            StatusCode = statusCode;
        }
    }
}
