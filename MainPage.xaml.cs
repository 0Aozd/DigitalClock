
namespace DigitalClock;

public partial class MainPage : ContentPage
{
    private readonly DigitalClockDrawable clockDrawable;

    private int lastSecond = -1;

    public MainPage()
    {
        InitializeComponent();

        clockDrawable = new DigitalClockDrawable();

        ClockGraphics.Drawable = clockDrawable;

        UpdateClock();

        // проверяем время каждые 100 миллисекунд.
        // Перерисовка выполняется при смене секунды.
        Dispatcher.StartTimer(
            TimeSpan.FromMilliseconds(100),
            () =>
            {
                DateTime now = DateTime.Now;

                if (now.Second != lastSecond)
                {
                    UpdateClock();
                }

                return true;
            }
        );
    }

    private void UpdateClock()
    {
        DateTime now = DateTime.Now;

        lastSecond = now.Second;

        clockDrawable.SetTime(now);

        ClockGraphics.Invalidate();
    }
}
