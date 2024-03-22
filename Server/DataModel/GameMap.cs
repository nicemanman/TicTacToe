using Database.Interfaces;

namespace Server.DataModel;

public class GameMap
{
    public GameMap()
    {
        
    }

    public GameMap(int width, int height)
    {
        Fields = new List<GameMapField>();
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                Fields.Add(new GameMapField()
                {
                    IndexX = w,
                    IndexY = h,
                    IsX = false
                });
            }
        }
    }
    
    public List<GameMapField> Fields { get; set; }
}