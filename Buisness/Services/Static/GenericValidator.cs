using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using FluentValidation;

namespace Buisness.Services.Static
{
    public static class GenericValidator<T, TValidator>
        where TValidator : IValidator<T>
    {
        public static async Task<IResult> ValidateAsync(T model, TValidator validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage));
                return new ErrorResult(errors, HttpStatusCode.BadRequest, "Validation Error");
            }
            return new SuccessResult(HttpStatusCode.OK);
        }
    }

    public static class GenericDataValidator<T, TModel, TValidator>
        where TValidator : IValidator<T>
    {
        public static async Task<IDataResult<TModel>> ValidateDataAsync(T model, TModel data, TValidator validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage));
                return new ErrorDataResult<TModel>(errors, HttpStatusCode.BadRequest, "Validation Error");
            }
            return new SuccessDataResult<TModel>(data, HttpStatusCode.OK);
        }
    }
}