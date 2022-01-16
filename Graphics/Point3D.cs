using System.Diagnostics;

namespace GraphicsApp.Graphics;

// Точка в трёхмерном пространстве
[DebuggerDisplay("{ToString(),nq}")]
public struct Point3D
{
    // Константа проецирования, см. метод Project()
    static readonly (double x, double y) project = (0.25, 0.05);

    public double X; // Координата X или ширина
    public double Y; // Координата Y или высота
    public double Z; // Координата Z или глубина

    // Расстояние до (0;0;0)
    public double Length => Sqrt(X * X + Y * Y + Z * Z);

    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // Нормализует данную точку
    public Point3D Normalize() => this / Length;

    // Диметрически проецирует данную точку на двумерное пространство
    public Point2D Project() => new(X + Z * project.x, Y - Z * project.y);

    // Переводит точку в сферическую систему координат
    public (double Angle1, double Angle2) ToRotation() => (Acos(Z / Length), Atan2(X, Y));

    public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Point3D operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Point3D operator *(Point3D a, double f) => new(a.X * f, a.Y * f, a.Z * f);
    public static Point3D operator /(Point3D a, double f) => new(a.X / f, a.Y / f, a.Z / f);

    public override string ToString() => $"(X:{X:F3} | Y:{Y:F3} | Z:{Z:F3})";
}