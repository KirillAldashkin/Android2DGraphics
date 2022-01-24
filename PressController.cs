namespace GraphicsApp;

// Конвертирует события нажатия и отпускания в события клика и удерживания.
public class PressController
{
    public int HoldTime { get; set; } = 250;

    private long lastPress = 0;
    private bool updated = false;
    private bool isPressed = false;
    private bool isHolding = false;

    public void Pressed()
    {
        lastPress = Environment.TickCount64;
        updated = true;
        isPressed = true;
    }

    public void Released()
    {
        updated = true;
        isPressed = false;
    }

    public State Update()
    {
        long now = Environment.TickCount64;
        if (updated)
        {
            updated = false;
            if (!isPressed)
            {
                if (!isHolding) return State.Click;
                isHolding = false;
                return State.HoldEnd;
            }
        }
        else if(isPressed && !isHolding && now - lastPress > HoldTime)
        {
            isHolding = true;
            return State.HoldStart;
        }
        return State.None;
    }

    public enum State
    {
        None, Click, HoldStart, HoldEnd
    }
}