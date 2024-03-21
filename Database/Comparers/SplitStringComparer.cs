using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Comparers;

public class SplitStringComparer : ValueComparer<ICollection<string>>
{
    public SplitStringComparer() : base(
        (c1, c2) => new HashSet<string>(c1!).SetEquals(new HashSet<string>(c2!)),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())))
    {
    }
}