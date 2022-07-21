using System.ComponentModel;

namespace StockQuoteViewer;

public class ViewSource
{
    public readonly BindingList<Stock> Stocks = new();

    public ViewSource()
    {
        InitialData();
    }

    private void InitialData()
    {
        var random = new Random();
        for (var i = 0; i < 200; i++)
        {
            Stocks.Add(new Stock(i.ToString("0000"), Convert.ToDecimal(random.Next(1, 150))));
        }
    }
}