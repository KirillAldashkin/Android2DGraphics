using Android.Graphics;
using GraphicsApp.Graphics;

namespace GraphicsApp.Drawers;

// Рисует бипирамиду
public class BipyramidDrawer : IDrawer
{
    int baseSegments; // Количество рёбер у основания

    Point3D[] rad; // Вершины основания
    Point2D[] projRad; // Спроецированные вершины основания
    Point3D top = new(0, 1, 0), bottom = new(0, -1, 0); // Верхняя и нижняя вершины
    Paint brush = new() { Color = Color.Black }; // Краска

    public BipyramidDrawer(int baseSegments)
    {
        this.baseSegments = baseSegments;
        rad = new Point3D[baseSegments];
        projRad = new Point2D[baseSegments];
    }

    public void Draw(Canvas canvas)
    {
        long time = Environment.TickCount64;
        

        Point2D size = canvas.GetSize(),
                projTop = top.Project().ToScreen(size, 0.8f),        // Спроецированная верхняя вершина
                projBottom = bottom.Project().ToScreen(size, 0.8f);  // Спроецированная нижняя вершина

        for (int i = 0; i < baseSegments; i++)
        {
            var angle = (360.0 * i / baseSegments) + (time / 6.0); // Считаем угол
            angle = (angle % 360) / 180 * PI;                      // Ограничиваем и переводим в радианы
            // Вычисляем вершину основания
            (rad[i].X, rad[i].Z) = SinCos(angle);
            rad[i] *= 0.7f;
            // Проецируем
            projRad[i] = (rad[i] * 0.7).Project().ToScreen(size, 0.8f);
        }

        // Отрисовка
        canvas.DrawColor(Color.White);
        CoordinateBaseDrawer.Instance.Draw(canvas);
        for (int i = 0; i < baseSegments; i++)
        {
            canvas.DrawLine(projTop, projRad[i], brush);
            canvas.DrawLine(projBottom, projRad[i], brush);
        }
        canvas.DrawLines(projRad, brush);
    }
}