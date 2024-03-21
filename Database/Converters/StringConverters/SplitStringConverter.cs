using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Converters.StringConverters;

public abstract class SplitStringConverter : ValueConverter<ICollection<string>, string>
{
    protected SplitStringConverter(char delimiter) : base(
        v => string.Join(delimiter.ToString(), v),
        v => v.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries))
    {
    }
}