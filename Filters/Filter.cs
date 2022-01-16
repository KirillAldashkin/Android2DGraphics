namespace GraphicsApp.Filters;

// Фильтр потока чисел с плавающей запятой
public abstract class Filter
{
    public double CurrentValue { get; protected set; }

    public abstract double Put(double value);

    public Filter Chain(Filter other) => new ChainedFilter(this, other);
}