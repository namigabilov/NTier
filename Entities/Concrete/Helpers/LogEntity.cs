using System.Net;
using Core.Entities.DTOs;

namespace Entities.Concrete.Helpers
{
    public class LogEntity : IDto
    {
        public string? IpAddres { get; set; }
        public string? Path { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucces { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionInner { get; set; }
        public Int64 ExecutingTime { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Agent { get; set; }
        public string? StackTrace { get; set; }
    }
}