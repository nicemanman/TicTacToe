using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Comparers;

public class SplitStringOfGuidComparer : ValueComparer<ICollection<Guid>>
{
    public SplitStringOfGuidComparer() : base(
        (c1, c2) => new HashSet<Guid>(c1!).SetEquals(new HashSet<Guid>(c2!)),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())))
    {
    }
}