using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JitEvolution.Data.Extensions
{
    internal class PropertyBuilderExtensions
    {
        public static ValueConverter<DateTime, DateTime> UtcKindConversion =>
            new(x => DateTime.SpecifyKind(x, DateTimeKind.Utc), x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
    }
}
