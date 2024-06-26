﻿using AutoMapper;
using Server.DataModel;
using Server.DTO;
using ArtificialIntelligence.DataModel;
using ArtificialIntelligence.DataModel.Interfaces;
using AIGame = ArtificialIntelligence.DataModel.Game;
using Game = Server.DataModel.Game;

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
        
        CreateMap<Game, AIGame>()
            .ForMember(dst => dst.Board, expression =>
            {
                expression.MapFrom(src=> MapFieldsToGameBoard(src.GameMap.Fields));
            });
        
        CreateMap<AIGame, Game>()
            .ForMember(dst => dst.GameMap, expression =>
            {
                expression.Ignore();
            });
    }

    private GameMap MapGameBoardToGameMap(IBoard board)
    {
        GameMap map = new GameMap();
        map.Fields = new List<GameMapField>();
        var cells = board.GetCells();
        foreach (var cell in cells)
        {
            map.Fields.Add(new GameMapField()
            {
                Char = cell.Char.ToString(),
                Column = cell.Column,
                Row = cell.Row
            });
        }

        return map;
    }

    private Board MapFieldsToGameBoard(List<GameMapField> fields)
    {
        return new Board(fields.Select(x => new BoardCell()
        {
            Row = x.Row,
            Column = x.Column,
            Char = x.Char
        }).ToList());
    }

    private Dictionary<int, Dictionary<int, string>> MapFieldsToNestedDictionary(List<GameMapField> list)
    {
        Dictionary<int, Dictionary<int, string>> dict = list
            .GroupBy(row => row.Row)
            .ToDictionary(row => row.Key,
                row => row.ToDictionary(column => column.Column,
                    cell => cell.Char));

        return dict;
    }
}