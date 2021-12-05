using JitEvolution.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace JitEvolution.Data.Extensions
{
    internal static class EfFilterExtensions
    {
        public static void SetSoftDeleteFilter(this IMutableEntityType entityData)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var methodToCall = typeof(EfFilterExtensions)
                .GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, Array.Empty<object>());
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            entityData.SetQueryFilter((LambdaExpression)filter);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, IBaseEntity
        {
            Expression<Func<TEntity, bool>> filter = x => x.DeletedAt == null;
            return filter;
        }
    }
}
