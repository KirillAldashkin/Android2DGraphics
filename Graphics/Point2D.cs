using System.Diagnostics;

namespace GraphicsApp.Graphics;

// Точка в двухмерном пространстве
[DebuggerDisplay("{ToString(),nq}")]
public struct Point2D
{
    public double X; // Координата X или ширина
    public double Y; // Координата Y или высота

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Переносит точку на экран таким образом, что точка (0;0) переносится
    // в центр экрана, а квадрат с вершинами (-1;-1) и (1;1) увеличивается
    // до размера экрана, сохраняя пропорции. Перед этим умножает исходную
    // точку на заданное число.
    public Point2D ToScreen(Point2D screenSize, double factor = 1) => 
        (this * Min(screenSize.X, screenSize.Y) * 0.5 * factor) + (screenSize * 0.5);

    public static Point2D operator +(Point2D a, Point2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Point2D operator -(Point2D a, Point2D b) => new(a.X - b.X, a.Y - b.Y);
    public static Point2D operator *(Point2D a, double f) => new(a.X * f, a.Y * f);

    public override string ToString() => $"(X:{X:F3} | Y:{Y:F3})";
}