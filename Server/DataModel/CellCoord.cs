namespace Server.DataModel;

public class CellCoord
{
	public int Row { get; set; }
	
	public int Col { get; set; }

	public CellCoord(int row, int col)
	{
		Row = row;
		Col = col;
	}
}