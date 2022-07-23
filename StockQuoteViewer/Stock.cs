using Microsoft.VisualBasic;

namespace StockQuoteViewer;

public class Stock
{
    private decimal _limitDown;
    private decimal _limitUp;
    private Random _random = new();
    private decimal _startPrice;
    public string Name { get; }
    public decimal Price { get; private set; }
    public string Amplitude { get; private set; }

    public Stock(string name, decimal startPrice)
    {
        Name = name;
        _startPrice = startPrice;
        Price = startPrice;

        Start();
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
        return  next % 2 == 0 ? Tick.TickUp(Price) : Tick.TickDown(Price);
    }

    private string GetAmplitude()
    {
        return $"{Math.Round((Price / _startPrice - 1) * 100, 2)}%";
    }
}