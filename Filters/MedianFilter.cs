namespace GraphicsApp.Filters;

// Медианный фильтр, эффективен для отсеивания сильных кратковременных всплесков.
public class MedianFilter : Filter
{
    private double[] buffer;
    private int length, bufferPosition;

    public MedianFilter(int length)
    {
        this.length = length;
        buffer = new double[length];
    }

    public override double Put(double value)
    {
        // Добавляем новый элемент в массив, затирая самый старый
        buffer[bufferPosition] = value;
        bufferPosition = (bufferPosition + 1) % length;
        // Считаем медиану массива
        return CurrentValue = buffer.OrderBy(x => x).Skip(length / 2).First();
    }
}