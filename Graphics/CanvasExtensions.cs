using Android.Graphics;

namespace GraphicsApp.Graphics;

// Всякие расширения для класса Canvas
public static class CanvasExtensions
{
    // Возвращает размер Canvas'а
    public static Point2D GetSize(this Canvas canvas) => new(canvas.Width, canvas.Height);

    // Рисует линию
    public static void DrawLine(this Canvas canvas, Point2D start, Point2D stop, Paint paint) =>
        canvas.DrawLine((float)start.X, (float)start.Y, (float)stop.X, (float)stop.Y, paint);

    // Рисует полигон (замкнутую ломаную)
    public static void DrawLines(this Canvas canvas, Point2D[] points, Paint paint)
    {
        canvas.DrawLine(points[0], points[^1], paint);
        for (int i = 1; i < points.Length; i++) canvas.DrawLine(points[i - 1], points[i], paint);
    }
}