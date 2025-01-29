using System.Reflection;
using Core.Entities;
using Core.Entities.DTOs;

namespace Core.Utilities.Static
{
    public static class CoreMapper
    {
        private static readonly Dictionary<Type, PropertyInfo[]> PropertyCache = new();

        private static PropertyInfo[] GetProperties(Type type)
        {
            if (!PropertyCache.TryGetValue(type, out var properties))
            {
                properties = type.GetProperties();
                PropertyCache[type] = properties;
            }
            return properties;
        }

        public static T Map<T, TDto>(this TDto dto)
            where T : class, IEntity, new()
            where TDto : class, IDto
        {
            T entity = new T();

            var dtoProperties = GetProperties(typeof(TDto));
            var entityProperties = GetProperties(typeof(T));

            var entityPropertyDict = entityProperties
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            foreach (var dtoProp in dtoProperties)
            {
                if (entityPropertyDict.TryGetValue(dtoProp.Name, out var matchingProperty) &&
                    matchingProperty.PropertyType == dtoProp.PropertyType)
                {
                    var value = dtoProp.GetValue(dto);

                    matchingProperty.SetValue(entity, value);
                }
            }

            return entity;
        }


        public static TDto MapReverse<TDto, T>(this T entity)
           where T : class, IEntity
           where TDto : class, IDto, new()
        {
            TDto dto = new TDto();

            var dtoProperties = GetProperties(typeof(TDto));
            var entityProperties = GetProperties(typeof(T));

            var dtoPropertyDict = dtoProperties
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            foreach (var Prop in entityProperties)
            {
                if (dtoPropertyDict.TryGetValue(Prop.Name, out var matchingProperty) &&
                    matchingProperty.PropertyType == Prop.PropertyType)
                {
                    var value = Prop.GetValue(entity);

                    matchingProperty.SetValue(dto, value);
                }
            }

            return dto;
        }
    }
}