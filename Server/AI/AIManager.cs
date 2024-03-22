using AutoMapper;
using TicTacToeAI.AI;
using TicTacToeAI.AI.Interfaces;
using TicTacToeAI.DataModel;
using AIGame = TicTacToeAI.DataModel.Game;
using Game = Server.DataModel.Game;

namespace Server.AI;

public class AiManager : IAiManager
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
        return _mapper.Map<AIGame, Game>(aiGame);
    }
}