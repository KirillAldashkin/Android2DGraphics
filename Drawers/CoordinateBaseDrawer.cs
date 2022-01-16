using Android.Graphics;
using GraphicsApp.Graphics;

namespace GraphicsApp.Drawers;

// Рисует систему координат
public class CoordinateBaseDrawer : IDrawer
{
    public static CoordinateBaseDrawer Instance = new();

    // Единичные точки
    static Point3D left = new(-1, 0, 0), right = new(1, 0, 0), 
                   down = new(0, -1, 0), up    = new(0, 1, 0), 
                   back = new(0, 0, -1), front = new(0, 0, 1);
    // Краски
    static Paint paintX = new() { Color = Color.Red }, 
                 paintY = new() { Color = Color.Green }, 
                 paintZ = new() { Color = Color.Blue };
    
    public void Draw(Canvas canvas)
    {
        Point2D size = canvas.GetSize();
        // Отрисовка
        canvas.DrawLine(left.Project().ToScreen(size, 0.5),
                        right.Project().ToScreen(size, 0.5), paintX);
        canvas.DrawLine(down.Project().ToScreen(size, 0.5),
                        up.Project().ToScreen(size, 0.5), paintY);
        canvas.DrawLine(front.Project().ToScreen(size, 0.5),
                        back.Project().ToScreen(size, 0.5), paintZ);
    }
}