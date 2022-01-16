using Android.Graphics;

namespace GraphicsApp.Drawers;

// Рисует что-либо
public interface IDrawer
{
    // Выполняет отрисовку на заданном canvas'e
    void Draw(Canvas canvas);
}