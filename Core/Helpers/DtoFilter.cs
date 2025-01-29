using System.Linq.Expressions;
using Core.Entities;
using Core.Entities.DTOs;

namespace Core.Helpers
{
    public class DtoFilter<TEntity, TDto>
        where TEntity : class, IEntity
        where TDto : class, IDto
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public Expression<Func<TEntity, TDto>> Selector { get; set; }
    }
}