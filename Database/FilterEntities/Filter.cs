namespace Database.FilterEntities;

/// <summary>
/// Класс фильтра, который содержит список критериев фильтрации
/// </summary>
/// <remarks>CloudWMS-16</remarks>
/// <author>m.shkodin@axelot.ru</author>
public class Filter
{
    public Filter()
    {
        Criteria = new List<FindCriteria>();
    }

    public List<FindCriteria> Criteria { get; set; }

    private FindCriteria GetFindCriteria(string field, object value, Comparator comparator)
    {
        return new FindCriteria()
        {
            Comparator = comparator,
            Field = field,
            Value = value
        };
    }

    public Filter Equals(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.Equals));

        return this;
    }

    public Filter GreaterThen(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.GreaterThan));

        return this;
    }


    public Filter GreaterOrEquals(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.GreaterThanOrEquals));

        return this;
    }

    public Filter LessThen(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.LessThan));

        return this;
    }

    public Filter LessOrEquals(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.LessThanOrEquals));

        return this;
    }

    public Filter Like(string field, object value)
    {
        Criteria.Add(GetFindCriteria(field, value, Comparator.Like));

        return this;
    }
}