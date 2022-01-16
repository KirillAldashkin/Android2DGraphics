using Android.Graphics;
using Android.Views;
using GraphicsApp.Drawers;

namespace GraphicsApp;

// Activity, отрисовывающий заданный Activity
[Activity(Label = "@string/app_name")]
public class SurfaceActivity : Activity, ISurfaceHolderCallback
{
    public static IDrawer Drawer { get; set; } // Отрисовщик

    SurfaceView surfaceView;
    ISurfaceHolder surfaceHolder;
    bool isDrawThreadRunning;
    Thread drawThread;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        surfaceView = new(this);
        SetContentView(surfaceView);
        surfaceHolder = surfaceView.Holder;
        surfaceHolder.AddCallback(this);
    }

    // Функция отрисовки одного кадра. Вызывается в потоке для отрисовки
    private void DrawCycle()
    {
        try
        {
            while (isDrawThreadRunning)
            {
                Canvas canvas = surfaceHolder.LockCanvas();
                lock (canvas) 
                { 
                    Drawer.Draw(canvas); 
                }
                if (canvas != null) surfaceHolder.UnlockCanvasAndPost(canvas);
            }
        }
        catch (Exception e)
        { 
            Console.WriteLine($"Thread exception: {e}"); 
        }
    }

    #region Callback implementation
    public void SurfaceCreated(ISurfaceHolder holder)
    {
        // Запускем поток
        isDrawThreadRunning = true;
        drawThread = new(DrawCycle);
        drawThread.Start();
    }
    public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) { }
    public void SurfaceDestroyed(ISurfaceHolder holder) => isDrawThreadRunning = false; // Останавливаем поток
    #endregion
}