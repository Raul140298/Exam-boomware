public class Cell
{
	public int y { get; set; }
	public int x { get; set; }
	public bool isEmpty { get; set; }

	public Cell(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.isEmpty = true;
	}
}