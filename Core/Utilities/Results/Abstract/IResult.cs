using System.Net;

namespace Core.Utilities.Results.Abstract
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
        HttpStatusCode StatusCode { get; }
        Exception? Exception { get; set; }
        string? ErrorMessage { get; set; }
    }
}
