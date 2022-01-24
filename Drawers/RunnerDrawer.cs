using Android.Graphics;
using Android.Views;
using GraphicsApp.Graphics;

namespace GraphicsApp.Drawers;

public class RunnerDrawer : IDrawer, ITouchListener
{
    private Bitmap sprite, background;
    private SpriteMap spriteMap;
    private PressController pressController = new();
    private CharacterState characterState = CharacterState.Running;
    private (int X, int Y) spriteIndex = (1, 0);
    private Rect characterDst, backgroundDst, backgroundSrc;

    public RunnerDrawer(Bitmap sprite, Bitmap background)
    {
        this.sprite = sprite;
        this.background = background;
        spriteMap = new(sprite, 3, 8);
        characterDst = new(0, 0, (int)spriteMap.SpriteSize.X / 4, (int)spriteMap.SpriteSize.Y / 4);
        backgroundSrc = new(0, 0, background.Width/2, background.Height);
        backgroundDst = new(0, 0, 0, 0);
    }

    long lastSpriteTime = 0;
    long lastTime = 0;
    double delta = 0, characterElevation = 0, characterX = 0;
    double jumpSpeed = 32, backSpeed = 16, flySpeed = 64;
    public void Draw(Canvas canvas)
    {
        Point2D size = canvas.GetSize();
        if(lastTime != 0) delta = (Environment.TickCount64 - lastTime) / 1000.0;
        lastTime = Environment.TickCount64;

        // Фон
        backgroundDst.Set(0, 0, (int)size.X, (int)size.Y);

        backgroundSrc.Set(backgroundSrc.Left,
                          backgroundSrc.Top,
                          backgroundSrc.Left + (int)(size.X / size.Y * background.Height),
                          backgroundSrc.Bottom);

        double offset = delta * background.Width / 8;
        backgroundSrc.Offset((int)offset, 0);
        if (backgroundSrc.Left >= background.Width / 2) 
            backgroundSrc.Offset(-background.Width / 2, 0);

        // Персонаж
        characterState = pressController.Update() switch
        {
            PressController.State.Click => CharacterState.Jumping,
            PressController.State.HoldStart => CharacterState.Flying,
            PressController.State.HoldEnd => CharacterState.Running,
            _ => characterState
        };
        if(Environment.TickCount64 - lastSpriteTime > 125)
        {
            lastSpriteTime = Environment.TickCount64;
            UpdateSprite();
        }

        // Положение персонажа
        double factor = Pow(0.2, delta), maxX = size.X - characterDst.Width();
        characterElevation = characterState switch
        {
            CharacterState.Flying => characterDst.Height() * (1 - factor) + characterElevation * factor,
            _ => characterElevation * Pow(factor, 4)
        };

        characterX = characterState switch
        {
            CharacterState.Jumping => Min(maxX, characterX + jumpSpeed * delta),
            CharacterState.Flying => Min(maxX, characterX + flySpeed * delta),
            _ => Max(0, characterX - backSpeed * delta)
        };

        characterDst.OffsetTo((int)characterX, (int)(size.Y - characterDst.Height() - characterElevation));

        // Отрисовка
        canvas.DrawBitmap(background, backgroundSrc, backgroundDst, null);
        canvas.DrawBitmap(sprite, spriteMap[spriteIndex.X, spriteIndex.Y], characterDst, null);
    }

    private void UpdateSprite()
    {
        switch (characterState)
        {
            // Переключились на бег из полёта. Включаем первый спрайт полёта (как спрайт приземления)
            case CharacterState.Running when spriteIndex.Y == 2 && spriteIndex.X != 0:
                spriteIndex = (2, 0);
                break;
            // Переключились на прыжок, но текущий спрайт - не прыжок. Включаем первый спрайт прыжка
            case CharacterState.Jumping when spriteIndex.Y != 0:
                spriteIndex = (0, 0);
                break;
            // Переключились на бег, но текущий спрайт - не бег. Включаем первый спрайт бега
            case CharacterState.Running when spriteIndex.Y != 1:
                spriteIndex = (0, 1);
                break;
            // Переключились на полёт, но текущий спрайт - не полёт. Включаем первый спрайт полёта
            case CharacterState.Flying when spriteIndex.Y != 2:
                spriteIndex = (0, 2);
                break;
            // Достигли последнего спрайта прыжка. Переходим на бег, включаем первый спрайт бега.
            case CharacterState.Jumping when spriteIndex == (3, 0):
                characterState = CharacterState.Running;
                spriteIndex = (0, 1);
                break;
            // Достигли последнего спрайта бега. Включаем первый спрайт бега.
            case CharacterState.Running when spriteIndex == (6, 1):
                spriteIndex = (0, 1);
                break;
            // Достигли последнего спрайта полёта. Включаем второй спрайт полёта (первый - это взлёт).
            case CharacterState.Flying when spriteIndex == (4, 2):
                spriteIndex = (1, 2);
                break;
            // Иначе просто переходим на следующий спрайт.
            default:
                spriteIndex.X++;
                break;
        }
    }

    public bool TouchOccured(MotionEvent e)
    {
        if(e.Action == MotionEventActions.Down)
        {
            pressController.Pressed();
            return true;
        }
        else if(e.Action == MotionEventActions.Up)
        {
            pressController.Released();
            return true;
        }
        return false;
    }

    enum CharacterState
    {
        Running, Jumping, Flying
    }
}