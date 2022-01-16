namespace GraphicsApp.Filters;

// Позволяет объединять фильтры
public class ChainedFilter : Filter
{
    private Filter filter1, filter2;

    public ChainedFilter(Filter filter1, Filter filter2)
    {
        this.filter1 = filter1;
        this.filter2 = filter2;
    }

    public override double Put(double value) => CurrentValue = filter2.Put(filter1.Put(value));
}