using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GesEmploy.models
{
    public class DateOnlyEFConverter: ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyEFConverter() : base(
             d => d.ToDateTime(TimeOnly.MinValue),
             dt => DateOnly.FromDateTime(dt))
        { }
    }
}
