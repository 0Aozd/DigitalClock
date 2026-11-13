
using Microsoft.Maui.Graphics;

namespace DigitalClock;

public class DigitalClockDrawable : IDrawable
{
    private DateTime currentTime = DateTime.Now;

    private readonly Color activeColor =
        Color.FromArgb("#FF2020");

    private readonly Color inactiveColor =
        Color.FromArgb("#301010");

    // Маска сегментов для цифр 0-9
    private readonly bool[][] digits =
    {
        new[] { true, true, true, true, true, true, false },
        new[] { false, true, true, false, false, false, false },
        new[] { true, true, false, true, true, false, true },
        new[] { true, true, true, true, false, false, true },
        new[] { false, true, true, false, false, true, true },
        new[] { true, false, true, true, false, true, true },
        new[] { true, false, true, true, true, true, true },
        new[] { true, true, true, false, false, false, false },
        new[] { true, true, true, true, true, true, true },
        new[] { true, true, true, true, false, true, true }
    };

    public void SetTime(DateTime time)
    {
        currentTime = time;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float width = dirtyRect.Width;
        float height = dirtyRect.Height;

        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        // размер цифр
        float digitHeight = Math.Min(
            height * 0.65f,
            width / 4.4f
        );

        float digitWidth = digitHeight * 0.58f;

        float thickness = digitHeight * 0.11f;

        float gap = digitHeight * 0.08f;

        float colonWidth = digitHeight * 0.18f;

        float totalWidth =
            digitWidth * 6 +
            colonWidth * 2 +
            gap * 7;

        float startX = (width - totalWidth) / 2f;
        float startY = (height - digitHeight) / 2f;

        string time = currentTime.ToString("HHmmss");

        float x = startX;

        for (int i = 0; i < 6; i++)
        {
            DrawDigit(
                canvas,
                int.Parse(time[i].ToString()),
                x,
                startY,
                digitWidth,
                digitHeight,
                thickness
            );

            x += digitWidth + gap;

            // Разделительные точки
            if (i == 1 || i == 3)
            {
                DrawColon(
                    canvas,
                    x,
                    startY,
                    digitHeight,
                    colonWidth
                );

                x += colonWidth + gap;
            }
        }
    }

    private void DrawDigit(
        ICanvas canvas,
        int number,
        float x,
        float y,
        float w,
        float h,
        float t)
    {
        float middle = h / 2f;

        // Позиция семи сегментов на экране
        RectF[] segments =
        {
            // A - верхний
            new RectF(x + t, y, w - 2*t, t),

            // B - верхний правый
            new RectF(x + w - t, y + t, t, middle - t),

            // C - нижний правый
            new RectF(x + w - t, y + middle, t, middle - t),

            // D - нижний
            new RectF(x + t, y + h - t, w - 2*t, t),

            // E - нижний левый
            new RectF(x, y + middle, t, middle - t),

            // F - верхний левый
            new RectF(x, y + t, t, middle - t),

            // G - центральный
            new RectF(x + t, y + middle - t/2,
                      w - 2*t, t)
        };

        for (int i = 0; i < 7; i++)
        {
            canvas.FillColor =
                digits[number][i]
                ? activeColor
                : inactiveColor;

            RectF rect = segments[i];

            canvas.FillRoundedRectangle(
                rect,
                t / 3f
            );
        }
    }

    private void DrawColon(
        ICanvas canvas,
        float x,
        float y,
        float h,
        float colonWidth)
    {
        canvas.FillColor = activeColor;

        float size = h * 0.07f;

        float centerX = x + colonWidth / 2f;

        // Верхняя точка
        canvas.FillCircle(
            centerX,
            y + h * 0.35f,
            size
        );

        // Нижняя точка
        canvas.FillCircle(
            centerX,
            y + h * 0.65f,
            size
        );
    }
}
