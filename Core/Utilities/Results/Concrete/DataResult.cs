using Core.Utilities.Results.Abstract; 
using System.Net; 
namespace Core.Utilities.Results.Concrete
{
    public class DataResult<T> : Result, IDataResult<T>
{
    public T Data { get; }   
 
    public DataResult(T data, bool success, string message, HttpStatusCode statusCode) 
        : base(success, message, statusCode)
    {
        Data = data;
    }

    public DataResult(T data, bool success, string message, HttpStatusCode statusCode, Exception ex) 
        : base(success, message, statusCode, ex)
    {
        Data = data;
    }

    public DataResult(T data, bool success, string message, HttpStatusCode statusCode, string errorMessage) 
        : base(success, message, statusCode, errorMessage)
    {
        Data = data;
    }

    public DataResult(T data, bool success, HttpStatusCode statusCode, Exception ex) 
        : base(success, statusCode, ex)
    {
        Data = data;
    }

    public DataResult(T data, bool success, HttpStatusCode statusCode, string errorMessage) 
        : base(success, statusCode, errorMessage)
    {
        Data = data;
    }

    public DataResult(T data, bool success, HttpStatusCode statusCode) 
        : base(success, statusCode)
    {
        Data = data;
    }
}

}
