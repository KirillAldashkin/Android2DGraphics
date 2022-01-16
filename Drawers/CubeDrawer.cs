using Android.Graphics;
using GraphicsApp.Graphics;

namespace GraphicsApp.Drawers;

// Рисует куб
public class CubeDrawer : IDrawer
{
    Func<Point3D> directionFunction; // Функция для получнения вращения куба
    Paint brush = new() { Color = Color.Black }; // Краска
    Point3D[] top, bottom;   // Массив точек верхней и нижней грани
    Point2D[] pTop, pBottom; // Массив спроецированных точек верхней и нижней грани

    public CubeDrawer(Func<Point3D> directionFunction)
    {
        this.directionFunction = directionFunction;
        top    = new Point3D[] { new(-1, 1,  -1), new(-1, 1,  1), new(1, 1,  1), new(1, 1,  -1) };
        bottom = new Point3D[] { new(-1, -1, -1), new(-1, -1, 1), new(1, -1, 1), new(1, -1, -1) };
        pTop = new Point2D[4];
        pBottom = new Point2D[4];
    }

    public void Draw(Canvas canvas)
    {
        var size = new Point2D(canvas.Width, canvas.Height);
        var (angle1, angle2) = directionFunction().Normalize().ToRotation(); // Получаем углы поворота

        Matrix3x3 m = Matrix3x3.FromRotation(angle1, angle2, 0); // Вычисляем матрицу поворота

        // Вычисляем точки
        for (int i = 0; i < 4; i++)
        {
            pTop[i] = (m * top[i]).Project().ToScreen(size, 0.4);
            pBottom[i] = (m * bottom[i]).Project().ToScreen(size, 0.4);
        }

        // Отрисовка
        canvas.DrawColor(Color.White);
        CoordinateBaseDrawer.Instance.Draw(canvas);
        for (int i = 0; i < 4; i++) canvas.DrawLine(pTop[i], pBottom[i], brush);
        canvas.DrawLines(pTop, brush);
        canvas.DrawLines(pBottom, brush);
    }
}