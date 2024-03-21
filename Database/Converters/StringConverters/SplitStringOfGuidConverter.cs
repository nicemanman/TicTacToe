using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Converters.StringConverters;

public abstract class SplitStringOfGuidConverter : ValueConverter<ICollection<Guid>, string>
{
    protected SplitStringOfGuidConverter(char delimiter) : base(
        v => string.Join(delimiter.ToString(), v),
        v => v.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToArray())
    {
    }
}