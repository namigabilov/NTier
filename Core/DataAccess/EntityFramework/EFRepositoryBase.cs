using System.Linq.Expressions;
using System.Net;
using Core.Entities;
using Core.Entities.DTOs;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TDto, TContext> : IRepositoryBase<TEntity, TDto, TContext>
        where TEntity : class, IEntity
        where TDto : class, IDto, new()
        where TContext : DbContext, new()
    {
        private readonly IStringLocalizer<Localizer> _localizer;

        public EFRepositoryBase(IStringLocalizer<Localizer> localizer)
        {
            _localizer = localizer;
        }

        public async Task<IResult> AddAsync(TEntity entity, TContext context)
        {
            try
            {
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                return new SuccessResult(_localizer["create"], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }

        }

        public async Task<IDataResult<TDto>> Detail(TEntity entity)
        {
            try
            {
                TDto data = entity.MapReverse<TDto, TEntity>();

                return new SuccessDataResult<TDto>(data, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TDto>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Remove(TEntity entity, TContext context)
        {
            try
            {
                var removeEntity = context.Entry(entity);

                context.Set<TEntity>().Remove(entity);

                await context.SaveChangesAsync();

                return new SuccessResult(_localizer["delete"], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(TEntity entity, TContext context)
        {
            try
            {
                var updateEntity = context.Entry(entity);

                context.Set<TEntity>().Update(entity);

                await context.SaveChangesAsync();

                return new SuccessResult(_localizer["update"], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }


        public async Task<IDataResult<BaseDto<TDto>>> GetAllAsync(IQueryable<TEntity> baseQuery, DtoFilter<TEntity, TDto> limitter, Func<TDto, bool> expression = null)
        {
            try
            {
                var con = await baseQuery
                    .Select(limitter.Selector)
                    .Skip((limitter.Page - 1) * limitter.Limit)
                    .Take(limitter.Limit)
                    .ToListAsync();

                if (expression is not null)
                {
                    con = con.Where(expression).ToList();
                }

                int totalRecords = await EntityCount(baseQuery);

                var pageCount = Math.Ceiling((double)totalRecords / limitter.Limit);
                pageCount = pageCount == 0 ? 1 : pageCount;
                BaseDto<TDto> data = new BaseDto<TDto>
                {
                    CurrentPage = limitter.Page,
                    PageCount = (int)pageCount,
                    Data = con
                };

                return new SuccessDataResult<BaseDto<TDto>>(data, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return new ErrorDataResult<BaseDto<TDto>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }


        public async Task<IDataResult<List<TDto>>> GetAllAsync(IQueryable<TEntity> entities, Expression<Func<TEntity, TDto>> selector, Expression<Func<TEntity, bool>> expression = null)
        {
            try
            {
                IQueryable<TDto> query;

                if (entities is null)
                {
                    return new SuccessDataResult<List<TDto>>(new List<TDto>(), HttpStatusCode.OK);
                }

                if (expression == null)
                {
                    query = entities.Select(selector);
                }
                else
                {
                    query = entities.Where(expression).Select(selector);
                }

                var data = await query.ToListAsync();

                return new SuccessDataResult<List<TDto>>(data, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TDto>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        private async Task<int> EntityCount(IQueryable<TEntity> entities)
        {
            return await entities.CountAsync();
        }
    }

}
