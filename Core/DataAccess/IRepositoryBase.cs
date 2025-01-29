using System.Linq.Expressions;
using Core.Entities;
using Core.Entities.DTOs;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess
{
    public interface IRepositoryBase<TEntity, TDto, TContext>
       where TEntity : class, IEntity
       where TDto : class, IDto
       where TContext : DbContext, new()
    {
        Task<IResult> AddAsync(TEntity entity, TContext context);
        Task<IResult> Remove(TEntity entity, TContext context);
        Task<IResult> Update(TEntity entity, TContext context);
        Task<IDataResult<TDto>> Detail(TEntity entity);
        Task<IDataResult<BaseDto<TDto>>> GetAllAsync(IQueryable<TEntity> baseQuery, DtoFilter<TEntity, TDto> limitter, Func<TDto, bool> expression = null);
        Task<IDataResult<List<TDto>>> GetAllAsync(IQueryable<TEntity> entities, Expression<Func<TEntity, TDto>> selector, Expression<Func<TEntity, bool>> expression = null);
    }
}
