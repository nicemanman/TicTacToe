using AutoMapper;
using Server.DataModel;
using Server.DTO;

namespace Server.Mappers;

public class GameMapper : Profile
{
    public GameMapper()
    {
        CreateMap<Game, GameDTO>()
            .ForMember(x => x.GameMap, expression =>
            {
                expression.MapFrom(x=> MapFieldsToNestedDictionary(x.GameMap.Fields));
            });
    }

    private Dictionary<int, Dictionary<int, bool>> MapFieldsToNestedDictionary(List<GameMapField> list)
    {
        Dictionary<int, Dictionary<int, bool>> dict = list
            .GroupBy(row => row.IndexX)
            .ToDictionary(row => row.Key,
                row => row.ToDictionary(column => column.IndexY,
                    cell => cell.IsX));

        return dict;
    }
}