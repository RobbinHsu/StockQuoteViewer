namespace StockQuoteViewer;

public class Stock
{
    private decimal _limitDown;
    private Random _random = new();
    private decimal _startPrice;
    public string Name { get; }
    public decimal Price { get; private set; }
    public string Amplitude { get; private set; }
    public decimal LimitUp { get; }
    public decimal LimitDown { get; }

    public Stock(string name, decimal startPrice)
    {
        Name = name;
        _startPrice = startPrice;
        Price = startPrice;
        LimitUp = GetLimitUp(startPrice);
        LimitDown = GetLimitDown(startPrice);

        Start();
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

    private void Start()
    {
        Task.Run(async delegate
        {
            while (true)
            {
                Price = TickPrice();

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