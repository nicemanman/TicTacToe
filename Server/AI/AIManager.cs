using AutoMapper;
using ArtificialIntelligence.AI;
using ArtificialIntelligence.AI.Interfaces;
using ArtificialIntelligence.DataModel;
using AIGame = ArtificialIntelligence.DataModel.Game;
using Game = Server.DataModel.Game;

namespace Server.AI;

/// <summary>
/// Реализация оппонента-искуственного интеллекта в игре крестики-нолики
/// </summary>
public class AiManager : IOpponentManager
{
    private readonly IMapper _mapper;
    private readonly IBot _bot;

    public AiManager(IMapper mapper, IBot bot)
    {
        _mapper = mapper;
        _bot = bot;
    }
    
    public Game MakeMove(Game game)
    {
        AIGame aiGame = _mapper.Map<Game, AIGame>(game);
        _bot.MakeMove(aiGame);
        _mapper.Map(aiGame, game);

        //TODO: если мапить поля через автомаппер, то теряется значение ShadowProperty GameMapField.Id, пока костыльнул, потом поправлю
        foreach (var cell in aiGame.Board.GetCells())
        {
            var field = game.GameMap.Fields.FirstOrDefault(x => x.Row == cell.Row && x.Column == cell.Column);
            field.Char = cell.Char;
        }
        
        return game;
    }
}