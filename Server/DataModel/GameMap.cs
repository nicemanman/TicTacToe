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
                    Row = w,
                    Column = h,
                    Char = ""
                });
            }
        }
    }
    
    public List<GameMapField> Fields { get; set; }
}