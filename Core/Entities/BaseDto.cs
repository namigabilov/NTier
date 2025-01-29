using Core.Entities.DTOs;

namespace Core.Entities
{
    public class BaseDto<TData>
        where TData : class, IDto
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<TData> Data { get; set; }
    }
}