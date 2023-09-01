using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GesEmploy.models
{
    public class TimeOnlyEFConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyEFConverter() : base(
             t => t.ToTimeSpan(),
             ts => TimeOnly.FromTimeSpan(ts))
        { }
    }
}
