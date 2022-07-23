namespace StockQuoteViewer;

public class Stock
{
    private decimal _limitDown;
    private Random _random = new();
    decimal _startPrice;
    public string Name { get; }
    public decimal Price { get; private set; }
    public string Amplitude { get; private set; }
    internal decimal LimitUp { get; }
    internal decimal LimitDown { get; }

    public Stock(string name, decimal startPrice)
    {
        Name = name;
        _startPrice = startPrice;
        Price = startPrice;
        //LimitUp = GetLimitUp(startPrice);
        //LimitDown = GetLimitDown(startPrice);

        StartVirtualPrice();
    }

    private static decimal GetLimitDown(decimal startPrice)
    {
        var result = startPrice;
        var limitUp = (startPrice * 0.9m);

        result = RecursiveLimitDown(result, limitUp);

        return result;
    }

    private static decimal RecursiveLimitDown(decimal result, decimal limitUp)
    {
        while (true)
        {
            if (limitUp < result)
            {
                var temp = Tick.TickDown(result);
                if (limitUp < temp)
                {
                    result = temp;
                    continue;
                }

                return result;
            }

            return result;
        }
    }

    private static decimal GetLimitUp(decimal startPrice)
    {
        var result = startPrice;
        var limitUp = (startPrice * 1.1m);

        result = RecursiveLimitUp(result, limitUp);

        return result;
    }

    private static decimal RecursiveLimitUp(decimal result, decimal limitUp)
    {
        while (true)
        {
            if (limitUp > result)
            {
                var temp = Tick.TickUp(result);
                if (limitUp > temp)
                {
                    result = temp;
                    continue;
                }

                return result;
            }

            return result;
        }
    }

    private void StartVirtualPrice()
    {
        Task.Run(async delegate
        {
            var up = Price + Math.Round( Math.Floor(Price * 0.1m),2); 
            var down = Price - Math.Round(Math.Floor(Price * 0.1m), 2);
            while (true)
            {
                var temPrice = TickPrice();
                if (up >= temPrice && temPrice >= down)
                {
                    Price = temPrice;
                }

                Amplitude = GetAmplitude();

                await Task.Delay(1);
            }
        });
    }

    private decimal TickPrice()
    {
        var next = _random.Next(1, 101);
        return next % 2 == 0 ? Tick.TickUp(Price) : Tick.TickDown(Price);
    }

    private string GetAmplitude()
    {
        return $"{Math.Round((Price / _startPrice - 1) * 100, 2)}%";
    }
}