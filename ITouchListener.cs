using Android.Views;

namespace GraphicsApp;

// Обрабатывает нажатия
public interface ITouchListener
{
    // Принимает и обрабатывает нажатие
    public bool TouchOccured(MotionEvent e);
}
