using Android.Graphics;
using Android.Views;
using GraphicsApp.Graphics;
using GraphicsApp.Filters;
using System.Diagnostics;
using Filter = GraphicsApp.Filters.Filter;

namespace GraphicsApp.Drawers;

// Рисует изображение, ускоряющееся в сторону нажатия
public class ImageDrawer : IDrawer, ITouchListener
{
    private Bitmap bitmap;
    private Stopwatch timer = Stopwatch.StartNew();
    private TimeSpan elapsed;
    private Paint speedPaint = new() { Color = Color.Red },
                  accelPaint = new() { Color = Color.Green };

    private Point2D position, speed, acceleration, minBound, maxBound, bitmapSize, physicalBmpSize;

    public ImageDrawer(Bitmap bitmap)
    {
        this.bitmap = bitmap;
        bitmapSize = new(bitmap.Width / 2, bitmap.Height / 2);
    }

    public void Draw(Canvas canvas)
    {
        double delta = ((double)(timer.Elapsed - elapsed).Ticks) / Stopwatch.Frequency;
        elapsed = timer.Elapsed;
        
        Point2D size = canvas.GetSize(), target = touchPoint.FromScreen(size);
        
        minBound = new Point2D(0, 0).FromScreen(size);
        maxBound = size.FromScreen(size);
        physicalBmpSize = bitmapSize / Min(size.X, size.Y);

        // Обновляем скорость, ускорение и положение
        speed *= Pow(0.01, delta);
        acceleration = pressed ? (target - position) * 0.00025 * Pow(0.05, delta) : new Point2D(0, 0);
        speed += acceleration;
        position += speed;

        CheckBounds();

        // Отрисовка
        Point2D bitmapPos = (position - physicalBmpSize / 2).ToScreen(size);
        canvas.DrawColor(Color.White);
        canvas.DrawBitmap(bitmap, (float)bitmapPos.X, (float)bitmapPos.Y, null);
    }

    private void CheckBounds()
    {
        if(position.X < minBound.X + physicalBmpSize.X / 2)
        {
            position.X = minBound.X + physicalBmpSize.X / 2;
            speed.X *= -1;
        }
        if(position.X > maxBound.X - physicalBmpSize.X / 2)
        {
            position.X = maxBound.X - physicalBmpSize.X / 2;
            speed.X *= -1;
        }

        if (position.Y < minBound.Y + physicalBmpSize.Y / 2)
        {
            position.Y = minBound.Y + physicalBmpSize.Y / 2;
            speed.Y *= -1;
        }
        if (position.Y > maxBound.Y - physicalBmpSize.Y / 2)
        {
            position.Y = maxBound.Y - physicalBmpSize.Y / 2;
            speed.Y *= -1;
        }
    }

    #region Реализация ITouchListener
    private Filter xFilter = new MedianFilter(5).Chain(new ExponentialFilter(0.95)),
               yFilter = new MedianFilter(5).Chain(new ExponentialFilter(0.95));
    private Point2D touchPoint;
    private int touchID;
    private bool pressed;
    public bool TouchOccured(MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down)
        {
            pressed = true;
            touchID = e.GetPointerId(e.ActionIndex);
            touchPoint = new(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
            return true;
        }
        else if (e.Action == MotionEventActions.Up)
        {
            pressed = false;
            return true;
        }
        else if (e.Action == MotionEventActions.Move)
        {
            int index = e.FindPointerIndex(touchID);
            if (index == -1) return false;
            touchPoint = new(
                xFilter.Put(e.GetX(index)),
                yFilter.Put(e.GetY(index))
            );
            return true;
        }
        return false;
    }
    #endregion
}
