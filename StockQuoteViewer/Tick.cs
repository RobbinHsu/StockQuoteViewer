namespace StockQuoteViewer;

public static class Tick
{
    public static decimal TickUp(decimal price)
    {
        var tickUp = 0m;

        if (price is < 10)
        {
            tickUp = 0.01m;
        }

        if (price is >= 10 and < 50)
        {
            tickUp = 0.05m;
        }

        if (price is >= 50 and < 100)
        {
            tickUp = 0.1m;
        }

        if (price is >= 100 and < 500)
        {
            tickUp = 0.5m;
        }

        if (price is >= 500 and < 1000)
        {
            tickUp = 1;
        }

        if (price >= 1000)
        {
            tickUp = 5;
        }

        return price + tickUp;
    }

    public static decimal TickDown(decimal price)
    {
        var tickUp = 0m;

        if (price is <= 10)
        {
            tickUp = 0.01m;
        }

        if (price is > 10 and <= 50)
        {
            tickUp = 0.05m;
        }

        if (price is > 50 and <= 100)
        {
            tickUp = 0.1m;
        }

        if (price is > 100 and <= 500)
        {
            tickUp = 0.5m;
        }

        if (price is > 500 and <= 1000)
        {
            tickUp = 1;
        }

        if (price > 1000)
        {
            tickUp = 5;
        }

        return price - tickUp;
    }
}