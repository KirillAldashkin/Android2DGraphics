using Android.Graphics;

namespace GraphicsApp.Graphics;

public class SpriteMap
{
    public Bitmap Bitmap { get; private set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public Point2D SpriteSize { get; private set; }
    private Rect curPos;

    public SpriteMap(Bitmap bitmap, int rows, int columns)
    {
        Bitmap = bitmap;
        Rows = rows;
        Columns = columns;
        SpriteSize = new(bitmap.Width / columns, bitmap.Height / rows);
        curPos = new(0, 0, (int)SpriteSize.X, (int)SpriteSize.Y);
    }

    public Rect this[int x, int y]
    {
        get
        {
            curPos.OffsetTo((int)(SpriteSize.X * x), (int)(SpriteSize.Y * y));
            return curPos;
        }
    }
}