namespace GraphicsApp.Filters;

// Экспоненциальный фильтр, эффективен при фильтрации малых постоянных шумов.
public class ExponentialFilter : Filter
{
    private double factor;

    public ExponentialFilter(double factor) => this.factor = factor;


    public override double Put(double value) => CurrentValue = (value * factor) + (CurrentValue * (1 - factor));
}